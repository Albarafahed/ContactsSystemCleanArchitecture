
using ContactsSystemApplication.Auth;
using ContactsSystemApplication.Interfaces;
using ContactsSystemApplication.Requests;
using ContactsSystemDomain.Entities;
using Microsoft.Extensions.Logging;

namespace ContactsSystemApplication.Services;

public sealed class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IContactRepository _contactRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<AuthService> _logger;

    public AuthService(
        IUserRepository userRepository,
        IContactRepository contactRepository,
        IRefreshTokenRepository refreshTokenRepository,
        IPasswordHasher passwordHasher,
        ITokenService tokenService,
        IUnitOfWork unitOfWork,
        ILogger<AuthService> logger)
    {
        _userRepository = userRepository;
        _contactRepository = contactRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<AuthResponseDto> RegisterAsync(
        RegisterRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(request.Contact);

        ValidatePassword(request.Password);

        if (request.Password != request.ConfirmPassword)
            throw new ArgumentException(
                "Password and confirmation password do not match.");

        var email = request.Contact.Email.Trim();

        if (await _userRepository.EmailExistsAsync(
            email, cancellationToken))
        {
            _logger.LogWarning(
                "Registration failed because the email is already registered.");

            throw new InvalidOperationException(
                "An account with this email already exists.");
        }

        var result = await _unitOfWork.ExecuteInTransactionAsync(
            async ct =>
            {
                var contact = new Contact(
                    request.Contact.FirstName,
                    request.Contact.LastName,
                    email,
                    request.Contact.Phone,
                    request.Contact.Address,
                    request.Contact.DateOfBirth,
                    request.Contact.CountryId,
                    request.Contact.ImagePath);

                await _contactRepository.AddAsync(contact, ct);
                await _contactRepository.SaveChangesAsync(ct);

                var user = new User(
                    contact.ContactId,
                    _passwordHasher.HashPassword(request.Password));

                await _userRepository.AddAsync(user, ct);
                await _userRepository.SaveChangesAsync(ct);

                contact.AssignToUser(user.UserId);

                await _contactRepository.SaveChangesAsync(ct);

                return await CreateAuthResponseAsync(user, ct);
            },
            cancellationToken);

        _logger.LogInformation(
            "User account registered successfully. UserId: {UserId}",
            result.User.UserId);

        return result;
    }

    public async Task<AuthResponseDto> LoginAsync(
        LoginRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (string.IsNullOrWhiteSpace(request.Email) ||
            string.IsNullOrWhiteSpace(request.Password))
        {
            throw new ArgumentException(
                "Email and password are required.");
        }

        var user = await _userRepository.GetByEmailAsync(
            request.Email.Trim(), cancellationToken);

        if (user is null ||
            !_passwordHasher.VerifyPassword(
                request.Password, user.PasswordHash))
        {
            _logger.LogWarning(
                "Login failed due to invalid credentials.");

            throw new UnauthorizedAccessException(
                "Invalid email or password.");
        }

        if (!user.IsActive)
        {
            _logger.LogWarning(
                "Login attempt rejected for inactive account. UserId: {UserId}",
                user.UserId);

            throw new UnauthorizedAccessException(
                "Invalid email or password.");
        }

        var result = await _unitOfWork.ExecuteInTransactionAsync(
            ct => CreateAuthResponseAsync(user, ct),
            cancellationToken);

        _logger.LogInformation(
            "User logged in successfully. UserId: {UserId}",
            user.UserId);

        return result;
    }

    public async Task<AuthResponseDto> RefreshTokenAsync(
        RefreshTokenRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (string.IsNullOrWhiteSpace(request.RefreshToken))
            throw new ArgumentException("Refresh token is required.");

        var tokenHash = _tokenService.HashRefreshToken(
            request.RefreshToken);

        var result = await _unitOfWork.ExecuteInTransactionAsync(
            async ct =>
            {
                var oldToken =
                    await _refreshTokenRepository.TryRevokeActiveAsync(
                        tokenHash,
                        DateTime.UtcNow,
                        ct);

                if (oldToken is null)
                {
                    _logger.LogWarning(
                        "Refresh token renewal rejected: invalid, expired, or revoked token.");

                    throw new UnauthorizedAccessException(
                        "Invalid or expired refresh token.");
                }

                var user = await _userRepository.GetByIdAsync(
                    oldToken.UserId,
                    ct);

                if (user is null || !user.IsActive)
                {
                    return (
                        Response: (AuthResponseDto?)null,
                        AccountUnavailable: true,
                        UserId: oldToken.UserId
                    );
                }

                var response = await CreateAuthResponseAsync(user, ct);

                return (
                    Response: (AuthResponseDto?)response,
                    AccountUnavailable: false,
                    UserId: user.UserId
                );
            },
            cancellationToken);

        if (result.AccountUnavailable)
        {
            _logger.LogWarning(
                "Refresh token renewal rejected for unavailable account. UserId: {UserId}",
                result.UserId);

            throw new UnauthorizedAccessException(
                "This account is unavailable.");
        }

        _logger.LogInformation(
            "Refresh token renewed successfully. UserId: {UserId}",
            result.UserId);

        return result.Response!;
    }

    public async Task LogoutAsync(
        RefreshTokenRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (string.IsNullOrWhiteSpace(request.RefreshToken))
            return;

        var tokenHash = _tokenService.HashRefreshToken(
            request.RefreshToken);

        var revokedToken = await _unitOfWork.ExecuteInTransactionAsync(
            async ct =>
            {
                return await _refreshTokenRepository.TryRevokeActiveAsync(
                    tokenHash,
                    DateTime.UtcNow,
                    ct);
            },
            cancellationToken);

        if (revokedToken is not null)
        {
            _logger.LogInformation(
                "User logged out successfully. UserId: {UserId}",
                revokedToken.UserId);
        }
        else
        {
            _logger.LogDebug(
                "Logout request processed without an active refresh token.");
        }
    }

    private async Task<AuthResponseDto> CreateAuthResponseAsync(
        User user,
        CancellationToken cancellationToken)
    {
        var accessToken = _tokenService.GenerateAccessToken(user);

        var rawRefreshToken = _tokenService.GenerateRefreshToken();

        var refreshTokenHash =
            _tokenService.HashRefreshToken(rawRefreshToken);

        var refreshToken = new RefreshToken(
            user.UserId,
            refreshTokenHash,
            _tokenService.GetRefreshTokenExpiration());

        await _refreshTokenRepository.AddAsync(
            refreshToken, cancellationToken);

        await _refreshTokenRepository.SaveChangesAsync(
            cancellationToken);

        return new AuthResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = rawRefreshToken,
            AccessTokenExpiresAt =
                _tokenService.GetAccessTokenExpiration(),
            User = new UserDto
            {
                UserId = user.UserId,
                ContactId = user.ContactId,
                FirstName = user.ProfileContact.FirstName,
                LastName = user.ProfileContact.LastName,
                Email = user.ProfileContact.Email,
                Role = user.Role,
                IsActive = user.IsActive
            }
        };
    }

    private static void ValidatePassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Password is required.");

        if (password.Length < 8)
            throw new ArgumentException(
                "Password must contain at least 8 characters.");

        if (password.Length > 128)
            throw new ArgumentException(
                "Password cannot exceed 128 characters.");
    }
}

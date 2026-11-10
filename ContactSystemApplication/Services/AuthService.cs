
using ContactsSystemApplication.Auth;
using ContactsSystemApplication.Interfaces;
using ContactsSystemApplication.Requests;
using ContactsSystemDomain.Entities;

namespace ContactsSystemApplication.Services;

public sealed class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IContactRepository _contactRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;
    private readonly IUnitOfWork _unitOfWork;

    public AuthService(
        IUserRepository userRepository,
        IContactRepository contactRepository,
        IRefreshTokenRepository refreshTokenRepository,
        IPasswordHasher passwordHasher,
        ITokenService tokenService,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _contactRepository = contactRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
        _unitOfWork = unitOfWork;
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
            throw new InvalidOperationException(
                "An account with this email already exists.");
        }

        return await _unitOfWork.ExecuteInTransactionAsync(
            async ct =>
            {
                // Create the user's profile contact.
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

                // Create the account with the default User role.
                var user = new User(
                    contact.ContactId,
                    _passwordHasher.HashPassword(request.Password));

                await _userRepository.AddAsync(user, ct);
                await _userRepository.SaveChangesAsync(ct);

                // Assign the profile contact to its account owner.
                contact.AssignToUser(user.UserId);

                await _contactRepository.SaveChangesAsync(ct);

                return await CreateAuthResponseAsync(user, ct);
            },
            cancellationToken);
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
            throw new UnauthorizedAccessException(
                "Invalid email or password.");
        }

        if (!user.IsActive)
        {
            throw new UnauthorizedAccessException(
                "This account is inactive.");
        }

        return await _unitOfWork.ExecuteInTransactionAsync(
            ct => CreateAuthResponseAsync(user, ct),
            cancellationToken);
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
                    throw new UnauthorizedAccessException(
                        "Invalid or expired refresh token.");
                }

                var user = await _userRepository.GetByIdAsync(
                    oldToken.UserId,
                    ct);

                if (user is null || !user.IsActive)
                {
                    // The old token has already been revoked.
                    // Returning allows the transaction to commit.
                    return (
                        Response: (AuthResponseDto?)null,
                        AccountUnavailable: true
                    );
                }

                var response = await CreateAuthResponseAsync(user, ct);

                return (
                    Response: (AuthResponseDto?)response,
                    AccountUnavailable: false
                );
            },
            cancellationToken);

        if (result.AccountUnavailable)
        {
            throw new UnauthorizedAccessException(
                "This account is unavailable.");
        }

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

        await _unitOfWork.ExecuteInTransactionAsync(
            async ct =>
            {
                var token =
                    await _refreshTokenRepository.GetByTokenHashAsync(
                        tokenHash, ct);

                if (token is not null && !token.IsRevoked())
                {
                    token.Revoke();

                    await _refreshTokenRepository.SaveChangesAsync(ct);
                }

                return true;
            },
            cancellationToken);
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
            DateTime.UtcNow.AddDays(7));

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

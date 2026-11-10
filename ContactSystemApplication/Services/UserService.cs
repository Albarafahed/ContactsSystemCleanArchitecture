using ContactsSystemApplication.Interfaces;
using ContactsSystemApplication.Requests;
using ContactsSystemApplication.Users;
using ContactsSystemDomain.Entities;

namespace ContactsSystemApplication.Services;

public sealed class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUnitOfWork _unitOfWork;

    public UserService(
        IUserRepository userRepository,
        IRefreshTokenRepository refreshTokenRepository,
        ICurrentUserService currentUserService,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _currentUserService = currentUserService;
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<UserManagementDto>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        EnsureAdmin();

        var users = await _userRepository.GetAllAsync(cancellationToken);

        return users.Select(MapToDto).ToList();
    }

    public async Task<UserManagementDto?> GetByIdAsync(
        int userId,
        CancellationToken cancellationToken = default)
    {
        EnsureAdmin();
        ValidateUserId(userId);

        var user = await _userRepository.GetByIdAsync(
            userId, cancellationToken);

        return user is null ? null : MapToDto(user);
    }

    public async Task ChangeRoleAsync(
        int userId,
        UpdateUserRoleRequest request,
        CancellationToken cancellationToken = default)
    {
        EnsureAdmin();
        ValidateUserId(userId);

        if (request is null)
            throw new ArgumentNullException(nameof(request));

        var role = request.Role?.Trim();

        if (role is not ("Admin" or "User"))
            throw new ArgumentException(
                "Role must be either Admin or User.");

        var currentUserId = GetCurrentUserId();

        if (userId == currentUserId)
            throw new InvalidOperationException(
                "You cannot change your own role.");

        await _unitOfWork.ExecuteInTransactionAsync(
            async token =>
            {
                var user = await GetRequiredUserAsync(userId, token);

                if (user.Role == role)
                    return true;

                // Do not remove the last active administrator.
                if (user.IsActive &&
                    user.Role == "Admin" &&
                    role == "User")
                {
                    var activeAdmins =
                        await _userRepository.CountActiveAdminsAsync(token);

                    if (activeAdmins <= 1)
                        throw new InvalidOperationException(
                            "The last active administrator cannot be demoted.");
                }

                user.ChangeRole(role);

                await _userRepository.SaveChangesAsync(token);

                return true;
            },
            cancellationToken);
    }

    public async Task ActivateAsync(
        int userId,
        CancellationToken cancellationToken = default)
    {
        EnsureAdmin();
        ValidateUserId(userId);

        await _unitOfWork.ExecuteInTransactionAsync(
            async token =>
            {
                var user = await GetRequiredUserAsync(userId, token);

                if (user.IsActive)
                    return true;

                user.Activate();

                await _userRepository.SaveChangesAsync(token);

                return true;
            },
            cancellationToken);
    }

    public async Task DeactivateAsync(
        int userId,
        CancellationToken cancellationToken = default)
    {
        EnsureAdmin();
        ValidateUserId(userId);

        var currentUserId = GetCurrentUserId();

        if (userId == currentUserId)
            throw new InvalidOperationException(
                "You cannot deactivate your own account.");

        await _unitOfWork.ExecuteInTransactionAsync(
            async token =>
            {
                var user = await GetRequiredUserAsync(userId, token);

                if (!user.IsActive)
                    return true;

                // Do not deactivate the last active administrator.
                if (user.Role == "Admin")
                {
                    var activeAdmins =
                        await _userRepository.CountActiveAdminsAsync(token);

                    if (activeAdmins <= 1)
                        throw new InvalidOperationException(
                            "The last active administrator cannot be deactivated.");
                }

                user.Deactivate();

                var activeTokens =
                    await _refreshTokenRepository.GetActiveByUserIdAsync(
                        userId, token);

                foreach (var refreshToken in activeTokens)
                    refreshToken.Revoke();

                await _userRepository.SaveChangesAsync(token);
                await _refreshTokenRepository.SaveChangesAsync(token);

                return true;
            },
            cancellationToken);
    }

    private void EnsureAdmin()
    {
        if (!_currentUserService.IsAuthenticated ||
            !_currentUserService.IsAdmin)
        {
            throw new UnauthorizedAccessException(
                "Only administrators can manage users.");
        }
    }

    private int GetCurrentUserId()
    {
        return _currentUserService.UserId
            ?? throw new UnauthorizedAccessException(
                "Authenticated user ID was not found.");
    }

    private static void ValidateUserId(int userId)
    {
        if (userId <= 0)
            throw new ArgumentException("Invalid user ID.");
    }

    private async Task<User> GetRequiredUserAsync(
        int userId,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(
            userId, cancellationToken);

        return user ?? throw new KeyNotFoundException(
            $"User with ID {userId} was not found.");
    }

    private static UserManagementDto MapToDto(User user)
    {
        var contact = user.ProfileContact
            ?? throw new InvalidOperationException(
                "User profile contact was not loaded.");

        return new UserManagementDto
        {
            UserId = user.UserId,
            ContactId = user.ContactId,
            FirstName = contact.FirstName,
            LastName = contact.LastName,
            Email = contact.Email,
            Role = user.Role,
            IsActive = user.IsActive
        };
    }
}
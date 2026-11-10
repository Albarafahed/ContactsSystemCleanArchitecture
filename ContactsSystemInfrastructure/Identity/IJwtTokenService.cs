namespace ContactsSystemInfrastructure.Identity;

public interface IJwtTokenService
{
    Task<string> CreateAccessTokenAsync(ApplicationUser user);
}

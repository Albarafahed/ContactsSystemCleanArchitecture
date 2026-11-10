using Microsoft.AspNetCore.Identity;

namespace ContactsSystemInfrastructure.Identity;

public sealed class ApplicationUser : IdentityUser
{
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
}

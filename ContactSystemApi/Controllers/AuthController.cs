using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ContactsSystemInfrastructure.Identity;
using ContactsSystemInfrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ContactsSystemApi.Controllers;

[ApiController]
[Route("api/auth")]
[EnableRateLimiting("auth")]
public sealed class AuthController(
    UserManager<ApplicationUser> userManager,
    RoleManager<IdentityRole> roleManager,
    ContactsDbContext dbContext,
    IJwtTokenService jwtTokenService,
    IOptions<JwtOptions> jwtOptions) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var email = request.Email.Trim();
        var user = new ApplicationUser { UserName = email, Email = email, EmailConfirmed = false, LockoutEnabled = true };
        var created = await userManager.CreateAsync(user, request.Password);
        if (!created.Succeeded)
            return BadRequest(new { errors = created.Errors.Select(error => error.Description) });

        if (!await roleManager.RoleExistsAsync("User"))
        {
            var roleResult = await roleManager.CreateAsync(new IdentityRole("User"));
            if (!roleResult.Succeeded)
            {
                await userManager.DeleteAsync(user);
                return Problem(statusCode: 500, title: "Could not initialize the default user role.");
            }
        }

        var assigned = await userManager.AddToRoleAsync(user, "User");
        if (!assigned.Succeeded)
        {
            await userManager.DeleteAsync(user);
            return Problem(statusCode: 500, title: "Could not assign the default user role.");
        }
        return Ok(await CreateTokenResponseAsync(user));
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var user = await userManager.FindByEmailAsync(request.Email.Trim());
        if (user is null || await userManager.IsLockedOutAsync(user))
            return Unauthorized(new { message = "Invalid email or password, or the account is locked." });

        if (!await userManager.CheckPasswordAsync(user, request.Password))
        {
            await userManager.AccessFailedAsync(user);
            return Unauthorized(new { message = "Invalid email or password, or the account is locked." });
        }

        await userManager.ResetAccessFailedCountAsync(user);
        return Ok(await CreateTokenResponseAsync(user));
    }

    [AllowAnonymous]
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(RefreshRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.RefreshToken))
            return BadRequest(new { message = "Refresh token is required." });

        var now = DateTime.UtcNow;
        var tokenHash = HashToken(request.RefreshToken);
        var storedToken = await dbContext.RefreshTokens.Include(token => token.User)
            .SingleOrDefaultAsync(token => token.TokenHash == tokenHash);
        if (storedToken is null || storedToken.IsRevoked || storedToken.IsExpired)
            return Unauthorized(new { message = "Refresh token is invalid, expired, or revoked." });

        var replacementRaw = GenerateRefreshToken();
        var replacementHash = HashToken(replacementRaw);
        storedToken.RevokedAtUtc = now;
        storedToken.ReplacedByTokenHash = replacementHash;
        storedToken.RowVersion = RandomNumberGenerator.GetBytes(8);

        var accessToken = await jwtTokenService.CreateAccessTokenAsync(storedToken.User);
        dbContext.RefreshTokens.Add(new RefreshToken
        {
            UserId = storedToken.UserId,
            TokenHash = replacementHash,
            CreatedAtUtc = now,
            ExpiresAtUtc = now.AddDays(jwtOptions.Value.RefreshTokenDays)
        });

        try
        {
            await dbContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            return Unauthorized(new { message = "Refresh token has already been used. Please sign in again." });
        }

        return Ok(new AuthResponse(accessToken, replacementRaw,
            now.AddMinutes(jwtOptions.Value.AccessTokenMinutes),
            now.AddDays(jwtOptions.Value.RefreshTokenDays)));
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout(RefreshRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(request.RefreshToken))
            return BadRequest(new { message = "A valid user and refresh token are required." });

        var tokenHash = HashToken(request.RefreshToken);
        var token = await dbContext.RefreshTokens.SingleOrDefaultAsync(item =>
            item.UserId == userId && item.TokenHash == tokenHash && item.RevokedAtUtc == null);
        if (token is not null)
        {
            token.RevokedAtUtc = DateTime.UtcNow;
            await dbContext.SaveChangesAsync();
        }
        return NoContent();
    }

    private async Task<AuthResponse> CreateTokenResponseAsync(ApplicationUser user)
    {
        var now = DateTime.UtcNow;
        var refreshRaw = GenerateRefreshToken();
        var accessToken = await jwtTokenService.CreateAccessTokenAsync(user);

        dbContext.RefreshTokens.Add(new RefreshToken
        {
            UserId = user.Id,
            TokenHash = HashToken(refreshRaw),
            CreatedAtUtc = now,
            ExpiresAtUtc = now.AddDays(jwtOptions.Value.RefreshTokenDays)
        });
        await dbContext.SaveChangesAsync();

        return new AuthResponse(accessToken, refreshRaw,
            now.AddMinutes(jwtOptions.Value.AccessTokenMinutes),
            now.AddDays(jwtOptions.Value.RefreshTokenDays));
    }

    private static string GenerateRefreshToken() =>
        Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

    private static string HashToken(string token) =>
        Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(token)));
}

public sealed record RegisterRequest(
    [param: Required, EmailAddress, StringLength(256)] string Email,
    [param: Required, StringLength(128, MinimumLength = 10)] string Password);
public sealed record LoginRequest(
    [param: Required, EmailAddress, StringLength(256)] string Email,
    [param: Required] string Password);
public sealed record RefreshRequest([param: Required] string RefreshToken);
public sealed record AuthResponse(
    string AccessToken,
    string RefreshToken,
    DateTime AccessTokenExpiresAtUtc,
    DateTime RefreshTokenExpiresAtUtc);

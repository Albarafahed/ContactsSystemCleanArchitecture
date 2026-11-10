using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Xunit;

namespace ContactSystem.Tests.Integration;

[CollectionDefinition("Auth API", DisableParallelization = true)]
public sealed class AuthApiCollection : ICollectionFixture<ContactsApiFactory>;

[Collection("Auth API")]
public sealed class AuthApiTests(ContactsApiFactory factory)
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    [Fact]
    public async Task Register_ThenLogin_ReturnsAccessAndRefreshTokens()
    {
        await factory.InitializeDatabaseAsync();
        var client = factory.CreateClient();
        var email = $"user-{Guid.NewGuid():N}@example.com";
        const string password = "SecurePassword123!";

        var register = await client.PostAsJsonAsync("/api/auth/register",
            new { email, password });
        Assert.Equal(HttpStatusCode.OK, register.StatusCode);
        var registered = await register.Content.ReadFromJsonAsync<AuthPayload>(JsonOptions);
        Assert.NotNull(registered);
        Assert.False(string.IsNullOrWhiteSpace(registered.AccessToken));
        Assert.False(string.IsNullOrWhiteSpace(registered.RefreshToken));

        var login = await client.PostAsJsonAsync("/api/auth/login",
            new { email, password });
        Assert.Equal(HttpStatusCode.OK, login.StatusCode);
        var loggedIn = await login.Content.ReadFromJsonAsync<AuthPayload>(JsonOptions);
        Assert.NotNull(loggedIn);
        Assert.False(string.IsNullOrWhiteSpace(loggedIn.AccessToken));
        Assert.False(string.IsNullOrWhiteSpace(loggedIn.RefreshToken));
    }

    [Fact]
    public async Task Login_WithWrongPassword_ReturnsUnauthorized()
    {
        await factory.InitializeDatabaseAsync();
        var client = factory.CreateClient();
        var email = $"wrong-{Guid.NewGuid():N}@example.com";
        await client.PostAsJsonAsync("/api/auth/register",
            new { email, password = "SecurePassword123!" });

        var response = await client.PostAsJsonAsync("/api/auth/login",
            new { email, password = "WrongPassword123!" });

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Refresh_RotatesToken_AndRejectsThePreviousToken()
    {
        await factory.InitializeDatabaseAsync();
        var client = factory.CreateClient();
        var email = $"refresh-{Guid.NewGuid():N}@example.com";
        var register = await client.PostAsJsonAsync("/api/auth/register",
            new { email, password = "SecurePassword123!" });
        var initial = await register.Content.ReadFromJsonAsync<AuthPayload>(JsonOptions);
        Assert.NotNull(initial);

        var refresh = await client.PostAsJsonAsync("/api/auth/refresh",
            new { refreshToken = initial.RefreshToken });
        Assert.Equal(HttpStatusCode.OK, refresh.StatusCode);
        var rotated = await refresh.Content.ReadFromJsonAsync<AuthPayload>(JsonOptions);
        Assert.NotNull(rotated);
        Assert.NotEqual(initial.RefreshToken, rotated.RefreshToken);

        var reuse = await client.PostAsJsonAsync("/api/auth/refresh",
            new { refreshToken = initial.RefreshToken });
        Assert.Equal(HttpStatusCode.Unauthorized, reuse.StatusCode);
    }

    [Fact]
    public async Task Logout_RevokesRefreshToken()
    {
        await factory.InitializeDatabaseAsync();
        var client = factory.CreateClient();
        var email = $"logout-{Guid.NewGuid():N}@example.com";
        var register = await client.PostAsJsonAsync("/api/auth/register",
            new { email, password = "SecurePassword123!" });
        var tokens = await register.Content.ReadFromJsonAsync<AuthPayload>(JsonOptions);
        Assert.NotNull(tokens);

        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokens.AccessToken);
        var logout = await client.PostAsJsonAsync("/api/auth/logout",
            new { refreshToken = tokens.RefreshToken });
        Assert.Equal(HttpStatusCode.NoContent, logout.StatusCode);

        var refresh = await client.PostAsJsonAsync("/api/auth/refresh",
            new { refreshToken = tokens.RefreshToken });
        Assert.Equal(HttpStatusCode.Unauthorized, refresh.StatusCode);
    }

    private sealed record AuthPayload(
        string AccessToken,
        string RefreshToken,
        DateTime AccessTokenExpiresAtUtc,
        DateTime RefreshTokenExpiresAtUtc);
}

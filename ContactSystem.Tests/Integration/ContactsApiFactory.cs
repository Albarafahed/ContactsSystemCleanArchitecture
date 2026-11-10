using ContactsSystemInfrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ContactSystem.Tests.Integration;

public sealed class ContactsApiFactory : WebApplicationFactory<Program>
{
    private readonly SqliteConnection _connection = new("Data Source=:memory:");
    private bool _initialized;
    private readonly Dictionary<string, string?> _previousEnvironment = new();

    public ContactsApiFactory()
    {
        SetEnvironment("ConnectionStrings__DefaultConnection", "Data Source=integration-tests.db");
        SetEnvironment("Jwt__Issuer", "ContactsSystem.Tests");
        SetEnvironment("Jwt__Audience", "ContactsSystem.Tests.Client");
        SetEnvironment("Jwt__Key", "integration-test-signing-key-at-least-32-bytes-long");
        SetEnvironment("Jwt__AccessTokenMinutes", "15");
        SetEnvironment("Jwt__RefreshTokenDays", "7");
    }

    private void SetEnvironment(string key, string value)
    {
        _previousEnvironment[key] = Environment.GetEnvironmentVariable(key);
        Environment.SetEnvironmentVariable(key, value);
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        _connection.Open();
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<DbContextOptions<ContactsDbContext>>();
            services.RemoveAll<DbContextOptions>();
            services.AddDbContext<ContactsDbContext>(options => options.UseSqlite(_connection));
        });
    }

    public async Task InitializeDatabaseAsync()
    {
        if (_initialized) return;
        using var scope = Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ContactsDbContext>();
        await db.Database.EnsureCreatedAsync();
        _initialized = true;
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (!disposing) return;

        _connection.Dispose();
        foreach (var (key, value) in _previousEnvironment)
            Environment.SetEnvironmentVariable(key, value);
    }
}

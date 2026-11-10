using System.Collections.Concurrent;
using ContactsSystemInfrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ContactSystem.Tests.Integration;

public sealed class ContactsApiFactory : WebApplicationFactory<Program>
{
    private readonly SqliteConnection _connection = new("Data Source=:memory:");
    private bool _initialized;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        _connection.Open();
        builder.ConfigureAppConfiguration((_, configuration) =>
        {
            configuration.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:DefaultConnection"] = "Data Source=integration-tests.db",
                ["Jwt:Issuer"] = "ContactsSystem.Tests",
                ["Jwt:Audience"] = "ContactsSystem.Tests.Client",
                ["Jwt:Key"] = "integration-test-signing-key-at-least-32-bytes-long",
                ["Jwt:AccessTokenMinutes"] = "15",
                ["Jwt:RefreshTokenDays"] = "7"
            });
        });

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
        if (disposing) _connection.Dispose();
    }
}

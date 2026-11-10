using ContactsSystemApplication.Contacts.Interfaces;
using ContactsSystemApplication.Contacts.Services;
using ContactsSystemApplication.Countries.Interfaces;
using ContactsSystemApplication.Countries.Services;
using ContactsSystemApplication.Interfaces;
using ContactsSystemInfrastructure.Identity;
using ContactsSystemInfrastructure.Persistence;
using ContactsSystemInfrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ContactsSystemInfrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidOperationException("Configure ConnectionStrings:DefaultConnection using user secrets or the ConnectionStrings__DefaultConnection environment variable.");

        services.AddDbContext<ContactsDbContext>(options => options.UseSqlServer(connectionString));
        services.Configure<JwtOptions>(options =>
        {
            options.Issuer = configuration["Jwt:Issuer"] ?? string.Empty;
            options.Audience = configuration["Jwt:Audience"] ?? string.Empty;
            options.Key = configuration["Jwt:Key"] ?? string.Empty;
            if (int.TryParse(configuration["Jwt:AccessTokenMinutes"], out var accessMinutes))
                options.AccessTokenMinutes = accessMinutes;
            if (int.TryParse(configuration["Jwt:RefreshTokenDays"], out var refreshDays))
                options.RefreshTokenDays = refreshDays;
        });
        services.AddScoped<IJwtTokenService, JwtTokenService>();

        services.AddIdentityCore<ApplicationUser>(options =>
        {
            options.User.RequireUniqueEmail = true;
            options.Password.RequiredLength = 10;
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
            options.Lockout.AllowedForNewUsers = true;
        })
        .AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<ContactsDbContext>();

        services.AddScoped<IContactRepository, ContactRepository>();
        services.AddScoped<IContactService, ContactService>();
        services.AddScoped<ICountryRepository, CountryRepository>();
        services.AddScoped<ICountryService, CountryService>();
        return services;
    }
}

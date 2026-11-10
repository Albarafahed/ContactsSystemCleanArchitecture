
using ContactsSystemApplication.Countries.Interfaces;
using ContactsSystemApplication.Countries.Services;
using ContactsSystemApplication.Interfaces;
using ContactsSystemApplication.Services;
using ContactsSystemInfrastructure.Persistence;
using ContactsSystemInfrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ContactsSystemInfrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new InvalidOperationException("Configure ConnectionStrings:DefaultConnection using user secrets or the ConnectionStrings__DefaultConnection environment variable.");
            services.AddDbContext<ContactsDbContext>(options => options.UseSqlServer(connectionString));

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IContactRepository, ContactRepository>();
            services.AddScoped<IContactService, ContactService>();

            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<ICountryService, CountryService>();

            return services;
        }
    }
}

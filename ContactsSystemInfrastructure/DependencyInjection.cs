using ContactsSystemApplication.Contacts.Interfaces;
using ContactsSystemInfrastructure.Models;
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


            return services;
        }
    }
}

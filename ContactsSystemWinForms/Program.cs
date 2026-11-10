using System.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ContactsSystemWinForms
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            IConfiguration configuration = new ConfigurationBuilder().SetBasePath(AppContext.BaseDirectory).AddJsonFile("appsettings.json", optional: true).AddEnvironmentVariables().Build();
            var services = new ServiceCollection();
            services.AddSingleton<IConfiguration>(configuration);
            var baseAddress = configuration["Api:BaseUrl"] ?? "http://localhost:5080/";
            services.AddHttpClient<ContactsApiClient>(client => client.BaseAddress = new Uri(baseAddress));
            services.AddHttpClient<CountriesApiClient>(client => client.BaseAddress = new Uri(baseAddress));
            services.AddPresentation();
            using var provider = services.BuildServiceProvider(new ServiceProviderOptions { ValidateOnBuild = true, ValidateScopes = true });
            Application.Run(provider.GetRequiredService<frmListContacts>());
        }
    }
}
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SummaryWebApp.Contracts;
using SummaryWebApp.Services;

namespace SummaryWebApp
{
    /// <summary>
    /// The main entry point for the SummaryWebApp application.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The main method that initializes and runs the web assembly host.
        /// </summary>
        /// <param name="args">The command-line arguments.</param>
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            // Add appsettings.json config
            IConfiguration configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build();

            // Add services
            builder.Services.AddTransient(sp => new HttpClient
            {
                BaseAddress = new Uri(configuration.GetSection("ApiEndpoint").Value)
            });
            builder.Services.AddScoped<IChatService, ChatService>();
            builder.Services.AddScoped<ITokenizerService, TokenizerService>();

            await builder.Build().RunAsync();
        }
    }
}

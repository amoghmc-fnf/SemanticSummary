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
            AddHttpClientService(builder, configuration);
            builder.Services.AddScoped<IChatService, ChatService>();
            builder.Services.AddScoped<ITokenizerService, TokenizerService>();

            await builder.Build().RunAsync();
        }

        /// <summary>
        /// Adds the HTTP client service to the service collection.
        /// </summary>
        /// <param name="builder">The web assembly host builder.</param>
        /// <param name="configuration">The configuration root.</param>
        /// <exception cref="NullReferenceException">Thrown when the value of the key "ApiEndpoint" is null.</exception>
        private static void AddHttpClientService(WebAssemblyHostBuilder builder, IConfiguration configuration)
        {
            var uri = configuration.GetSection("ApiEndpoint").Value ?? throw new NullReferenceException();
            builder.Services.AddTransient(sp => new HttpClient
            {
                BaseAddress = new Uri(uri)
            });
        }
    }
}

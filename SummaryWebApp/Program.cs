using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SummaryWebApp.Services;

namespace SummaryWebApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddTransient(sp => new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5129/api/")
            });
            builder.Services.AddScoped<IChatService, ChatService>();
            builder.Services.AddScoped<ITokenizerService, TokenizerService>();

            await builder.Build().RunAsync();
        }
    }
}

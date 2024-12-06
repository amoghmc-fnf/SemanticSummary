using Microsoft.ML.Tokenizers;
using Microsoft.SemanticKernel;
using Plugins.Models;
using SemanticKernelService.Contracts;
using SemanticKernelService.Services;

namespace SemanticKernelApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            ConfigureCors(builder);

            IConfigurationRoot config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddUserSecrets<Program>()
            .Build();

            // Create a kernel with Azure OpenAI chat completion
            builder.Services.AddSingleton(sp =>
            {
                var kernelBuilder = Kernel.CreateBuilder();
                kernelBuilder.AddAzureOpenAIChatCompletion(
                    config["DeploymentName"],
                    config["Endpoint"],
                    config["Key"]);

                // Add the SettingsPlugin
                var settings = new SettingsPlugin(config);
                kernelBuilder.Plugins.AddFromObject(settings, pluginName: "Settings");

                return kernelBuilder.Build();
            });

            // Add configuration for appsettings
            builder.Services.AddSingleton<IConfiguration>(config);

            // Add API services
            builder.Services.AddSingleton<IChatService, ChatService>();
            builder.Services.AddSingleton<Tokenizer>(TiktokenTokenizer.CreateForModel("gpt-4o-mini"));
            builder.Services.AddSingleton<ITokenizerService, TokenizerService>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("cors");
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }

        private static void ConfigureCors(WebApplicationBuilder builder)
        {
            builder.Services.AddCors(setUpAction =>
            {
                setUpAction.AddPolicy("cors", policy =>
                {
                    policy.AllowAnyHeader();
                    policy.AllowAnyMethod();
                    policy.AllowAnyOrigin();
                });
            });
        }
    }
}

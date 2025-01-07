using Microsoft.Extensions.Logging;
using Microsoft.ML.Tokenizers;
using Microsoft.SemanticKernel;
using Plugins.Plugins;
using SemanticKernelService.Contracts;
using SemanticKernelService.Services;

namespace SemanticKernelApi
{
    /// <summary>
    /// The main entry point for the Semantic Kernel API application.
    /// </summary>
    public class Program
    {
        const string allowAll = "AllowAll";

        /// <summary>
        /// The main method, which is the entry point of the application.
        /// </summary>
        /// <param name="args">The command-line arguments.</param>
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add Cors policy
            ConfigureCors(builder);

            IConfigurationRoot config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddUserSecrets<Program>()
                .Build();

            // Configure logging
            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();

            // Create a kernel with Azure OpenAI chat completion
            builder.Services.AddSingleton(sp =>
            {
                IKernelBuilder kernelBuilder = BuildKernelWithConfig(config);
                return kernelBuilder.Build();
            });

            // Add configuration for appsettings
            builder.Services.AddSingleton<IConfiguration>(config);

            // Add API services
            AddApiServices(builder, config);

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

            app.UseCors(allowAll);
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }

        /// <summary>
        /// Adds API services to the service collection.
        /// </summary>
        /// <param name="builder">The web application builder.</param>
        /// <param name="config">The configuration root.</param>
        /// <exception cref="ArgumentNullException">Thrown when the configuration value is null.</exception>
        private static void AddApiServices(WebApplicationBuilder builder, IConfigurationRoot config)
        {
            builder.Services.AddSingleton<IChatService, ChatService>();
            var model = config["Model"] ?? throw new ArgumentNullException(nameof(config));
            builder.Services.AddSingleton<Tokenizer>(TiktokenTokenizer.CreateForModel(model));
            builder.Services.AddSingleton<ITokenizerService, TokenizerService>();
        }

        /// <summary>
        /// Builds the kernel with the provided configuration.
        /// </summary>
        /// <param name="config">The configuration root.</param>
        /// <returns>An instance of <see cref="IKernelBuilder"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when a required configuration value is null.</exception>
        private static IKernelBuilder BuildKernelWithConfig(IConfigurationRoot config)
        {
            var kernelBuilder = Kernel.CreateBuilder();

            var deploymentName = config["DeploymentName"] ?? throw new ArgumentNullException(nameof(config));
            var endpoint = config["Endpoint"] ?? throw new ArgumentNullException(nameof(config));
            var key = config["Key"] ?? throw new ArgumentNullException(nameof(config));

            kernelBuilder.AddAzureOpenAIChatCompletion(deploymentName, endpoint, key);

            // Add the SettingsPlugin
            var settings = new SettingsPlugin(config);
            kernelBuilder.Plugins.AddFromObject(settings, pluginName: "Settings");
            return kernelBuilder;
        }

        /// <summary>
        /// Configures CORS to allow all origins, headers, and methods.
        /// </summary>
        /// <param name="builder">The web application builder.</param>
        private static void ConfigureCors(WebApplicationBuilder builder)
        {
            builder.Services.AddCors(setUpAction =>
            {
                setUpAction.AddPolicy(allowAll, policy =>
                {
                    policy.AllowAnyHeader();
                    policy.AllowAnyMethod();
                    policy.AllowAnyOrigin();
                });
            });
        }
    }
}
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel;
using Microsoft.Extensions.Configuration;
using SummaryPluginPoc;

var builder = Kernel.CreateBuilder();
IConfigurationRoot config = new ConfigurationBuilder()
            .AddUserSecrets<Program>()
            .Build();

// Create a kernel with Azure OpenAI chat completion
builder.AddAzureOpenAIChatCompletion(
         config["DeploymentName"],
         config["Endpoint"],
         config["Key"]);

// Add enterprise components
// builder.Services.AddLogging(services => services.AddConsole().SetMinimumLevel(LogLevel.Trace));

// Build the kernel
Kernel kernel = builder.Build();
var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

// Add a plugin
kernel.Plugins.AddFromType<SettingsPlugin>(pluginName : "Settings");
//var settings = new SettingsPlugin();
//var summary = new SummaryPlugin(settings);
//kernel.Plugins.AddFromObject(summary, pluginName : "Summary");

// Enable planning
OpenAIPromptExecutionSettings openAIPromptExecutionSettings = new()
{
    ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions
};

// Create a history store the conversation
var history = new ChatHistory();
history.AddSystemMessage("You are a summarizer bot which will be given settings either by user through prompts or by code through kernel.InvokeAsync. Your task is to use the latest updated settings and generate the summary based on the summary prompt. Remember the code could have called kernel.InvokeAsync which always takes precedence over user prompts so DO NOT correct it back to the user prompt");

// Initiate a back-and-forth chat
string? userInput;
do
{
    // Collect user input
    Console.Write("User > ");
    userInput = Console.ReadLine();

    history.AddUserMessage(userInput);

    // Get the response from the AI
    var result = await chatCompletionService.GetChatMessageContentAsync(
        history,
        executionSettings: openAIPromptExecutionSettings,
        kernel: kernel);

    // Print the results
    Console.WriteLine("Assistant > " + result);
    var getTopic = kernel.Plugins.GetFunction("Settings", "get_topic");
    var getLength = kernel.Plugins.GetFunction("Settings", "get_length");
    var getSummary = kernel.Plugins.GetFunction("Settings", "get_summary_prompt");
    var topic = await kernel.InvokeAsync(getTopic);
    var length = await kernel.InvokeAsync(getLength);
    var summary = await kernel.InvokeAsync(getSummary);
    Console.WriteLine("Kernel Settings > " + topic + "\t" + length + "\t" + summary);

    var setTopic = kernel.Plugins.GetFunction("Settings", "set_topic");
    var setLength = kernel.Plugins.GetFunction("Settings", "set_length");
    var setTopicArgs = new KernelArguments();
    var setLengthArgs = new KernelArguments();
    setTopicArgs.Add("newTopic", Topic.Comedy);
    setLengthArgs.Add("newPromptLength", 69);
    await kernel.InvokeAsync(setTopic, setTopicArgs);
    await kernel.InvokeAsync(setLength, setLengthArgs);
    //Console.WriteLine("Kernel Settings after Changing > " + topic + "\t" + length);


    // Add the message from the agent to the chat history
    history.AddMessage(result.Role, result.Content ?? string.Empty);
} while (userInput is not null);
using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Plugins.Models;
using SemanticKernelService.Contracts;

namespace SemanticKernelService.Services
{
    public class ChatService : IChatService
    {
        private readonly Kernel _kernel;
        private readonly ChatHistory _history;
        private readonly IChatCompletionService _chatCompletionService;
        private readonly IConfiguration _configuration;
        private const string settingsPluginName = "Settings";
        private const float temperature = (float) 0.5;

        public ChatService(Kernel kernel, IConfiguration configuration)
        {
            _kernel = kernel;
            _history = [];
            _configuration = configuration;
            InititializeSystemMessage();
            _chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();
        }

        private void InititializeSystemMessage()
        {
            var systemMessage = File.ReadAllText(_configuration["SystemMessage"]);
            _history.AddSystemMessage(systemMessage);
        }

        public async Task<string> GetSummary(string userInput)
        {
            return await GetResultForPrompt(userInput);
        }

        public async Task<string> GetRegeneratedSummary(string userInput)
        {
            _history.AddUserMessage("Regenerate summary for below user prompt: \n");
            return await GetResultForPrompt(userInput, temperature);
        }

        private async Task<string> GetResultForPrompt(string userInput, float temperature = 0)
        {
            _history.AddUserMessage(userInput);

            var openAIPromptExecutionSettings = new OpenAIPromptExecutionSettings
            {
                ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions,
                Temperature = temperature
            };

            var result = await _chatCompletionService.GetChatMessageContentAsync(
                _history,
                executionSettings: openAIPromptExecutionSettings,
                kernel: _kernel);

            _history.AddMessage(result.Role, result.Content ?? string.Empty);
            return result.Content;
        }

        public async Task<string> GetTopic()
        {
            var getTopic = _kernel.Plugins.GetFunction(settingsPluginName, "get_topic");
            var topic = await _kernel.InvokeAsync(getTopic);
            var result = topic.ToString();
            return result;
        }

        public async Task UpdateTopic(Topic newTopic)
        {
            var setTopic = _kernel.Plugins.GetFunction(settingsPluginName, "set_topic");
            var setTopicArgs = new KernelArguments
            {
                { "newTopic", newTopic }
            };
            await _kernel.InvokeAsync(setTopic, setTopicArgs);
            return;
        }

        public async Task<string> GetPromptLength()
        {
            var getLength = _kernel.Plugins.GetFunction(settingsPluginName, "get_length");
            var length = await _kernel.InvokeAsync(getLength);
            var result = length.ToString();
            return result;
        }

        public async Task UpdatePromptLength(int newPromptLength)
        {
            var setLength = _kernel.Plugins.GetFunction(settingsPluginName, "set_length");
            var setLengthArgs = new KernelArguments
            {
                { "newPromptLength", newPromptLength }
            };
            await _kernel.InvokeAsync(setLength, setLengthArgs);
            return;
        }

        public async Task<string> GetSummaryPrompt()
        {
            var getSummaryPrompt = _kernel.Plugins.GetFunction(settingsPluginName, "get_summary_prompt");
            var summaryPrompt = await _kernel.InvokeAsync(getSummaryPrompt);
            var result = summaryPrompt.ToString();
            return result;
        }
    }
}

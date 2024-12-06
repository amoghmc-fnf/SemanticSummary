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

        public ChatService(Kernel kernel, IConfiguration configuration)
        {
            _kernel = kernel;
            _history = new ChatHistory();
            _configuration = configuration;
            _history.AddSystemMessage(_configuration["SystemMessage"]);
            _chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();
        }

        public async Task<string> GetSummary(string userInput)
        {
            _history.AddUserMessage(userInput);

            var openAIPromptExecutionSettings = new OpenAIPromptExecutionSettings
            {
                ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions
            };

            var result = await _chatCompletionService.GetChatMessageContentAsync(
                _history,
                executionSettings: openAIPromptExecutionSettings,
                kernel: _kernel);

            _history.AddMessage(result.Role, result.Content ?? string.Empty);
            return result.Content;
        }

        public async Task<string> GetRegeneratedSummary(string userInput)
        {
            _history.AddUserMessage("Regenerate summary for below user prompt: \n");
            _history.AddUserMessage(userInput);

            var openAIPromptExecutionSettings = new OpenAIPromptExecutionSettings
            {
                ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions,
                Temperature = 0.5
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
            var getTopic = _kernel.Plugins.GetFunction("Settings", "get_topic");
            var topic = await _kernel.InvokeAsync(getTopic);
            var result = topic.ToString();
            return result;
        }

        public async Task UpdateTopic(Topic newTopic)
        {
            var setTopic = _kernel.Plugins.GetFunction("Settings", "set_topic");
            var setTopicArgs = new KernelArguments();
            setTopicArgs.Add("newTopic", newTopic);

            await _kernel.InvokeAsync(setTopic, setTopicArgs);

            var getTopic = _kernel.Plugins.GetFunction("Settings", "get_topic");
            var topic = await _kernel.InvokeAsync(getTopic);
            return;
        }

        public async Task<string> GetPromptLength()
        {
            var getLength = _kernel.Plugins.GetFunction("Settings", "get_length");
            var length = await _kernel.InvokeAsync(getLength);
            var result = length.ToString();
            return result;
        }

        public async Task UpdatePromptLength(int newPromptLength)
        {
            var setLength = _kernel.Plugins.GetFunction("Settings", "set_length");
            var setLengthArgs = new KernelArguments();
            setLengthArgs.Add("newPromptLength", newPromptLength);

            await _kernel.InvokeAsync(setLength, setLengthArgs);

            var getLength = _kernel.Plugins.GetFunction("Settings", "get_length");
            var length = await _kernel.InvokeAsync(getLength);
            return;
        }

        public async Task<string> GetSummaryPrompt()
        {
            var getSummaryPrompt = _kernel.Plugins.GetFunction("Settings", "get_summary_prompt");
            var summaryPrompt = await _kernel.InvokeAsync(getSummaryPrompt);
            var result = summaryPrompt.ToString();
            return result;
        }
    }
}

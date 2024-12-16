using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Plugins.Models;
using SemanticKernelService.Contracts;

namespace SemanticKernelService.Services
{
    /// <summary>
    /// Provides chat-related services using the Semantic Kernel.
    /// </summary>
    public class ChatService : IChatService
    {
        private readonly Kernel _kernel;
        private readonly ChatHistory _history;
        private readonly IChatCompletionService _chatCompletionService;
        private readonly IConfiguration _configuration;
        private const string settingsPluginName = "Settings";
        private const float temperature = (float)0.5;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChatService"/> class.
        /// </summary>
        /// <param name="kernel">The Semantic Kernel instance.</param>
        /// <param name="configuration">The configuration to use for retrieving settings.</param>
        public ChatService(Kernel kernel, IConfiguration configuration)
        {
            ArgumentNullException.ThrowIfNull(nameof(kernel));
            ArgumentNullException.ThrowIfNull(nameof(configuration));
            _kernel = kernel;
            _history = new ChatHistory();
            _configuration = configuration;
            InititializeSystemMessage();
            _chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();
        }

        /// <summary>
        /// Initializes the system message from the configuration.
        /// </summary>
        private void InititializeSystemMessage()
        {
            string systemMessage;
            try
            {
                systemMessage = File.ReadAllText(_configuration["SystemMessage"]);
            }
            catch (ArgumentNullException)
            {
                var systemMessagePath = _configuration["SystemMessage"];
                throw new ArgumentNullException(nameof(systemMessagePath), "Path for file 'SystemMessage' cannot be empty!");
            }
            _history.AddSystemMessage(systemMessage);
        }

        /// <summary>
        /// Gets a summary based on the provided user input.
        /// </summary>
        /// <param name="userInput">The user input to summarize.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the summary.</returns>
        public async Task<string> GetSummary(string userInput)
        {
            return await GetResultForPrompt(userInput);
        }

        /// <summary>
        /// Regenerates a summary based on the provided user input.
        /// </summary>
        /// <param name="userInput">The user input to summarize again.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the regenerated summary.</returns>
        public async Task<string> GetRegeneratedSummary(string userInput)
        {
            _history.AddUserMessage("Regenerate summary for below user prompt: \n");
            return await GetResultForPrompt(userInput, temperature);
        }

        /// <summary>
        /// Gets the result for the provided prompt.
        /// </summary>
        /// <param name="userInput">The user input to process.</param>
        /// <param name="temperature">The temperature setting for the prompt execution.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the result for the prompt.</returns>
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
            if (result.Content == null)
            {
                throw new NullReferenceException("Response cannot be null!");
            }
            return result.Content;
        }

        /// <summary>
        /// Gets the current topic.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains the current topic.</returns>
        public async Task<string> GetTopic()
        {
            var getTopic = _kernel.Plugins.GetFunction(settingsPluginName, "get_topic");
            var topic = await _kernel.InvokeAsync(getTopic);
            var result = topic.ToString();
            return result;
        }

        /// <summary>
        /// Updates the current topic with a new value.
        /// </summary>
        /// <param name="newTopic">The new topic to set.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task UpdateTopic(Topic newTopic)
        {
            var setTopic = _kernel.Plugins.GetFunction(settingsPluginName, "set_topic");
            var setTopicArgs = new KernelArguments
            {
                { "newTopic", newTopic }
            };
            await _kernel.InvokeAsync(setTopic, setTopicArgs);
        }

        /// <summary>
        /// Gets the current prompt length.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains the current prompt length.</returns>
        public async Task<string> GetPromptLength()
        {
            var getLength = _kernel.Plugins.GetFunction(settingsPluginName, "get_length");
            var length = await _kernel.InvokeAsync(getLength);
            var result = length.ToString();
            return result;
        }

        /// <summary>
        /// Updates the prompt length with a new value.
        /// </summary>
        /// <param name="newPromptLength">The new prompt length to set.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task UpdatePromptLength(int newPromptLength)
        {
            var setLength = _kernel.Plugins.GetFunction(settingsPluginName, "set_length");
            var setLengthArgs = new KernelArguments
            {
                { "newPromptLength", newPromptLength }
            };
            await _kernel.InvokeAsync(setLength, setLengthArgs);
        }

        /// <summary>
        /// Gets the prompt for generating a summary.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains the summary prompt.</returns>
        public async Task<string> GetSummaryPrompt()
        {
            var getSummaryPrompt = _kernel.Plugins.GetFunction(settingsPluginName, "get_summary_prompt");
            var summaryPrompt = await _kernel.InvokeAsync(getSummaryPrompt);
            var result = summaryPrompt.ToString();
            return result;
        }
    }
}

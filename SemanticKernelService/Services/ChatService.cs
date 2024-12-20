﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<ChatService> _logger;
        private const string settingsPluginName = "Settings";
        private const float temperature = (float)0.5;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChatService"/> class.
        /// </summary>
        /// <param name="kernel">The Semantic Kernel instance.</param>
        /// <param name="configuration">The configuration to use for retrieving settings.</param>
        /// <param name="logger">The logger to use for logging.</param>
        /// <exception cref="ArgumentNullException">Thrown when the kernel or configuration is null.</exception>
        public ChatService(Kernel kernel, IConfiguration configuration, ILogger<ChatService> logger)
        {
            _kernel = kernel ?? throw new ArgumentNullException(nameof(kernel), "Kernel cannot be null.");
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration), "Configuration cannot be null.");
            _logger = logger ?? throw new ArgumentNullException(nameof(logger), "Logger cannot be null.");
            _history = new ChatHistory();
            InititializeSystemMessage();
            _chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();
            _logger.LogInformation("ChatService initialized.");
        }

        /// <summary>
        /// Initializes the system message from the configuration.
        /// </summary>
        /// <exception cref="NullReferenceException">Thrown when the system message path is null or empty.</exception>
        private void InititializeSystemMessage()
        {
            var systemMessagePath = _configuration["SystemMessage"];
            if (string.IsNullOrEmpty(systemMessagePath))
            {
                _logger.LogError("Path for file 'SystemMessage' cannot be empty.");
                throw new NullReferenceException("Path for file 'SystemMessage' cannot be empty.");
            }
            var systemMessage = File.ReadAllText(systemMessagePath);
            _history.AddSystemMessage(systemMessage);
            _logger.LogInformation("System message initialized.");
        }

        /// <summary>
        /// Gets a summary based on the provided user input.
        /// </summary>
        /// <param name="userInput">The user input to summarize.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the summary.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the user input is null or empty.</exception>
        public async Task<string> GetSummary(string userInput)
        {
            if (string.IsNullOrEmpty(userInput))
            {
                _logger.LogError("User input cannot be null or empty.");
                throw new ArgumentNullException(nameof(userInput), "User input cannot be null or empty.");
            }

            _logger.LogInformation("Generating summary for user input.");
            return await GetResultForPrompt(userInput);
        }

        /// <summary>
        /// Regenerates a summary based on the provided user input.
        /// </summary>
        /// <param name="userInput">The user input to summarize again.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the regenerated summary.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the user input is null or empty.</exception>
        public async Task<string> GetRegeneratedSummary(string userInput)
        {
            if (string.IsNullOrEmpty(userInput))
            {
                _logger.LogError("User input cannot be null or empty.");
                throw new ArgumentNullException(nameof(userInput), "User input cannot be null or empty.");
            }

            _history.AddUserMessage("Regenerate summary for below user prompt: \n");
            _logger.LogInformation("Regenerating summary for user input.");
            return await GetResultForPrompt(userInput, temperature);
        }

        /// <summary>
        /// Gets the result for the provided prompt.
        /// </summary>
        /// <param name="userInput">The user input to process.</param>
        /// <param name="temperature">The temperature setting for the prompt execution.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the result for the prompt.</returns>
        /// <exception cref="NullReferenceException">Thrown when the response content is null.</exception>
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
            if (result.Content is null)
            {
                _logger.LogError("Response cannot be null!");
                throw new NullReferenceException("Response cannot be null!");
            }
            _logger.LogInformation("Generated result for prompt.");
            return result.Content;
        }

        /// <summary>
        /// Gets the current topic.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains the current topic.</returns>
        public async Task<string> GetTopic()
        {
            _logger.LogInformation("Getting current topic.");
            var getTopic = _kernel.Plugins.GetFunction(settingsPluginName, "get_topic");
            var topic = await _kernel.InvokeAsync(getTopic);
            var result = topic.ToString();
            _logger.LogInformation("Current topic: {Topic}", result);
            return result;
        }

        /// <summary>
        /// Updates the current topic with a new value.
        /// </summary>
        /// <param name="newTopic">The new topic to set.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task UpdateTopic(Topic newTopic)
        {
            _logger.LogInformation("Updating topic to {NewTopic}", newTopic);
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
            _logger.LogInformation("Getting current prompt length.");
            var getLength = _kernel.Plugins.GetFunction(settingsPluginName, "get_length");
            var length = await _kernel.InvokeAsync(getLength);
            var result = length.ToString();
            _logger.LogInformation("Current prompt length: {PromptLength}", result);
            return result;
        }

        /// <summary>
        /// Updates the prompt length with a new value.
        /// </summary>
        /// <param name="newPromptLength">The new prompt length to set.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the new prompt length is less than or equal to zero.</exception>
        public async Task UpdatePromptLength(int newPromptLength)
        {
            if (newPromptLength <= 0)
            {
                _logger.LogError("Prompt length must be greater than zero.");
                throw new ArgumentOutOfRangeException(nameof(newPromptLength), "Prompt length must be greater than zero.");
            }

            _logger.LogInformation("Updating prompt length to {NewPromptLength}", newPromptLength);
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
            _logger.LogInformation("Getting summary prompt.");
            var getSummaryPrompt = _kernel.Plugins.GetFunction(settingsPluginName, "get_summary_prompt");
            var summaryPrompt = await _kernel.InvokeAsync(getSummaryPrompt);
            var result = summaryPrompt.ToString();
            _logger.LogInformation("Summary prompt: {SummaryPrompt}", result);
            return result;
        }
    }
}
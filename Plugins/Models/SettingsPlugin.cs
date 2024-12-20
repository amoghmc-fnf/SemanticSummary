using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using System.ComponentModel;

namespace Plugins.Models
{
    /// <summary>
    /// The SettingsPlugin class implements the ISettingsPlugin interface and provides methods to manage settings related to topic and prompt length.
    /// </summary>
    public class SettingsPlugin : ISettingsPlugin
    {
        private Topic _topic;
        private int _promptLength;
        private readonly IConfiguration _configuration;
        private readonly ILogger<SettingsPlugin> _logger;

        /// <summary>
        /// Initializes a new instance of the SettingsPlugin class with the specified configuration and logger.
        /// </summary>
        /// <param name="configuration">The configuration to use for retrieving settings.</param>
        /// <param name="logger">The logger to use for logging.</param>
        public SettingsPlugin(IConfiguration configuration, ILogger<SettingsPlugin> logger)
        {
            _topic = Topic.Generic;
            _promptLength = 10;
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration), "Configuration cannot be null.");
            _logger = logger ?? throw new ArgumentNullException(nameof(logger), "Logger cannot be null.");
            _logger.LogInformation("SettingsPlugin initialized with topic {Topic} and prompt length {PromptLength}", _topic, _promptLength);
        }

        /// <summary>
        /// Gets the latest topic name for the summary.
        /// </summary>
        /// <returns>The current topic.</returns>
        [KernelFunction("get_topic")]
        [Description("Gets the latest topic name for the summary")]
        [return: Description("Topic name")]
        public Topic GetTopic()
        {
            _logger.LogInformation("GetTopic called, returning {Topic}", _topic);
            return _topic;
        }

        /// <summary>
        /// Sets the topic name for the summary.
        /// </summary>
        /// <param name="newTopic">The new topic to set.</param>
        [KernelFunction("set_topic")]
        [Description("Sets the topic name for the summary")]
        public void SetTopic(Topic newTopic)
        {
            _logger.LogInformation("SetTopic called, changing topic from {OldTopic} to {NewTopic}", _topic, newTopic);
            _topic = newTopic;
        }

        /// <summary>
        /// Gets the latest maximum word count for the LLM to output for the summary.
        /// </summary>
        /// <returns>The current prompt length.</returns>
        [KernelFunction("get_length")]
        [Description("Gets the latest maximum word count for the LLM to output for the summary")]
        [return: Description("Output length")]
        public int GetPromptLength()
        {
            _logger.LogInformation("GetPromptLength called, returning {PromptLength}", _promptLength);
            return _promptLength;
        }

        /// <summary>
        /// Sets the maximum word count for the LLM to output for the summary.
        /// </summary>
        /// <param name="newPromptLength">The new maximum word count to set.</param>
        [KernelFunction("set_length")]
        [Description("Sets the maximum word count for the LLM to output for the summary")]
        public void SetPromptLength(int newPromptLength)
        {
            _logger.LogInformation("SetPromptLength called, changing prompt length from {OldPromptLength} to {NewPromptLength}", _promptLength, newPromptLength);
            _promptLength = newPromptLength;
        }

        /// <summary>
        /// Gets the prompt for summary using the latest updated settings for the topic name and prompt length.
        /// </summary>
        /// <returns>The summary prompt.</returns>
        [KernelFunction("get_summary_prompt")]
        [Description("Gets the prompt for summary using the latest updated settings for the topic name and prompt length")]
        public string GetSummaryPrompt()
        {
            _logger.LogInformation("GetSummaryPrompt called");
            string topicPath = GetTopicFilePath();
            string topicDescription = File.ReadAllText(topicPath);
            return $"Summarize the above text for the topic {GetTopic()} in at most {GetPromptLength()} words.\n" +
                $"Use the following description for the topic:\n{topicDescription}";
        }

        /// <summary>
        /// Gets the file path for the topic.
        /// </summary>
        /// <returns>The file path for the topic.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the configuration setting for 'PromptsForTopics' is null or empty.
        /// </exception>
        private string GetTopicFilePath()
        {
            _logger.LogInformation("GetTopicFilePath called");
            var promptsForTopicsPath = _configuration["PromptsForTopics"];
            if (string.IsNullOrEmpty(promptsForTopicsPath))
            {
                _logger.LogError("Path for folder 'PromptsForTopics' cannot be empty!");
                throw new NullReferenceException("Path for folder 'PromptsForTopics' cannot be empty!");
            }
            return Path.Combine(promptsForTopicsPath, $"{GetTopic()}.txt");
        }
    }
}
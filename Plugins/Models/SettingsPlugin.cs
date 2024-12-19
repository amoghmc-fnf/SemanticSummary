using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

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

        /// <summary>
        /// Initializes a new instance of the SettingsPlugin class with the specified configuration.
        /// </summary>
        /// <param name="configuration">The configuration to use for retrieving settings.</param>
        public SettingsPlugin(IConfiguration configuration)
        {
            _topic = Topic.Generic;
            _promptLength = 10;
            try
            {
                _configuration = configuration;
            }
            catch (ArgumentNullException)
            {
                throw new ArgumentException(nameof(configuration), "Configuration cannot be null!");
            }
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
            string topicPath;
            topicPath = GetTopicFilePath();
            string topicDescription = File.ReadAllText(topicPath);
            return $"Summarize the above text for the topic {GetTopic()} " +
                $"in at most {GetPromptLength()} words.\n" +
                $"Use the following description for the topic:\n{topicDescription}";
        }

        /// <summary>
        /// Constructs and returns the file path for a topic based on the configuration settings.
        /// </summary>
        /// <returns>The file path for the topic.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the configuration setting for 'PromptsForTopics' is null or empty.
        /// </exception>
        private string GetTopicFilePath()
        {
            string topicPath;
            try
            {
                topicPath = Path.Combine(_configuration["PromptsForTopics"], $"{GetTopic()}.txt");
            }
            catch (ArgumentNullException)
            {
                var promptsForTopicsPath = _configuration["PromptsForTopics"];
                throw new ArgumentNullException(nameof(promptsForTopicsPath), "Path for folder 'PromptsForTopics' cannot be empty!");
            }
            return topicPath;
        }
    }
}
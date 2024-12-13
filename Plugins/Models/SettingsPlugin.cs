using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;
using System.ComponentModel;

namespace Plugins.Models
{
    /// <summary>
    /// The SettingsPlugin class implements the ISettingsPlugin interface and provides methods to manage settings related to topic and prompt length.
    /// </summary>
    public class SettingsPlugin : ISettingsPlugin
    {
        private Topic topic;
        private int promptLength;
        private IConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the SettingsPlugin class with the specified configuration.
        /// </summary>
        /// <param name="configuration">The configuration to use for retrieving settings.</param>
        public SettingsPlugin(IConfiguration configuration)
        {
            topic = Topic.Generic;
            promptLength = 10;
            this.configuration = configuration;
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
            return topic;
        }

        /// <summary>
        /// Sets the topic name for the summary.
        /// </summary>
        /// <param name="newTopic">The new topic to set.</param>
        [KernelFunction("set_topic")]
        [Description("Sets the topic name for the summary")]
        public void SetTopic(Topic newTopic)
        {
            topic = newTopic;
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
            return promptLength;
        }

        /// <summary>
        /// Sets the maximum word count for the LLM to output for the summary.
        /// </summary>
        /// <param name="newPromptLength">The new maximum word count to set.</param>
        [KernelFunction("set_length")]
        [Description("Sets the maximum word count for the LLM to output for the summary")]
        public void SetPromptLength(int newPromptLength)
        {
            promptLength = newPromptLength;
        }

        /// <summary>
        /// Gets the prompt for summary using the latest updated settings for the topic name and prompt length.
        /// </summary>
        /// <returns>The summary prompt.</returns>
        [KernelFunction("get_summary_prompt")]
        [Description("Gets the prompt for summary using the latest updated settings for the topic name and prompt length")]
        public string GetSummaryPrompt()
        {
            string topicPath = Path.Combine(configuration["PromptsForTopics"], $"{GetTopic()}.txt");
            string topicDescription = File.ReadAllText(topicPath);
            return $"Summarize the above text for the topic {GetTopic()} " +
                $"in at most {GetPromptLength()} words.\n" +
                $"Use the following description for the topic:\n{topicDescription}";
        }
    }
}
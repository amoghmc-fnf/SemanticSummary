using Plugins.Models;

namespace Plugins
{
    /// <summary>
    /// The ISettingsPlugin interface defines methods for managing settings related to prompt length and topic.
    /// Each method provides functionality to get or set specific settings related to the plugin.
    /// </summary>
    public interface ISettingsPlugin
    {
        /// <summary>
        /// Gets the current prompt length.
        /// </summary>
        /// <returns>The current prompt length.</returns>
        int GetPromptLength();

        /// <summary>
        /// Gets the prompt for generating a summary.
        /// </summary>
        /// <returns>The summary prompt.</returns>
        string GetSummaryPrompt();

        /// <summary>
        /// Gets the current topic.
        /// </summary>
        /// <returns>The current topic.</returns>
        Topic GetTopic();

        /// <summary>
        /// Sets the prompt length with a new value.
        /// </summary>
        /// <param name="newPromptLength">The new prompt length to set.</param>
        void SetPromptLength(int newPromptLength);

        /// <summary>
        /// Sets the topic with a new value.
        /// </summary>
        /// <param name="newTopic">The new topic to set.</param>
        void SetTopic(Topic newTopic);
    }
}
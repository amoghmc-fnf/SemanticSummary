using Plugins.Models;

namespace Plugins
{
    /// <summary>
    /// The ISettingsPlugin interface defines methods for managing settings related to prompt length and topic.
    /// Each method provides functionality to get or set specific settings related to the plugin.
    /// </summary>
    public interface ISettingsPlugin
    {
        int GetPromptLength();
        string GetSummaryPrompt();
        Topic GetTopic();
        void SetPromptLength(int newPromptLength);
        void SetTopic(Topic newTopic);
    }
}
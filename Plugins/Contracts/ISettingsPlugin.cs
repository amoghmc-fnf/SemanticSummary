namespace Plugins
{
    public interface ISettingsPlugin
    {
        int GetPromptLength();
        string GetSummaryPrompt();
        Topic GetTopic();
        void SetPromptLength(int newPromptLength);
        void SetTopic(Topic newTopic);
    }
}
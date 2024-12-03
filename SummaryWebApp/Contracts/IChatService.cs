using Plugins.Models;

namespace SummaryWebApp.Contracts
{
    public interface IChatService
    {
        Task<int> GetPromptLengthAsync();
        Task<string> GetRegeneratedSummaryAsync(string input);
        Task<string> GetSummaryAsync(string input);
        Task<Topic> GetTopicAsync();
        Task UpdatePromptLengthAsync(int newPromptLength);
        Task UpdateTopicAsync(Topic newTopic);
    }
}
using Plugins.Models;

namespace SemanticKernelService.Contracts
{
    public interface IChatService
    {
        Task<string> GetPromptLength();
        Task<string> GetRegeneratedSummary(string userInput);
        Task<string> GetSummary(string userInput);
        Task<string> GetSummaryPrompt();
        Task<string> GetTopic();
        Task UpdatePromptLength(int newPromptLength);
        Task UpdateTopic(Topic newTopic);
    }
}
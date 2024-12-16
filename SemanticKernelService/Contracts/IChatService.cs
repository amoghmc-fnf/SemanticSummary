using Plugins.Models;

namespace SemanticKernelService.Contracts
{
    /// <summary>
    /// Defines methods for managing chat-related services.
    /// </summary>
    public interface IChatService
    {
        /// <summary>
        /// Gets the current prompt length.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains the current prompt length.</returns>
        Task<string> GetPromptLength();

        /// <summary>
        /// Regenerates a summary based on the provided user input.
        /// </summary>
        /// <param name="userInput">The user input to summarize again.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the regenerated summary.</returns>
        Task<string> GetRegeneratedSummary(string userInput);

        /// <summary>
        /// Gets a summary based on the provided user input.
        /// </summary>
        /// <param name="userInput">The user input to summarize.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the summary.</returns>
        Task<string> GetSummary(string userInput);

        /// <summary>
        /// Gets the prompt for generating a summary.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains the summary prompt.</returns>
        Task<string> GetSummaryPrompt();

        /// <summary>
        /// Gets the current topic.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains the current topic.</returns>
        Task<string> GetTopic();

        /// <summary>
        /// Updates the prompt length with a new value.
        /// </summary>
        /// <param name="newPromptLength">The new prompt length to set.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task UpdatePromptLength(int newPromptLength);

        /// <summary>
        /// Updates the current topic with a new value.
        /// </summary>
        /// <param name="newTopic">The new topic to set.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task UpdateTopic(Topic newTopic);
    }
}
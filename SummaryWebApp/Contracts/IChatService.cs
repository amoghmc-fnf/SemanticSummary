using Plugins.Models;

namespace SummaryWebApp.Contracts
{
    /// <summary>
    /// Defines the contract for chat services.
    /// </summary>
    public interface IChatService
    {
        /// <summary>
        /// Gets the length of the prompt asynchronously.
        /// </summary>
        /// <returns>The length of the prompt.</returns>
        Task<int> GetPromptLengthAsync();

        /// <summary>
        /// Regenerates the summary asynchronously based on the provided input.
        /// </summary>
        /// <param name="input">The input text to regenerate the summary from.</param>
        /// <returns>The regenerated summary.</returns>
        Task<string> GetRegeneratedSummaryAsync(string input);

        /// <summary>
        /// Gets the summary asynchronously based on the provided input.
        /// </summary>
        /// <param name="input">The input text to summarize.</param>
        /// <returns>The summary of the input text.</returns>
        Task<string> GetSummaryAsync(string input);

        /// <summary>
        /// Gets the current topic asynchronously.
        /// </summary>
        /// <returns>The current topic.</returns>
        Task<Topic> GetTopicAsync();

        /// <summary>
        /// Updates the length of the prompt asynchronously.
        /// </summary>
        /// <param name="newPromptLength">The new length of the prompt.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task UpdatePromptLengthAsync(int newPromptLength);

        /// <summary>
        /// Updates the current topic asynchronously.
        /// </summary>
        /// <param name="newTopic">The new topic to set.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task UpdateTopicAsync(Topic newTopic);
    }
}

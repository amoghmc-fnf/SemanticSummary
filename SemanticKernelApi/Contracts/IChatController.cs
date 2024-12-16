using Microsoft.AspNetCore.Mvc;
using Plugins.Models;

namespace SemanticKernelApi.Contracts
{
    /// <summary>
    /// The IChatController interface defines a set of asynchronous methods for managing chat-related operations.
    /// Each method returns a Task<IActionResult>, indicating asynchronous execution and an HTTP response.
    /// </summary>
    public interface IChatController
    {
        /// <summary>
        /// Gets the current prompt length.
        /// </summary>
        /// <returns>An <see cref="IActionResult"/> containing the current prompt length.</returns>
        Task<IActionResult> GetPromptLength();

        /// <summary>
        /// Regenerates a summary based on the provided user input.
        /// </summary>
        /// <param name="userInput">The user input to summarize again.</param>
        /// <returns>An <see cref="IActionResult"/> containing the regenerated summary.</returns>
        Task<IActionResult> GetRegeneratedSummary([FromBody] string userInput);

        /// <summary>
        /// Gets a summary based on the provided user input.
        /// </summary>
        /// <param name="userInput">The user input to summarize.</param>
        /// <returns>An <see cref="IActionResult"/> containing the summary.</returns>
        Task<IActionResult> GetSummary([FromBody] string userInput);

        /// <summary>
        /// Gets the current topic.
        /// </summary>
        /// <returns>An <see cref="IActionResult"/> containing the current topic.</returns>
        Task<IActionResult> GetTopic();

        /// <summary>
        /// Gets the prompt for generating a summary.
        /// </summary>
        /// <returns>An <see cref="IActionResult"/> containing the summary prompt.</returns>
        Task<IActionResult> GetSummaryPrompt();

        /// <summary>
        /// Updates the prompt length with a new value.
        /// </summary>
        /// <param name="newPromptLength">The new prompt length to set.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
        Task<IActionResult> UpdatePromptLength([FromBody] int newPromptLength);

        /// <summary>
        /// Updates the current topic with a new value.
        /// </summary>
        /// <param name="newTopic">The new topic to set.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
        Task<IActionResult> UpdateTopic([FromBody] Topic newTopic);
    }
}

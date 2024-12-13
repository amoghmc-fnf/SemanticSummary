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
        Task<IActionResult> GetPromptLength();
        Task<IActionResult> GetRegeneratedSummary([FromBody] string userInput);
        Task<IActionResult> GetSummary([FromBody] string userInput);
        Task<IActionResult> GetTopic();
        Task<IActionResult> GetSummaryPrompt();
        Task<IActionResult> UpdatePromptLength([FromBody] int newPromptLength);
        Task<IActionResult> UpdateTopic([FromBody] Topic newTopic);
    }
}
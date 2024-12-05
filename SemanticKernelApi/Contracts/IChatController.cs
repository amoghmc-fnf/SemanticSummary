using Microsoft.AspNetCore.Mvc;
using Plugins.Models;

namespace SemanticKernelApi.Contracts
{
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
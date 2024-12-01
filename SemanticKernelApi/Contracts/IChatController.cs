using Microsoft.AspNetCore.Mvc;
using Plugins;

namespace SemanticKernelApi.Contracts
{
    public interface IChatController
    {
        Task<IActionResult> GetPromptLength();
        Task<IActionResult> GetRegeneratedSummary([FromBody] string userInput);
        Task<IActionResult> GetSummary([FromBody] string userInput);
        Task<IActionResult> GetTopic();
        Task<IActionResult> UpdatePromptLength([FromBody] int newPromptLength);
        Task<IActionResult> UpdateTopic([FromBody] Topic newTopic);
    }
}
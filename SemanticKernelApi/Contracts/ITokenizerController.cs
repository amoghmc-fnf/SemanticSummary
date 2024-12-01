using Microsoft.AspNetCore.Mvc;

namespace SemanticKernelApi.Contracts
{
    public interface ITokenizerController
    {
        Task<IActionResult> GetTokenCount([FromBody] string userInput);
    }
}
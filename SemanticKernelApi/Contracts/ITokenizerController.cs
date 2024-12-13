using Microsoft.AspNetCore.Mvc;

namespace SemanticKernelApi.Contracts
{
    /// <summary>
    /// Defines methods for tokenizing user input.
    /// </summary>
    public interface ITokenizerController
    {
        Task<IActionResult> GetTokenCount([FromBody] string userInput);
    }
}
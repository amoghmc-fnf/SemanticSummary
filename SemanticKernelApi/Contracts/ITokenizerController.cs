using Microsoft.AspNetCore.Mvc;

namespace SemanticKernelApi.Contracts
{
    /// <summary>
    /// Defines methods for tokenizing user input.
    /// </summary>
    public interface ITokenizerController
    {
        /// <summary>
        /// Gets the token count for the provided user input.
        /// </summary>
        /// <param name="userInput">The user input to tokenize.</param>
        /// <returns>An IActionResult containing the token count.</returns>
        Task<IActionResult> GetTokenCount([FromBody] string userInput);
    }
}
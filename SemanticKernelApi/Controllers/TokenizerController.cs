using Microsoft.AspNetCore.Mvc;
using Microsoft.ML.Tokenizers;
using SemanticKernelApi.Contracts;
using SemanticKernelService.Contracts;

namespace SemanticKernelApi.Controllers
{
    /// <summary>
    /// Controller for handling tokenization-related operations.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TokenizerController : ControllerBase, ITokenizerController
    {
        private readonly ITokenizerService _tokenizerService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenizerController"/> class.
        /// </summary>
        /// <param name="service">The tokenizer service to use.</param>
        public TokenizerController(ITokenizerService service)
        {
            _tokenizerService = service;
        }

        /// <summary>
        /// Gets the token count for the provided user input.
        /// </summary>
        /// <param name="userInput">The user input to tokenize.</param>
        /// <returns>An <see cref="IActionResult"/> containing the token count.</returns>
        [HttpPost("count")]
        public async Task<IActionResult> GetTokenCount([FromBody] string userInput)
        {
            var result = await _tokenizerService.GetTokenCount(userInput);
            return Ok(result);
        }
    }
}
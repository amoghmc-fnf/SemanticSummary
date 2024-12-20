using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<TokenizerController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenizerController"/> class.
        /// </summary>
        /// <param name="service">The tokenizer service to use.</param>
        /// <param name="logger">The logger to use for logging.</param>
        public TokenizerController(ITokenizerService service, ILogger<TokenizerController> logger)
        {
            _tokenizerService = service;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger), "Logger cannot be null.");
            _logger.LogInformation("TokenizerController initialized.");
        }

        /// <summary>
        /// Gets the token count for the provided user input.
        /// </summary>
        /// <param name="userInput">The user input to tokenize.</param>
        /// <returns>An <see cref="IActionResult"/> containing the token count.</returns>
        [HttpPost("count")]
        public async Task<IActionResult> GetTokenCount([FromBody] string userInput)
        {
            if (string.IsNullOrEmpty(userInput))
            {
                _logger.LogError("User input cannot be null or empty.");
                return BadRequest("User input cannot be null or empty.");
            }

            try
            {
                _logger.LogInformation("Tokenizing user input.");
                var result = await _tokenizerService.GetTokenCount(userInput);
                _logger.LogInformation("Token count for input: {TokenCount}", result);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error tokenizing user input.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
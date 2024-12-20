using Microsoft.ML.Tokenizers;
using Microsoft.Extensions.Logging;
using SemanticKernelService.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemanticKernelService.Services
{
    /// <summary>
    /// Provides services for tokenizing user input.
    /// </summary>
    public class TokenizerService : ITokenizerService
    {
        private readonly Tokenizer _tokenizer;
        private readonly ILogger<TokenizerService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenizerService"/> class.
        /// </summary>
        /// <param name="tokenizer">The tokenizer to use.</param>
        /// <param name="logger">The logger to use for logging.</param>
        /// <exception cref="ArgumentNullException">Thrown when the tokenizer is null.</exception>
        public TokenizerService(Tokenizer tokenizer, ILogger<TokenizerService> logger)
        {
            _tokenizer = tokenizer ?? throw new ArgumentNullException(nameof(tokenizer), "Tokenizer cannot be null!");
            _logger = logger ?? throw new ArgumentNullException(nameof(logger), "Logger cannot be null!");
            _logger.LogInformation("TokenizerService initialized.");
        }

        /// <summary>
        /// Gets the token count for the provided user input.
        /// </summary>
        /// <param name="userInput">The user input to tokenize.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the token count as a string.</returns>
        public async Task<string> GetTokenCount(string userInput)
        {
            if (string.IsNullOrEmpty(userInput))
            {
                _logger.LogError("User input cannot be null or empty.");
                throw new ArgumentNullException(nameof(userInput), "User input cannot be null or empty.");
            }

            _logger.LogInformation("Tokenizing user input.");
            var result = _tokenizer.CountTokens(userInput);
            _logger.LogInformation("Token count for input: {TokenCount}", result);
            return await Task.FromResult(result.ToString());
        }
    }
}
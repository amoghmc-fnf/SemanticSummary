using Microsoft.ML.Tokenizers;
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
        private Tokenizer _tokenizer;

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenizerService"/> class.
        /// </summary>
        /// <param name="tokenizer">The tokenizer to use.</param>
        public TokenizerService(Tokenizer tokenizer)
        {
            _tokenizer = tokenizer;
        }

        /// <summary>
        /// Gets the token count for the provided user input.
        /// </summary>
        /// <param name="userInput">The user input to tokenize.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the token count as a string.</returns>
        public async Task<string> GetTokenCount(string userInput)
        {
            var result = _tokenizer.CountTokens(userInput);
            return result.ToString();
        }
    }
}
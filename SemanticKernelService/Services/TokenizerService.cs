using Microsoft.ML.Tokenizers;
using SemanticKernelService.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemanticKernelService.Services
{
    public class TokenizerService : ITokenizerService
    {
        private Tokenizer _tokenizer;

        public TokenizerService(Tokenizer tokenizer)
        {
            _tokenizer = tokenizer;
        }

        public async Task<string> GetTokenCount(string userInput)
        {
            var result = _tokenizer.CountTokens(userInput);
            return result.ToString();
        }
    }
}

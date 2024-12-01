using Microsoft.AspNetCore.Mvc;
using Microsoft.ML.Tokenizers;
using SemanticKernelApi.Contracts;

namespace SemanticKernelApi
{

    [Route("api/[controller]")]
    [ApiController]
    public class TokenizerController : ControllerBase, ITokenizerController
    {
        private Tokenizer _tokenizer;

        public TokenizerController(Tokenizer tokenizer)
        {
            _tokenizer = tokenizer;
        }

        [HttpPost("count")]
        public async Task<IActionResult> GetTokenCount([FromBody] string userInput)
        {
            var result = _tokenizer.CountTokens(userInput);
            return Ok(result.ToString());
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.ML.Tokenizers;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SemanticKernelApi.Controllers
{
    public interface ITokenizerController
    {
        Task<IActionResult> GetTokenCount([FromBody] string userInput);
    }

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

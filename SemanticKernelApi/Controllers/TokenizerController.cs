using Microsoft.AspNetCore.Mvc;
using Microsoft.ML.Tokenizers;
using SemanticKernelApi.Contracts;
using SemanticKernelService.Contracts;

namespace SemanticKernelApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class TokenizerController : ControllerBase, ITokenizerController
    {
        private readonly ITokenizerService _tokenizerService;

        public TokenizerController(ITokenizerService service)
        {
            _tokenizerService = service;
        }

        [HttpPost("count")]
        public async Task<IActionResult> GetTokenCount([FromBody] string userInput)
        {
            var result = await _tokenizerService.GetTokenCount(userInput);
            return Ok(result);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using System;
using System.Text.Json;
using SemanticKernelApi.Contracts;
using SemanticKernelService.Contracts;
using Plugins.Models;

namespace SemanticKernelApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase, IChatController
    {
        private readonly IChatService _chatService;

        public ChatController(IChatService service)
        {
            _chatService = service;
        }

        [HttpPost("summary")]
        public async Task<IActionResult> GetSummary([FromBody] string userInput)
        {
            var result = await _chatService.GetSummary(userInput);
            return Ok(result);
        }

        [HttpPost("regenerate_summary")]
        public async Task<IActionResult> GetRegeneratedSummary([FromBody] string userInput)
        {
            var result = await _chatService.GetRegeneratedSummary(userInput);
            return Ok(result);
        }

        [HttpGet("settings/topic")]
        public async Task<IActionResult> GetTopic()
        {
            var result = await _chatService.GetTopic();
            return Ok(result);
        }

        [HttpPost("settings/topic")]
        public async Task<IActionResult> UpdateTopic([FromBody] Topic newTopic)
        {
            await _chatService.UpdateTopic(newTopic);
            return Ok("Topic updated successfully!");
        }

        [HttpGet("settings/promptLength")]
        public async Task<IActionResult> GetPromptLength()
        {
            var result = await _chatService.GetPromptLength();
            return Ok(result);
        }

        [HttpPost("settings/promptLength")]
        public async Task<IActionResult> UpdatePromptLength([FromBody] int newPromptLength)
        {
            await _chatService.UpdatePromptLength(newPromptLength);
            return Ok("Prompt length updated successfully!");
        }
    }
}

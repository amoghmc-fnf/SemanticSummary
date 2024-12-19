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
    /// <summary>
    /// Controller for handling chat-related operations.
    /// </summary>
    // TODO: Add try catch for apis
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase, IChatController
    {
        private readonly IChatService _chatService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChatController"/> class.
        /// </summary>
        /// <param name="service">The chat service to use.</param>
        public ChatController(IChatService service)
        {
            _chatService = service;
        }

        /// <summary>
        /// Gets a summary based on the provided user input.
        /// </summary>
        /// <param name="userInput">The user input to summarize.</param>
        /// <returns>An <see cref="IActionResult"/> containing the summary.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the user input is null or empty.</exception>
        [HttpPost("summary")]
        public async Task<IActionResult> GetSummary([FromBody] string userInput)
        {
            if (string.IsNullOrEmpty(userInput))
            {
                throw new ArgumentNullException(nameof(userInput), "User input cannot be null or empty.");
            }

            try
            {
                var result = await _chatService.GetSummary(userInput);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Regenerates a summary based on the provided user input.
        /// </summary>
        /// <param name="userInput">The user input to summarize again.</param>
        /// <returns>An <see cref="IActionResult"/> containing the regenerated summary.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the user input is null or empty.</exception>
        [HttpPost("regenerate_summary")]
        public async Task<IActionResult> GetRegeneratedSummary([FromBody] string userInput)
        {
            if (string.IsNullOrEmpty(userInput))
            {
                throw new ArgumentNullException(nameof(userInput), "User input cannot be null or empty.");
            }

            try
            {
                var result = await _chatService.GetRegeneratedSummary(userInput);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets the current topic.
        /// </summary>
        /// <returns>An <see cref="IActionResult"/> containing the current topic.</returns>
        [HttpGet("settings/topic")]
        public async Task<IActionResult> GetTopic()
        {
            try
            {
                var result = await _chatService.GetTopic();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates the current topic with a new value.
        /// </summary>
        /// <param name="newTopic">The new topic to set.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the new topic is null.</exception>
        [HttpPost("settings/topic")]
        public async Task<IActionResult> UpdateTopic([FromBody] Topic newTopic)
        {
            try
            {
                await _chatService.UpdateTopic(newTopic);
                return Ok("Topic updated successfully!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets the current prompt length.
        /// </summary>
        /// <returns>An <see cref="IActionResult"/> containing the current prompt length.</returns>
        [HttpGet("settings/promptLength")]
        public async Task<IActionResult> GetPromptLength()
        {
            try
            {
                var result = await _chatService.GetPromptLength();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates the prompt length with a new value.
        /// </summary>
        /// <param name="newPromptLength">The new prompt length to set.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the new prompt length is less than or equal to zero.</exception>
        [HttpPost("settings/promptLength")]
        public async Task<IActionResult> UpdatePromptLength([FromBody] int newPromptLength)
        {
            if (newPromptLength <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(newPromptLength), "Prompt length must be greater than zero.");
            }

            try
            {
                await _chatService.UpdatePromptLength(newPromptLength);
                return Ok("Prompt length updated successfully!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets the prompt for generating a summary.
        /// </summary>
        /// <returns>An <see cref="IActionResult"/> containing the summary prompt.</returns>
        [HttpGet("settings/summaryPrompt")]
        public async Task<IActionResult> GetSummaryPrompt()
        {
            try
            {
                var result = await _chatService.GetSummaryPrompt();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Plugins;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SemanticKernelApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly Kernel _kernel;
        private readonly ChatHistory _history;
        private readonly IChatCompletionService _chatCompletionService;

        public ChatController(Kernel kernel)
        {
            _kernel = kernel;
            _history = new ChatHistory();
            _history.AddSystemMessage("You are a summarizer bot which will be given settings either by user through prompts or by code through kernel.InvokeAsync. Your task is to use the latest updated settings and generate the summary based on the summary prompt. Remember the code could have called kernel.InvokeAsync which always takes precedence over user prompts so DO NOT correct it back to the user prompt");
            _chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();
        }

        [HttpPost("chat")]
        public async Task<IActionResult> Chat([FromBody] string userInput)
        {
            _history.AddUserMessage(userInput);

            var openAIPromptExecutionSettings = new OpenAIPromptExecutionSettings
            {
                ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions
            };

            var result = await _chatCompletionService.GetChatMessageContentAsync(
                _history,
                executionSettings: openAIPromptExecutionSettings,
                kernel: _kernel);

            _history.AddMessage(result.Role, result.Content ?? string.Empty);
            var getTopic = _kernel.Plugins.GetFunction("Settings", "get_topic");
            var getLength = _kernel.Plugins.GetFunction("Settings", "get_length");
            var getSummary = _kernel.Plugins.GetFunction("Settings", "get_summary_prompt");
            var topic = await _kernel.InvokeAsync(getTopic);
            var length = await _kernel.InvokeAsync(getLength);
            var summary = await _kernel.InvokeAsync(getSummary);
            Console.WriteLine("Kernel Settings > " + topic + "\t" + length + "\t" + summary);
            return Ok(result.Content);
        }

        [HttpGet("settings/topic")]
        public async Task<IActionResult> GetTopic()
        {
            var getTopic = _kernel.Plugins.GetFunction("Settings", "get_topic");
            var topic = await _kernel.InvokeAsync(getTopic);

            return Ok(topic.ToString());
        }

        [HttpPost("settings/topic")]
        public async Task<IActionResult> UpdateTopic([FromBody] string newTopic)
        {
            var setTopic = _kernel.Plugins.GetFunction("Settings", "set_topic");
            var setTopicArgs = new KernelArguments();
            setTopicArgs.Add("newTopic", newTopic);

            await _kernel.InvokeAsync(setTopic, setTopicArgs);

            return Ok("Topic updated successfully");
        }

        [HttpGet("settings/promptLength")]
        public async Task<IActionResult> GetPromptLength()
        {
            var getLength = _kernel.Plugins.GetFunction("Settings", "get_length");
            var length = await _kernel.InvokeAsync(getLength);

            return Ok(length.ToString());
        }

        [HttpPost("settings/promptLength")]
        public async Task<IActionResult> UpdatePromptLength([FromBody] int newPromptLength)
        {
            var setLength = _kernel.Plugins.GetFunction("Settings", "set_length");
            var setLengthArgs = new KernelArguments();
            setLengthArgs.Add("newPromptLength", newPromptLength);

            await _kernel.InvokeAsync(setLength, setLengthArgs);

            return Ok("Prompt length updated successfully");
        }

    }
}

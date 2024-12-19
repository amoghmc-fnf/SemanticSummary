using Plugins.Models;
using SummaryWebApp.Contracts;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SummaryWebApp.Services
{
    /// <summary>
    /// Provides chat services by interacting with an HTTP API.
    /// </summary>
    // TODO: Add try catch only where necessary
    public class ChatService : IChatService
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChatService"/> class.
        /// </summary>
        /// <param name="httpClient">The HTTP client to use for requests.</param>
        public ChatService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Gets the summary of the input text asynchronously.
        /// </summary>
        /// <param name="input">The input text to summarize.</param>
        /// <returns>The summary of the input text.</returns>
        public async Task<string> GetSummaryAsync(string input)
        {
            var response = await _httpClient.PostAsJsonAsync("Chat/summary", input);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// Regenerates the summary of the input text asynchronously.
        /// </summary>
        /// <param name="input">The input text to regenerate the summary from.</param>
        /// <returns>The regenerated summary.</returns>
        public async Task<string> GetRegeneratedSummaryAsync(string input)
        {
            var response = await _httpClient.PostAsJsonAsync("Chat/regenerate_summary", input);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// Gets the current topic asynchronously.
        /// </summary>
        /// <returns>The current topic.</returns>
        public async Task<Topic> GetTopicAsync()
        {
            var response = await _httpClient.GetStringAsync("Chat/settings/topic");
            var result = (Topic)Enum.Parse(typeof(Topic), response);
            return result;
        }

        /// <summary>
        /// Updates the current topic asynchronously.
        /// </summary>
        /// <param name="newTopic">The new topic to set.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task UpdateTopicAsync(Topic newTopic)
        {
            var response = await _httpClient.PostAsJsonAsync("Chat/settings/topic", newTopic);
            response.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Gets the length of the prompt asynchronously.
        /// </summary>
        /// <returns>The length of the prompt.</returns>
        public async Task<int> GetPromptLengthAsync()
        {
            var response = await _httpClient.GetStringAsync("Chat/settings/promptLength");
            var result = int.Parse(response);
            return result;
        }

        /// <summary>
        /// Updates the length of the prompt asynchronously.
        /// </summary>
        /// <param name="newPromptLength">The new length of the prompt.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task UpdatePromptLengthAsync(int newPromptLength)
        {
            var response = await _httpClient.PostAsJsonAsync("Chat/settings/promptLength", newPromptLength);
            response.EnsureSuccessStatusCode();
        }
    }
}
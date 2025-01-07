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
    public class ChatService : IChatService
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChatService"/> class.
        /// </summary>
        /// <param name="httpClient">The HTTP client to use for requests.</param>
        /// <exception cref="ArgumentNullException">Thrown when the HTTP client is null.</exception>
        public ChatService(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        /// <summary>
        /// Gets the summary of the input text asynchronously.
        /// </summary>
        /// <param name="input">The input text to summarize.</param>
        /// <returns>The summary of the input text.</returns>
        /// <exception cref="HttpRequestException">Thrown when the HTTP request fails.</exception>
        public async Task<string> GetSummaryAsync(string input)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("Chat/summary", input);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException(ex.Message);
            }
        }

        /// <summary>
        /// Regenerates the summary of the input text asynchronously.
        /// </summary>
        /// <param name="input">The input text to regenerate the summary from.</param>
        /// <returns>The regenerated summary.</returns>
        /// <exception cref="HttpRequestException">Thrown when the HTTP request fails.</exception>
        public async Task<string> GetRegeneratedSummaryAsync(string input)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("Chat/regenerate_summary", input);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException(ex.Message);
            }
        }

        /// <summary>
        /// Gets the current topic asynchronously.
        /// </summary>
        /// <returns>The current topic.</returns>
        /// <exception cref="HttpRequestException">Thrown when the HTTP request fails.</exception>
        /// <exception cref="ArgumentException">Thrown when the response cannot be parsed to a Topic.</exception>
        public async Task<Topic> GetTopicAsync()
        {
            try
            {
                var response = await _httpClient.GetStringAsync("Chat/settings/topic");
                var result = (Topic)Enum.Parse(typeof(Topic), response);
                return result;
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException(ex.Message);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }

        /// <summary>
        /// Updates the current topic asynchronously.
        /// </summary>
        /// <param name="newTopic">The new topic to set.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="HttpRequestException">Thrown when the HTTP request fails.</exception>
        public async Task UpdateTopicAsync(Topic newTopic)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("Chat/settings/topic", newTopic);
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException(ex.Message);
            }
        }

        /// <summary>
        /// Gets the length of the prompt asynchronously.
        /// </summary>
        /// <returns>The length of the prompt.</returns>
        /// <exception cref="HttpRequestException">Thrown when the HTTP request fails.</exception>
        /// <exception cref="FormatException">Thrown when the response cannot be parsed to an integer.</exception>
        public async Task<int> GetPromptLengthAsync()
        {
            try
            {
                var response = await _httpClient.GetStringAsync("Chat/settings/promptLength");
                var result = int.Parse(response);
                return result;
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException(ex.Message);
            }
            catch (FormatException ex)
            {
                throw new FormatException(ex.Message);
            }
        }

        /// <summary>
        /// Updates the length of the prompt asynchronously.
        /// </summary>
        /// <param name="newPromptLength">The new length of the prompt.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="HttpRequestException">Thrown when the HTTP request fails.</exception>
        public async Task UpdatePromptLengthAsync(int newPromptLength)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("Chat/settings/promptLength", newPromptLength);
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException(ex.Message);
            }
        }
    }
}
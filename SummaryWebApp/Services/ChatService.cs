using Plugins;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SummaryWebApp.Services
{
    public interface IChatService
    {
        Task<int> GetPromptLengthAsync();
        Task<string> GetRegeneratedSummaryAsync(string input);
        Task<string> GetSummaryAsync(string input);
        Task<Topic> GetTopicAsync();
        Task UpdatePromptLengthAsync(int newPromptLength);
        Task UpdateTopicAsync(Topic newTopic);
    }

    public class ChatService : IChatService
    {
        private readonly HttpClient _httpClient;

        public ChatService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetSummaryAsync(string input)
        {
            var response = await _httpClient.PostAsJsonAsync("Chat/summary", input);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetRegeneratedSummaryAsync(string input)
        {
            var response = await _httpClient.PostAsJsonAsync("Chat/regenerate_summary", input);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<Topic> GetTopicAsync()
        {
            var response = await _httpClient.GetStringAsync("Chat/settings/topic");
            var result = (Topic)Enum.Parse(typeof(Topic), response);
            return result;
        }

        public async Task UpdateTopicAsync(Topic newTopic)
        {
            var response = await _httpClient.PostAsJsonAsync("Chat/settings/topic", newTopic);
            response.EnsureSuccessStatusCode();
        }

        public async Task<int> GetPromptLengthAsync()
        {
            var response = await _httpClient.GetStringAsync("Chat/settings/promptLength");
            var result = int.Parse(response);
            return result;
        }

        public async Task UpdatePromptLengthAsync(int newPromptLength)
        {
            var response = await _httpClient.PostAsJsonAsync("Chat/settings/promptLength", newPromptLength);
            response.EnsureSuccessStatusCode();
        }
    }
}

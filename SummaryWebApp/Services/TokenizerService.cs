using Plugins;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SummaryWebApp.Services
{
    public interface ITokenizerService
    {
        Task<int> GetTokenCountAsync(string userInput);
    }

    public class TokenizerService : ITokenizerService
    {
        private readonly HttpClient _httpClient;

        public TokenizerService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<int> GetTokenCountAsync(string userInput)
        {
            var response = await _httpClient.PostAsJsonAsync("Tokenizer/count", userInput);
            response.EnsureSuccessStatusCode();
            var parsedResponse = await response.Content.ReadAsStringAsync();
            var result = int.Parse(parsedResponse);
            return result;
        }
    }
}

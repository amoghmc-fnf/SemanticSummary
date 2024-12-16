using Plugins;
using SummaryWebApp.Contracts;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SummaryWebApp.Services
{
    /// <summary>
    /// Provides tokenizer services by interacting with an HTTP API.
    /// </summary>
    public class TokenizerService : ITokenizerService
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenizerService"/> class.
        /// </summary>
        /// <param name="httpClient">The HTTP client to use for requests.</param>
        public TokenizerService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Gets the token count of the user input asynchronously.
        /// </summary>
        /// <param name="userInput">The input text to tokenize.</param>
        /// <returns>The number of tokens in the input text.</returns>
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
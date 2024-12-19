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
        /// <exception cref="ArgumentNullException">Thrown when the HTTP client is null.</exception>
        public TokenizerService(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient), "HTTP client cannot be null!");
        }

        /// <summary>
        /// Gets the token count of the user input asynchronously.
        /// </summary>
        /// <param name="userInput">The input text to tokenize.</param>
        /// <returns>The number of tokens in the input text.</returns>
        /// <exception cref="HttpRequestException">Thrown when the HTTP request fails.</exception>
        /// <exception cref="FormatException">Thrown when the response cannot be parsed to an integer.</exception>
        public async Task<int> GetTokenCountAsync(string userInput)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("Tokenizer/count", userInput);
                response.EnsureSuccessStatusCode();
                var parsedResponse = await response.Content.ReadAsStringAsync();
                var result = int.Parse(parsedResponse);
                return result;
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException("An error occurred while getting the token count.", ex);
            }
            catch (FormatException ex)
            {
                throw new FormatException("An error occurred while parsing the token count.", ex);
            }
        }
    }
}
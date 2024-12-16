namespace SummaryWebApp.Contracts
{
    /// <summary>
    /// Defines the contract for tokenizer services.
    /// </summary>
    public interface ITokenizerService
    {
        /// <summary>
        /// Gets the token count asynchronously based on the provided user input.
        /// </summary>
        /// <param name="userInput">The input text to tokenize.</param>
        /// <returns>The number of tokens in the input text.</returns>
        Task<int> GetTokenCountAsync(string userInput);
    }
}
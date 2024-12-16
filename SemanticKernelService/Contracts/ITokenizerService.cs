namespace SemanticKernelService.Contracts
{
    /// <summary>
    /// Defines methods for tokenizing user input.
    /// </summary>
    public interface ITokenizerService
    {
        /// <summary>
        /// Gets the token count for the provided user input.
        /// </summary>
        /// <param name="userInput">The user input to tokenize.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the token count.</returns>
        Task<string> GetTokenCount(string userInput);
    }
}
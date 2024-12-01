namespace SummaryWebApp.Contracts
{
    public interface ITokenizerService
    {
        Task<int> GetTokenCountAsync(string userInput);
    }
}
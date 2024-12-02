namespace SemanticKernelService.Contracts
{
    public interface ITokenizerService
    {
        Task<string> GetTokenCount(string userInput);
    }
}
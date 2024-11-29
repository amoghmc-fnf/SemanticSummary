using Plugins;

namespace SummaryWebApp.Models
{
    public class Message
    {
        public string? Text { get; set; }
        public bool IsSent { get; set; }
        public List<string> Texts { get; set; } = [];
        public Topic Topic { get; set; }
        public int PromptLen { get; set; }
        public int CurrIndex { get; set; } = 0;
    }
}

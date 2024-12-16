using Plugins.Models;

namespace SummaryWebApp.Models
{
    /// <summary>
    /// Represents a message with user text, responses, and related metadata.
    /// </summary>
    public class Message
    {
        /// <summary>
        /// Gets or sets the user text of the message.
        /// </summary>
        public required string UserText { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the message is sent.
        /// </summary>
        public bool IsSent { get; set; }

        /// <summary>
        /// Gets or sets the list of responses to the message.
        /// </summary>
        public List<string> Responses { get; set; } = [];

        /// <summary>
        /// Gets or sets the topic of the message.
        /// </summary>
        public Topic Topic { get; set; }

        /// <summary>
        /// Gets or sets the length of the prompt.
        /// </summary>
        public int PromptLen { get; set; }

        /// <summary>
        /// Gets or sets the current index of the message.
        /// </summary>
        public int CurrIndex { get; set; } = 0;
    }
}
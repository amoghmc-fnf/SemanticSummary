using Microsoft.AspNetCore.Components;
using Plugins.Models;
using SummaryWebApp.Models;

namespace SummaryWebApp.Pages
{
    public partial class Chat
    {
        private int _outputLength;
        private string _userMessage;
        private List<Message> _messages;
        private Topic _selectedTopic;
        private int _inputTokenCount;
        private const int DefaultResponsesLimit = 3;

        public Chat()
        {
            _outputLength = 20;
            _selectedTopic = Topic.Generic;
            _userMessage = string.Empty;
            _messages = new();
            _inputTokenCount = 0;
        }

        /// <summary>
        /// This method gives the live count of the tokens used by the input message.
        /// </summary>
        /// <param name="changeEvent">The change in input message length.</param>
        /// <exception cref="ArgumentNullException">Thrown when the change event value is null.</exception>
        private async Task GetLiveCountAsync(ChangeEventArgs changeEvent)
        {
            if (changeEvent.Value is null)
            {
                throw new ArgumentNullException(nameof(changeEvent.Value), "ChangeEventArgs cannot be null.");
            }

            // Update the user message and get the token count
            _userMessage = changeEvent.Value.ToString() ?? string.Empty;
            _inputTokenCount = await TokenizerService.GetTokenCountAsync(_userMessage);
            StateHasChanged();
        }

        /// <summary>
        /// Sends the user's message and processes the response.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when an error occurs while sending the message.</exception>
        private async Task SendMessageAsync()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(_userMessage))
                {
                    // Update the summary settings and get the response
                    await UpdateSummarySettings(_selectedTopic, _outputLength);
                    var response = await ChatService.GetSummaryAsync(_userMessage);

                    // Add the user message and response to the messages list
                    _messages.Add(new Message { UserText = _userMessage, IsSent = true });
                    _messages.Add(new Message
                    {
                        UserText = _userMessage,
                        IsSent = false,
                        Responses = new List<string> { response },
                        Topic = _selectedTopic,
                        PromptLen = _outputLength
                    });

                    // Get the updated summary settings and reset the input message
                    await GetSummarySettings();
                    ResetInputMessage();
                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while sending the message.", ex);
            }
        }

        /// <summary>
        /// Gets the current summary settings.
        /// </summary>
        private async Task GetSummarySettings()
        {
            _selectedTopic = await ChatService.GetTopicAsync();
            _outputLength = await ChatService.GetPromptLengthAsync();
        }

        /// <summary>
        /// Updates the summary settings with the new topic and output length.
        /// </summary>
        /// <param name="newTopic">The new topic to set.</param>
        /// <param name="newOutputLength">The new output length to set.</param>
        private async Task UpdateSummarySettings(Topic newTopic, int newOutputLength)
        {
            await ChatService.UpdateTopicAsync(newTopic);
            await ChatService.UpdatePromptLengthAsync(newOutputLength);
        }

        /// <summary>
        /// Resets the input message and token count.
        /// </summary>
        private void ResetInputMessage()
        {
            _userMessage = string.Empty; // Clear the input after sending
            _inputTokenCount = 0;
        }

        /// <summary>
        /// Regenerates the last message's response if the number of responses is below the default limit.
        /// </summary>
        /// <param name="message">The message to regenerate the response for.</param>
        /// <exception cref="InvalidOperationException">Thrown when an error occurs while regenerating the message.</exception>
        private async Task RegenerateLastMessageAsync(Message message)
        {
            try
            {
                if (message.Responses.Count < DefaultResponsesLimit)
                {
                    // Update the summary settings and get the regenerated response
                    await UpdateSummarySettings(message.Topic, message.PromptLen);
                    var response = await ChatService.GetRegeneratedSummaryAsync(message.UserText);

                    // Add the regenerated response to the message
                    message.Responses.Add(response);
                    message.CurrIndex++;
                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while regenerating the message.", ex);
            }
        }

        /// <summary>
        /// Moves to the previous response page of the message.
        /// </summary>
        /// <param name="message">The message to navigate.</param>
        private static void PrevPage(Message message)
        {
            if (message.CurrIndex > 0)
                message.CurrIndex--;
        }

        /// <summary>
        /// Moves to the next response page of the message.
        /// </summary>
        /// <param name="message">The message to navigate.</param>
        private static void NextPage(Message message)
        {
            if (message.CurrIndex < message.Responses.Count - 1)
                message.CurrIndex++;
        }
    }
}
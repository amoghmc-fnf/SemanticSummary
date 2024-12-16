using Microsoft.AspNetCore.Components;
using Plugins.Models;
using SummaryWebApp.Models;

namespace SummaryWebApp.Pages
{
    public partial class Chat
    {
        private int outputLen;
        private string userMessage;
        private List<Message> messages;
        private Topic topicSelect;
        private int inputTokenCount;
        private const int defaultOutputLen = 20;
        private const int defaultResponsesLimit = 3;

        /// <summary>
        /// Initializes the chat component.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            userMessage = String.Empty;
            messages = [];
            outputLen = defaultOutputLen;
        }

        /// <summary>
        /// This method gives the live count of the tokens used by the input message.
        /// </summary>
        /// <param name="changeEvent">The change in input message length.</param>
        private async void GetLiveCount(ChangeEventArgs changeEvent)
        {
            try
            {
                userMessage = changeEvent.Value.ToString();
            }
            catch (Exception)
            {

                throw new NullReferenceException("Change in input text box cannot be null!");
            }
            inputTokenCount = await tokenizerService.GetTokenCountAsync(userMessage);
            StateHasChanged();
        }

        /// <summary>
        /// Sends the user's message and processes the response.
        /// </summary>
        private async void SendMessage()
        {
            if (!string.IsNullOrWhiteSpace(userMessage))
            {
                await chatService.UpdateTopicAsync(topicSelect);
                await chatService.UpdatePromptLengthAsync(outputLen);
                var response = await chatService.GetSummaryAsync(userMessage);
                messages.Add(new Message { UserText = userMessage, IsSent = true });
                messages.Add(new Message
                {
                    UserText = userMessage,
                    IsSent = false,
                    Responses = [response],
                    Topic = topicSelect,
                    PromptLen = outputLen
                });
                userMessage = string.Empty; // Clear the input after sending
                topicSelect = await chatService.GetTopicAsync();
                outputLen = await chatService.GetPromptLengthAsync();
                inputTokenCount = 0;
                StateHasChanged();
            }
        }

        /// <summary>
        /// Regenerates the last message's response if the number of responses is below the default limit.
        /// </summary>
        /// <param name="message">The message to regenerate the response for.</param>
        private async void RegenerateLastMessage(Message message)
        {
            if (message.Responses.Count < defaultResponsesLimit)
            {
                await chatService.UpdateTopicAsync(message.Topic);
                await chatService.UpdatePromptLengthAsync(message.PromptLen);
                var response = await chatService.GetRegeneratedSummaryAsync(message.UserText);

                message.Responses.Add(response);
                message.CurrIndex += 1;
                StateHasChanged();
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
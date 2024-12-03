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

        protected override async Task OnInitializedAsync()
        {
            userMessage = String.Empty;
            messages = new List<Message>();
            outputLen = 20;
        }

        /// <summary>
        /// This method gives the live count of the tokens used by the input message
        /// </summary>
        /// <param name="changeEvent">
        /// The change in input message length
        /// </param>
        private async void GetLiveCount(ChangeEventArgs changeEvent)
        {
            userMessage = changeEvent.Value.ToString();
            inputTokenCount = await tokenizerService.GetTokenCountAsync(userMessage);
            StateHasChanged();
        }

        private async void SendMessage()
        {
            if (!string.IsNullOrWhiteSpace(userMessage))
            {
                await chatService.UpdateTopicAsync(topicSelect);
                await chatService.UpdatePromptLengthAsync(outputLen);
                var response = await chatService.GetSummaryAsync(userMessage);
                messages.Add(new Message { Text = userMessage, IsSent = true });
                messages.Add(new Message
                {
                    Text = userMessage,
                    IsSent = false,
                    Texts = [response],
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

        private void HandleValidSubmit()
        {
            SendMessage();
        }

        private async void RegenerateLastMessage(Message message)
        {
            if (message.Texts.Count < 3)
            {
                await chatService.UpdateTopicAsync(message.Topic);
                await chatService.UpdatePromptLengthAsync(message.PromptLen);
                var response = await chatService.GetRegeneratedSummaryAsync(message.Text);

                message.Texts.Add(response);
                message.CurrIndex += 1;
                StateHasChanged();

            }
        }

        private void PrevPage(Message message)
        {
            if (message.CurrIndex > 0)
            {
                message.CurrIndex -= 1;
            }
        }

        private void NextPage(Message message)
        {
            if (message.CurrIndex < message.Texts.Count - 1)
            {
                message.CurrIndex += 1;
            }
        }
    }
}
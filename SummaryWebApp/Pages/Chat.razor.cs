using Microsoft.AspNetCore.Components;
using Plugins;
using SummaryWebApp.Models;

namespace SummaryWebApp.Pages
{
    public partial class Chat
    {
        private int outputLen;
        private string userMessage = string.Empty;
        private List<Message> messages = new List<Message>();
        private Topic topicSelect;
        private int inputTokenCount = 0;

        protected override async Task OnInitializedAsync()
        {
            outputLen = 20;
        }

        /// <summary>
        /// This method gives the live count of the tokens used by the input message
        /// </summary>
        /// <param name="changeEvent">
        /// This is passed when a change is detected in the input message div
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
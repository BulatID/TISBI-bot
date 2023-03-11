using OpenQA.Selenium.DevTools.V108.Debugger;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Telegram.Bot.Types;

namespace ParserTISBINew
{
    internal class ChatGPT
    {

        public string userData { get; set; }

        public ChatGPT(string UserData)
        {
            userData = UserData;
        }

        public async Task<string> sendText(string text)
        {
            userData = text;

            string apiKey = "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";
            string endpoint = "https://api.openai.com/v1/chat/completions";
            List<Message> messages = new List<Message>();
            var httpClient = new HttpClient();

            if (userData is not { Length: >0 }) return "Произошла ошибка";

            var message = new Message() { Role = "user", Content = text };

            messages.Add(message);

            var requestData = new Request()
            {
                ModelId = "gpt-3.5-turbo",
                Messages = messages
            };

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
            // отправляем запрос
            using var response = await httpClient.PostAsJsonAsync(endpoint, requestData);

            // получаем данные ответа
            ResponseData? responseData = await response.Content.ReadFromJsonAsync<ResponseData>();


            var choices = responseData?.Choices ?? new List<Choice>();

            var choice = choices[0];
            var responseMessage = choice.Message;
            // добавляем полученное сообщение в список сообщений
            messages.Add(responseMessage);
            var responseText = responseMessage.Content.Trim();

            return responseText;
        }
    }

    class Message
    {
        [JsonPropertyName("role")]
        public string Role { get; set; } = "";
        [JsonPropertyName("content")]
        public string Content { get; set; } = "";
    }
    class Request
    {
        [JsonPropertyName("model")]
        public string ModelId { get; set; } = "";
        [JsonPropertyName("messages")]
        public List<Message> Messages { get; set; } = new();
    }

    class ResponseData
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = "";
        [JsonPropertyName("object")]
        public string Object { get; set; } = "";
        [JsonPropertyName("created")]
        public ulong Created { get; set; }
        [JsonPropertyName("choices")]
        public List<Choice> Choices { get; set; } = new();
        [JsonPropertyName("usage")]
        public Usage Usage { get; set; } = new();
    }

    class Choice
    {
        [JsonPropertyName("index")]
        public int Index { get; set; }
        [JsonPropertyName("message")]
        public Message Message { get; set; } = new();
        [JsonPropertyName("finish_reason")]
        public string FinishReason { get; set; } = "";
    }

    class Usage
    {
        [JsonPropertyName("prompt_tokens")]
        public int PromptTokens { get; set; }
        [JsonPropertyName("completion_tokens")]
        public int CompletionTokens { get; set; }
        [JsonPropertyName("total_tokens")]
        public int TotalTokens { get; set; }
    }
}
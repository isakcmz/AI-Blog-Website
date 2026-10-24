using Microsoft.AspNetCore.SignalR;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BtkAkademiAIBlog.WebUI.Hubs
{
    public class ChatHub : Hub
    {
        private const string modelName = "openai/gpt-oss-120b";

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _apiKey;

        public ChatHub(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _apiKey = configuration["Groq:ApiKey"]
                      ?? throw new InvalidOperationException("Groq:ApiKey bulunamadı.");
        }

        private static readonly Dictionary<string, List<Dictionary<string, string>>> _history = new();

        public override Task OnConnectedAsync()
        {
            _history[Context.ConnectionId] = new List<Dictionary<string, string>>
            {
                new Dictionary<string, string>
                {
                    ["role"] = "system",
                    ["content"] = "Sen yardımcı bir asistansın. Cevapları kısa ve Türkçe ver."
                }
            };

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            _history.Remove(Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(string userMessage)
        {
            await Clients.Caller.SendAsync("ReceiveUserEcho", userMessage);

            var history = _history[Context.ConnectionId];
            history.Add(new() { ["role"] = "user", ["content"] = userMessage });

            try
            {
                await StreamGroq(history, Context.ConnectionAborted);
            }
            catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
            {
                history.RemoveAt(history.Count - 1); // başarısız mesajı geçmişten çıkar
                await Clients.Caller.SendAsync("ReceiveError", "Çok hızlı mesaj gönderdin, birkaç saniye bekleyip tekrar dene.");
            }
            catch (OperationCanceledException)
            {
                // kullanıcı bağlantıyı kapattı, bir şey yapmaya gerek yok
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message); // Visual Studio Output / konsolda görünür
                history.RemoveAt(history.Count - 1);
                await Clients.Caller.SendAsync("ReceiveError", "Şu an cevap veremiyorum, lütfen biraz sonra tekrar dene.");
            }
        }

        private async Task StreamGroq(List<Dictionary<string, string>> history, CancellationToken cancellationToken)
        {
            var client = _httpClientFactory.CreateClient("groq");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

            var payload = new
            {
                model = modelName,
                messages = history,
                stream = true,
                temperature = 0.2
            };

            using var req = new HttpRequestMessage(HttpMethod.Post, "v1/chat/completions");
            req.Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

            using var resp = await client.SendAsync(req, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

            if (!resp.IsSuccessStatusCode)
            {
                var errorBody = await resp.Content.ReadAsStringAsync(cancellationToken);
                throw new HttpRequestException(
                    $"Groq hatası {(int)resp.StatusCode}: {errorBody}", null, resp.StatusCode);
            }

            using var stream = await resp.Content.ReadAsStreamAsync(cancellationToken);
            using var reader = new StreamReader(stream);

            var sb = new StringBuilder();
            while (!reader.EndOfStream && !cancellationToken.IsCancellationRequested)
            {
                var line = await reader.ReadLineAsync();
                if (string.IsNullOrWhiteSpace(line)) continue;
                if (!line.StartsWith("data:")) continue;

                var data = line["data:".Length..].Trim();
                if (data == "[DONE]") break;

                try
                {
                    var chunk = JsonSerializer.Deserialize<ChatStreamChunk>(data);
                    var delta = chunk?.Choices?.FirstOrDefault()?.Delta?.Content;
                    if (!string.IsNullOrEmpty(delta))
                    {
                        sb.Append(delta); // AppendLine değil, Append
                        await Clients.Caller.SendAsync("ReceiveToken", delta, cancellationToken);
                    }
                }
                catch
                {
                    // hatalı chunk'ları atla
                }
            }

            var full = sb.ToString();
            history.Add(new() { ["role"] = "assistant", ["content"] = full });
            await Clients.Caller.SendAsync("CompleteMessage", full, cancellationToken);
        }

        private sealed class ChatStreamChunk
        {
            [JsonPropertyName("choices")] public List<Choice>? Choices { get; set; }
        }

        private sealed class Choice
        {
            [JsonPropertyName("delta")] public Delta? Delta { get; set; }
            [JsonPropertyName("finish_reason")] public string? FinishReason { get; set; }
        }

        private sealed class Delta
        {
            [JsonPropertyName("content")] public string? Content { get; set; }
            [JsonPropertyName("role")] public string? Role { get; set; }
        }
    }
}
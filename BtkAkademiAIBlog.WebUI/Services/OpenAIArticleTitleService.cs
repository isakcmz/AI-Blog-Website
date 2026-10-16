using System.Text;
using System.Text.Json;

namespace BtkAkademiAIBlog.WebUI.Services
{
    public class OpenAIArticleTitleService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public OpenAIArticleTitleService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }



        public async Task<string> GenerateArticleTitleAsync(string topic)
        {
            var client = _httpClientFactory.CreateClient();

            var apiKey = _configuration["Groq:ApiKey"];

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);

            var requestBody = new
            {
                model = "openai/gpt-oss-120b",
                messages = new[]
                {
                    new
                    {
                        role = "system",
                        content = "Sen teknoloji, yaşam, spor, finans, sanat gibi her alanda " +
                                  "derinlemesine, akıcı ve SEO uyumlu içerik üretebilen " +
                                  "profesyonel bir makale başlık oluşturucususun. " +
                                  "Akıcı, imla kurallarına uygun temiz bir Türkçe kullan."
                    },
                    new
                    {
                        role = "user",
                        content = $"'{topic}' konusu hakkında ve bu makale anahtar kelimelerini referans " +
                                  "alarak bu konuya uygun bir makale başlığı önermeni istiyorum. "
                    }
                },
                temperature = 0.6,
                max_completion_tokens = 2500
            };

            var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://api.groq.com/openai/v1/chat/completions", content);

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(responseString);

            return doc.RootElement
                      .GetProperty("choices")[0]
                      .GetProperty("message")
                      .GetProperty("content")
                      .GetString() ?? string.Empty;
        }
    }
}

using System.Text;
using System.Text.Json;

namespace BtkAkademiAIBlog.WebUI.Services
{
    public class OpenAIArticleService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public OpenAIArticleService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }



        public async Task<string> GenerateArticleAsync(string topic)
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
                                  "profesyonel bir baş editör ve kıdemli içerik yazarıyısın. " +
                                  "Sana verilen konunun türünü otomatik analiz et ve o türe " +
                                  "en uygun anlatım tarzını kendin seç. " +
                                  "Karakter sınırına ulaşmak için aynı cümleleri, fikirleri " +
                                  "ve kalıpları tekrarlama. Konuyu mantıklı alt başlıklara böl, " +
                                  "tanımlara, gerçek hayat örneklerine, avantaj ve dezavantajlara " +
                                  "ve konunun geleceğine değin. Akıcı, imla kurallarına uygun " +
                                  "temiz bir Türkçe kullan."
                    },
                    new
                    {
                        role = "user",
                        content = $"'{topic}' konusu hakkında; okuyucuyu meraklandıran " +
                                  "bir giriş paragrafı, konuyu farklı boyutlarıyla ele alan " +
                                  "detaylı alt başlıklar ve vurucu bir sonuç bölümünden oluşan, " +
                                  "laf kalabalığı içermeyen, minimum 1500 karakter uzunluğunda " +
                                  "kapsamlı ve özgün bir makale yaz."
                    }
                },
                temperature = 0.7,
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

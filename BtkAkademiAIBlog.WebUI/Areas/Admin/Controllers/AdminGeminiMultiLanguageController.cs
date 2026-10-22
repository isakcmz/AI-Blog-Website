using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BtkAkademiAIBlog.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminGeminiMultiLanguageController : Controller
    {
        private readonly IConfiguration _configuration;

        public AdminGeminiMultiLanguageController(IConfiguration configuration)
        {
            _configuration = configuration;
        }





        [HttpGet]
        public IActionResult CreateBlogWithGeminiMultiLanguage()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> CreateBlogWithGeminiMultiLanguage(
            string titleTr,
            string contentTr,
            string targetLanguage)
        {
            if (string.IsNullOrWhiteSpace(titleTr) || string.IsNullOrWhiteSpace(contentTr))
            {
                ViewBag.Error = "Başlık ve içerik boş olamaz.";
                return View();
            }

            var languageLabel = targetLanguage switch
            {
                "en" => "English",
                "de" => "Deutsch",
                "it" => "Italiano",
                "fr" => "Français",
                _ => "English"
            };

            var prompt = $@"
                You are a professional blog editor and localization expert.

                Rules:
                - Do NOT translate word by word
                - Rewrite for native {languageLabel} readers
                - Preserve meaning and technical accuracy
                - SEO friendly
                - Return ONLY valid JSON without any markdown code blocks or backticks

                JSON format (return exactly this structure with no extra text):
                {{
                  ""title"": ""..."",
                  ""content"": ""..."",
                  ""metaDescription"": ""...""
                }}

                Turkish Title:
                {titleTr}

                Turkish Content:
                {contentTr}
                ";

            var requestBody = new
            {
                contents = new[]
                {
                new
                {
                    parts = new[]
                    {
                        new { text = prompt }
                    }
                }
            }
            };


            var apiKey = _configuration["Gemini:ApiKey"];
            var model = _configuration["Gemini:Model"];

            if (string.IsNullOrWhiteSpace(apiKey) || string.IsNullOrWhiteSpace(model))
            {
                ViewBag.Error = "Gemini API Key veya Model yapılandırması bulunamadı.";
                return View();
            }

            var url = $"https://generativelanguage.googleapis.com/v1/models/{model}:generateContent?key={apiKey}";


            var httpContent = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

            using var client = new HttpClient();
            HttpResponseMessage response;

            try
            {
                response = await client.PostAsync(url, httpContent);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "HTTP çağrısı başarısız: " + ex.Message;
                return View();
            }

            var responseText = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Gemini API Hatası:\n" + responseText;
                return View();
            }

            // Gemini response parse
            string modelText;
            try
            {
                using var doc = JsonDocument.Parse(responseText);
                var parts = doc.RootElement
                    .GetProperty("candidates")[0]
                    .GetProperty("content")
                    .GetProperty("parts");

                modelText = string.Join("",
                    parts.EnumerateArray()
                         .Select(p => p.GetProperty("text").GetString()));
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Model çıktısı parse edilemedi: " + ex.Message;
                return View();
            }

            // 🔥 YENİ: Markdown code block'ları temizle
            modelText = CleanJsonFromMarkdown(modelText);

            // JSON → Model
            try
            {
                var localized = JsonSerializer.Deserialize<LocalizedPost>(modelText,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                ViewBag.TitleOut = localized?.Title;
                ViewBag.ContentOut = localized?.Content;
                ViewBag.MetaOut = localized?.MetaDescription;
                ViewBag.Raw = modelText;
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Model JSON formatında dönmedi:\n" + ex.Message;
                ViewBag.Raw = modelText;
            }

            return View();
        }

        // 🔥 YENİ: Markdown code block temizleme fonksiyonu
        private string CleanJsonFromMarkdown(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return text;

            // ```json veya ``` ile sarmalanmış içeriği temizle
            text = text.Trim();

            if (text.StartsWith("```json"))
            {
                text = text.Substring(7); // "```json" kaldır
            }
            else if (text.StartsWith("```"))
            {
                text = text.Substring(3); // "```" kaldır
            }

            if (text.EndsWith("```"))
            {
                text = text.Substring(0, text.Length - 3); // Son "```" kaldır
            }

            return text.Trim();
        }

        // 👇 Model sınıfı da controller dosyasının içinde
        public class LocalizedPost
        {
            [JsonPropertyName("title")]
            public string Title { get; set; }

            [JsonPropertyName("content")]
            public string Content { get; set; }

            [JsonPropertyName("metaDescription")]
            public string MetaDescription { get; set; }
        }


    }

}

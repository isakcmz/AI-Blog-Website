using BtkAkademiAIBlog.WebUI.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace BtkAkademiAIBlog.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminEmailCreateAIController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public AdminEmailCreateAIController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }






        [HttpGet]
        public IActionResult CreateMailWithOpenAI()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> CreateMailWithOpenAI(EmailModel model)
        {
            try
            {
                var prompt = $@"
                    The following message is an email sent by a user.

                    - Detect the language automatically.
                    - Do NOT translate the message.
                    - Write a professional and polite email reply.
                    - The reply must be written in the SAME language as the original message.

                    Email Subject: {model.MessageSubject}

                    Email Content: {model.MessageDetail}
                ";

                var client = _httpClientFactory.CreateClient();

                var apiKey = _configuration["OpenAI:ApiKey"];

                if (string.IsNullOrWhiteSpace(apiKey))
                {
                    ViewBag.AIError = "OpenAI API anahtarı yapılandırılmamış.";
                    return View();
                }

                client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue(
                        "Bearer", apiKey);

                var requestBody = new
                {
                    model = "gpt-4o-mini",
                    messages = new[]
                    {
                new { role = "user", content = prompt }
            }
                };

                var content = new StringContent(
                    JsonSerializer.Serialize(requestBody),
                    Encoding.UTF8,
                    "application/json");

                var response = await client.PostAsync(
                    "https://api.openai.com/v1/chat/completions",
                    content);

                var responseString = await response.Content.ReadAsStringAsync();

                // API başarısızsa hata mesajını göster.
                if (!response.IsSuccessStatusCode)
                {
                    string errorMessage = response.StatusCode switch
                    {
                        System.Net.HttpStatusCode.Unauthorized =>
                            "OpenAI API anahtarı geçersiz veya yetkisiz. API anahtarını kontrol et.",

                        System.Net.HttpStatusCode.TooManyRequests =>
                            "OpenAI API isteği reddetti. Kredin/bakiyen bitmiş veya kullanım limitine ulaşmış olabilirsin.",

                        System.Net.HttpStatusCode.Forbidden =>
                            "OpenAI API erişimine izin verilmedi. Hesap ve model erişimini kontrol et.",

                        _ =>
                            $"OpenAI API isteği başarısız oldu. HTTP kodu: {(int)response.StatusCode} ({response.StatusCode})."
                    };

                    // API'nin döndürdüğü asıl hata ayrıntısını da almayı dene.
                    try
                    {
                        using var errorDoc = JsonDocument.Parse(responseString);

                        if (errorDoc.RootElement.TryGetProperty("error", out var error)
                            && error.TryGetProperty("message", out var message))
                        {
                            errorMessage += $" Ayrıntı: {message.GetString()}";
                        }
                    }
                    catch (JsonException)
                    {
                        // Hata cevabı JSON değilse genel mesaj yeterli.
                    }

                    ViewBag.AIError = errorMessage;
                    return View();
                }

                using var jsonDoc = JsonDocument.Parse(responseString);

                var aiResponse = jsonDoc
                    .RootElement
                    .GetProperty("choices")[0]
                    .GetProperty("message")
                    .GetProperty("content")
                    .GetString();

                ViewBag.AIResponse = aiResponse;

                return View();
            }
            catch (HttpRequestException)
            {
                ViewBag.AIError =
                    "OpenAI API'sine bağlanılamadı. İnternet bağlantını ve API adresini kontrol et.";

                return View();
            }
            catch (TaskCanceledException)
            {
                ViewBag.AIError =
                    "OpenAI API isteği zaman aşımına uğradı. Lütfen tekrar dene.";

                return View();
            }
            catch (JsonException)
            {
                ViewBag.AIError =
                    "OpenAI API cevabı okunamadı. Lütfen tekrar dene.";

                return View();
            }
        }








        [HttpGet]
        public IActionResult CreateMailWithGroq()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateMailWithGroq(EmailModel model)
        {
            try
            {
                var prompt = $@"
                    The following message is an email sent by a user.

                    - Detect the language automatically.
                    - Do NOT translate the message.
                    - Write a professional and polite email reply.
                    - The reply must be written in the SAME language as the original message.

                    Email Subject: {model.MessageSubject}

                    Email Content: {model.MessageDetail}
                ";

                var client = _httpClientFactory.CreateClient();

                var apiKey = _configuration["Groq:ApiKey"];

                if (string.IsNullOrWhiteSpace(apiKey))
                {
                    ViewBag.AIError = "Groq API anahtarı yapılandırılmamış.";
                    return View();
                }

                client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue(
                        "Bearer", apiKey);

                var requestBody = new
                {
                    model = "openai/gpt-oss-120b",
                    messages = new[]
                    {
                        new { role = "user", content = prompt }
                    }
                };

                var content = new StringContent(
                    JsonSerializer.Serialize(requestBody),
                    Encoding.UTF8,
                    "application/json");

                var response = await client.PostAsync(
                    "https://api.groq.com/openai/v1/chat/completions",
                    content);

                var responseString = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    string errorMessage = response.StatusCode switch
                    {
                        System.Net.HttpStatusCode.Unauthorized =>
                            "Groq API anahtarı geçersiz veya yetkisiz.",

                        System.Net.HttpStatusCode.TooManyRequests =>
                            "Groq kullanım limitine ulaşılmış olabilir. Ücretsiz plan limitlerini kontrol et.",

                        System.Net.HttpStatusCode.Forbidden =>
                            "Groq API erişimine izin verilmedi. Hesap ve model erişimini kontrol et.",

                        _ =>
                            $"Groq API isteği başarısız oldu. HTTP kodu: {(int)response.StatusCode} ({response.StatusCode})."
                    };

                    try
                    {
                        using var errorDoc = JsonDocument.Parse(responseString);

                        if (errorDoc.RootElement.TryGetProperty("error", out var error))
                        {
                            if (error.ValueKind == JsonValueKind.Object &&
                                error.TryGetProperty("message", out var message))
                            {
                                errorMessage += $" Ayrıntı: {message.GetString()}";
                            }
                            else if (error.ValueKind == JsonValueKind.String)
                            {
                                errorMessage += $" Ayrıntı: {error.GetString()}";
                            }
                        }
                    }
                    catch (JsonException)
                    {
                        // JSON olmayan hata cevabı.
                    }

                    ViewBag.AIError = errorMessage;
                    return View();
                }

                using var jsonDoc = JsonDocument.Parse(responseString);

                var aiResponse = jsonDoc
                    .RootElement
                    .GetProperty("choices")[0]
                    .GetProperty("message")
                    .GetProperty("content")
                    .GetString();

                ViewBag.AIResponse = aiResponse;

                return View();
            }
            catch (HttpRequestException)
            {
                ViewBag.AIError =
                    "Groq API'sine bağlanılamadı. İnternet bağlantını kontrol et.";

                return View();
            }
            catch (TaskCanceledException)
            {
                ViewBag.AIError =
                    "Groq API isteği zaman aşımına uğradı. Lütfen tekrar dene.";

                return View();
            }
            catch (JsonException)
            {
                ViewBag.AIError =
                    "Groq API cevabı okunamadı. Lütfen tekrar dene.";

                return View();
            }
        }
    }
}

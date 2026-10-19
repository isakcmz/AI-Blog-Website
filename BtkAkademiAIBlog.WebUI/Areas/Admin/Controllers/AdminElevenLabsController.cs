using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace BtkAkademiAIBlog.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminElevenLabsController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public AdminElevenLabsController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }



        [HttpGet]
        public IActionResult TextToSpeech()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> TextToSpeech(string text)
        {
            try
            {
                var apiKey = _configuration["ElevenLabs:ApiKey"];
                var voiceId = "EXAVITQu4vr4xnSDxMaL"; // Rachel (elevenlabs resmi örnek sesi)

                var url = $"https://api.elevenlabs.io/v1/text-to-speech/{voiceId}/stream";

                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Add("xi-api-key", apiKey);

                var payload = new
                {
                    text = text,
                    model_id = "eleven_multilingual_v2"
                };

                var json = JsonSerializer.Serialize(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(url, content);

                var responseText = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    // HATA MESAJINI GÖRELİM (çok önemli)
                    ViewBag.ErrorDetail = responseText;
                    ViewBag.Error = "Ses oluşturulamadı.";
                    return View();
                }

                var audioBytes = await response.Content.ReadAsByteArrayAsync();

                var fileName = $"voice_{Guid.NewGuid()}.mp3";
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/voices", fileName);
                Directory.CreateDirectory("wwwroot/voices");

                await System.IO.File.WriteAllBytesAsync(path, audioBytes);

                ViewBag.AudioUrl = "/voices/" + fileName;
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
        }







        [HttpGet]
        public IActionResult TextToSpeech2()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> TextToSpeech2(string text)
        {
            try
            {
                var apiKey = _configuration["ElevenLabs:ApiKey"];
                var voiceId = "EXAVITQu4vr4xnSDxMaL"; // Rachel (elevenlabs resmi örnek sesi)

                var url = $"https://api.elevenlabs.io/v1/text-to-speech/{voiceId}/stream";

                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Add("xi-api-key", apiKey);

                var payload = new
                {
                    text = text,
                    model_id = "eleven_multilingual_v2"
                };

                var json = JsonSerializer.Serialize(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(url, content);

                var responseText = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    // HATA MESAJINI GÖRELİM (çok önemli)
                    ViewBag.ErrorDetail = responseText;
                    ViewBag.Error = "Ses oluşturulamadı.";
                    return View();
                }

                var audioBytes = await response.Content.ReadAsByteArrayAsync();

                var fileName = $"voice_{Guid.NewGuid()}.mp3";
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/voices", fileName);
                Directory.CreateDirectory("wwwroot/voices");

                await System.IO.File.WriteAllBytesAsync(path, audioBytes);

                ViewBag.AudioUrl = "/voices/" + fileName;
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
        }








        [HttpGet]
        public IActionResult TextToSpeech3()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> TextToSpeech3(string text)
        {

            if (string.IsNullOrWhiteSpace(text))
            {
                ViewBag.Answer = "Lütfen bir metin girin.";
                return View();
            }

            // 1) AI METİN CEVABI (şimdilik direkt text'i döndürüyoruz)
            // Gerçek senaryoda burada OpenAI / Gemini / Claude çağrın olacak.
            string aiTextResponse = $"AIAsistan yanıtı: {text}";

            ViewBag.Answer = aiTextResponse;


            // 2) ELEVENLABS AYARLARI
            string apiKey = _configuration["ElevenLabs:ApiKey"];
            string voiceId = "EXAVITQu4vr4xnSDxMaL"; // Rachel (güzel bir ses)
            string url = $"https://api.elevenlabs.io/v1/text-to-speech/{voiceId}/stream";

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Add("xi-api-key", apiKey);

            var payload = new
            {
                text = aiTextResponse, // konuşsun diye cevabı okuyor
                model_id = "eleven_multilingual_v2"
            };

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");


            // 3) ELEVENLABS'TEN SES OLUŞTURMA
            var response = await client.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Answer = "Ses oluşturulamadı.";
                ViewBag.AudioUrl = null;
                return View();
            }

            var audioBytes = await response.Content.ReadAsByteArrayAsync();


            // 4) MP3 DOSYASINI KAYDETME
            var fileName = $"voice_{Guid.NewGuid()}.mp3";
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/voices", fileName);

            Directory.CreateDirectory("wwwroot/voices");

            await System.IO.File.WriteAllBytesAsync(path, audioBytes);


            // 5) UI'YE VERİLERİ GÖNDER
            ViewBag.AudioUrl = "/voices/" + fileName;
            ViewBag.Answer = aiTextResponse;

            return View();

        }


    }
}

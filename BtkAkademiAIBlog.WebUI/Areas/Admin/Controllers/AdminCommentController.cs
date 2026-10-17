using BtkAkademiAIBlog.WebUI.Dtos.CommentDtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace BtkAkademiAIBlog.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminCommentController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AdminCommentController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }




        public async Task<IActionResult> CommentList()
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync("https://localhost:7017/api/Comments/CommentListWithArticleAndAuthor");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<ResultCommentDto>>(jsonData);
                return View(values);
            }

            return View();
        }







        [HttpGet]
        public IActionResult CreateComment()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> CreateComment(string CommentDetail)
        {
            ViewBag.OriginalText = CommentDetail;

            if (string.IsNullOrWhiteSpace(CommentDetail))
            {
                ViewBag.Success = false;
                ViewBag.Error = "Lütfen çevrilecek bir metin girin.";
                return View();
            }

            // MyMemory tek istekte en fazla 500 karakter kabul ediyor. bayt olarak ölçme
            if (Encoding.UTF8.GetByteCount(CommentDetail) > 500)
            {
                ViewBag.Success = false;
                ViewBag.Error = "Metin en fazla 500 karakter olabilir.";
                return View();
            }

            try
            {
                var client = _httpClientFactory.CreateClient();
                string url = "https://api.mymemory.translated.net/get" +
                             $"?q={Uri.EscapeDataString(CommentDetail)}" +
                             $"&langpair={Uri.EscapeDataString("tr|en")}";

                var response = await client.GetAsync(url);
                var json = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    ViewBag.Success = false;
                    ViewBag.Error = $"Api hatası: {response.StatusCode}";
                    ViewBag.Details = json;
                    return View();
                }

                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                // responseStatus bazen sayı, bazen metin olarak geliyor
                var statusElement = root.GetProperty("responseStatus");
                int apiStatus = statusElement.ValueKind == JsonValueKind.Number
                    ? statusElement.GetInt32()
                    : int.TryParse(statusElement.GetString(), out var s) ? s : 0;

                string translated = root.GetProperty("responseData")
                                        .GetProperty("translatedText")
                                        .GetString();

                if (apiStatus == 200 && !string.IsNullOrWhiteSpace(translated))
                {
                    ViewBag.Success = true;
                    ViewBag.TranslatedText = System.Net.WebUtility.HtmlDecode(translated).Trim();
                    ViewBag.Model = "MyMemory Translation API";
                    ViewBag.StatusCode = apiStatus;
                    return View();
                }

                // Limit dolduysa mesaj translatedText içinde geliyor
                ViewBag.Success = false;
                ViewBag.Error = apiStatus == 429
                    ? "Günlük ücretsiz çeviri limiti doldu, daha sonra tekrar deneyin."
                    : $"Çeviri alınamadı (kod: {apiStatus}).";
                ViewBag.Details = translated;
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Success = false;
                ViewBag.Error = "Bir hata oluştu!";
                ViewBag.Message = ex.Message;
                return View();
            }
        }



    }
}

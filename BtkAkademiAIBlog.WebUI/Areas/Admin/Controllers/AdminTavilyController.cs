using BtkAkademiAIBlog.WebUI.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace BtkAkademiAIBlog.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminTavilyController : Controller
    {
        private readonly string _apiKey;
        private readonly IHttpClientFactory _httpClientFactory;
        private const string ApiUrl = "https://api.tavily.com/search";

        public AdminTavilyController(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _apiKey = configuration["Tavily:ApiKey"]
                      ?? throw new InvalidOperationException("Tavily:ApiKey ayarı bulunamadı.");
            _httpClientFactory = httpClientFactory;
        }



        // GET: Search/Index
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.SearchCompleted = false;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                ViewBag.Error = "Lütfen bir arama sorgusu girin.";
                ViewBag.SearchCompleted = false;
                return View();
            }

            try
            {
                var httpClient = _httpClientFactory.CreateClient();

                var requestBody = new
                {
                    query = query,
                    search_depth = "advanced",
                    include_answer = true,
                    max_results = 5
                };

                var jsonContent = JsonSerializer.Serialize(requestBody);
                var request = new HttpRequestMessage(HttpMethod.Post, ApiUrl)
                {
                    Content = new StringContent(jsonContent, Encoding.UTF8, "application/json")
                };
                request.Headers.Add("Authorization", $"Bearer {_apiKey}");

                var response = await httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<TavilyApiResponse>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                ViewBag.Query = query;
                ViewBag.Answer = apiResponse?.Answer ?? "Cevap bulunamadı";
                ViewBag.Results = apiResponse?.Results ?? new List<TavilyResult>();
                ViewBag.SearchCompleted = true;
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Arama sırasında bir hata oluştu: {ex.Message}";
                ViewBag.SearchCompleted = false;
            }

            return View();
        }

    }
    

}

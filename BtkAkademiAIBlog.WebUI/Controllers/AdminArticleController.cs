using BtkAkademiAIBlog.WebUI.Dtos.ArticleDtos;
using BtkAkademiAIBlog.WebUI.Dtos.CategoryDtos;
using BtkAkademiAIBlog.WebUI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;

namespace BtkAkademiAIBlog.WebUI.Controllers
{
    public class AdminArticleController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly OpenAIArticleService _openAIArticleService;

        public AdminArticleController(IHttpClientFactory httpClientFactory, OpenAIArticleService openAIArticleService)
        {
            _httpClientFactory = httpClientFactory;
            _openAIArticleService = openAIArticleService;
        }






        public async Task<IActionResult> ArticleList()
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync("https://localhost:7017/api/Articles");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<ResultArticleDto>>(jsonData);
                return View(values);
            }

            return View();
        }






        [HttpGet]
        public async Task<IActionResult> CreateArticle()
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync("https://localhost:7017/api/Categories");
            var jsonData = await responseMessage.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<ResultCategoryDto>>(jsonData);
            List<SelectListItem> categoryValues = (from x in values
                                                   select new SelectListItem
                                                   {
                                                       Text = x.CategoryName,
                                                       Value = x.CategoryId.ToString()
                                                   }).ToList();
            ViewBag.CategoryValues = categoryValues;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateArticle(CreateArticleDto createArticleDto)
        {
            createArticleDto.CreatedDate = DateTime.Now;
            var client = _httpClientFactory.CreateClient();
            var jsonData = JsonConvert.SerializeObject(createArticleDto);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            await client.PostAsync("https://localhost:7017/api/Articles", stringContent);
            return RedirectToAction("ArticleList");
        }







        public async Task<IActionResult> DeleteArticle(int id)
        {
            var client = _httpClientFactory.CreateClient();
            await client.DeleteAsync($"https://localhost:7017/api/Articles?id={id}");
            return RedirectToAction("ArticleList");
        }








        [HttpGet]
        public async Task<IActionResult> UpdateArticle(int id)
        {
            var client1 = _httpClientFactory.CreateClient();
            var responseMessage1 = await client1.GetAsync("https://localhost:7017/api/Categories");
            var jsonData1 = await responseMessage1.Content.ReadAsStringAsync();
            var values1 = JsonConvert.DeserializeObject<List<ResultCategoryDto>>(jsonData1);
            List<SelectListItem> categoryValues = (from x in values1
                                                   select new SelectListItem
                                                   {
                                                       Text = x.CategoryName,
                                                       Value = x.CategoryId.ToString()
                                                   }).ToList();
            ViewBag.CategoryValues = categoryValues;

            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync($"https://localhost:7017/api/Articles/GetArticle?id={id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<GetArticleByIdDto>(jsonData);
                return View(values);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateArticle(UpdateArticleDto updateArticleDto)
        {
            var client = _httpClientFactory.CreateClient();
            var jsonData = JsonConvert.SerializeObject(updateArticleDto);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            await client.PutAsync("https://localhost:7017/api/Articles/", stringContent);
            return RedirectToAction("ArticleList");
        }









        [HttpGet]
        public IActionResult CreateArticleWithOpenAI()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> CreateArticleWithOpenAI(string topic)
        {
            if (string.IsNullOrEmpty(topic))
            {
                ViewBag.Error = "Lütfen konu giriniz!";
                return View();
            }

            ViewBag.Article = await _openAIArticleService.GenerateArticleAsync(topic);
            return View();
        }
    }
}

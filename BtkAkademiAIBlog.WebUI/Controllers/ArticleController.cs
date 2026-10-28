using BtkAkademiAIBlog.WebUI.Dtos.ArticleDtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BtkAkademiAIBlog.WebUI.Controllers
{
    public class ArticleController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ArticleController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }



        public async Task<IActionResult> ArticleList(int page=1)
        {
            int pageSize = 9;

            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync("https://localhost:7017/api/Articles");

            if (!responseMessage.IsSuccessStatusCode)
                return View(new List<ResultArticleDto>());

            var jsonData = await responseMessage.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<ResultArticleDto>>(jsonData);

            int totalCount = values.Count;
            int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var pagedArticles = values
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(pagedArticles);
        }




        public IActionResult ArticleDetail(int id)
        {
            ViewBag.i = id;
            return View();
        }
    }
}

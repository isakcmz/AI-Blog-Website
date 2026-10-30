using BtkAkademiAIBlog.WebUI.Dtos.ArticleDtos;
using BtkAkademiAIBlog.WebUI.Dtos.TradingVideoDtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BtkAkademiAIBlog.WebUI.ViewComponents.ArticleDetailComponents
{
    public class _ArticleDetailRelatedPostComponentPartial : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public _ArticleDetailRelatedPostComponentPartial(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }



        public async Task<IViewComponentResult> InvokeAsync(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync($"https://localhost:7017/api/Articles/GetArticlesRelatedByCategory?id={id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<Result3ArticlesByCategoryIdDto>>(jsonData);
                return View(values);
            }

            return View();
        }
    }
}

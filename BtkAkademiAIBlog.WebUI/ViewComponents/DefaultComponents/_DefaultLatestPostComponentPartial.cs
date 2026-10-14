using BtkAkademiAIBlog.WebUI.Dtos.ArticleDtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BtkAkademiAIBlog.WebUI.ViewComponents.DefaultComponents
{
    public class _DefaultLatestPostComponentPartial : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public _DefaultLatestPostComponentPartial(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync("https://localhost:7017/api/Articles/GetLastArticle");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<ResultLastArticleDto>(jsonData);
                ViewBag.Title = values.Title;
                ViewBag.LastArticleImageUrl = values.LastArticleImageUrl;
                ViewBag.CreatedDate = values.CreatedDate;
                ViewBag.NameSurname = values.Name + " " + values.Surname;
                ViewBag.CategoryName = values.CategoryName;
                return View();
            }

            return View();
        }
    }
}

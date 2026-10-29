using BtkAkademiAIBlog.WebUI.Dtos.ArticleDtos;
using BtkAkademiAIBlog.WebUI.Dtos.UserDtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BtkAkademiAIBlog.WebUI.ViewComponents.ArticleDetailComponents
{
    public class _ArticleDetailAuthorComponentPartial : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public _ArticleDetailAuthorComponentPartial(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }





        public async Task<IViewComponentResult> InvokeAsync(int id)
        {
            string userId = "";


            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync($"https://localhost:7017/api/Articles/GetArticle?id={id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<GetArticleByIdDto>(jsonData);
                userId = values.AppUserId;
            }


            var client2 = _httpClientFactory.CreateClient();
            var responseMessage2 = await client2.GetAsync($"https://localhost:7017/api/Users/GetUserById?id={userId}");
            if (responseMessage2.IsSuccessStatusCode)
            {
                var jsonData2 = await responseMessage2.Content.ReadAsStringAsync();
                var values2 = JsonConvert.DeserializeObject<GetUserByIdDto>(jsonData2);
                ViewBag.nameSurname = values2.Name + " " + values2.Surname;
                ViewBag.description = values2.Description;
                ViewBag.imageUrl = values2.ImageUrl;
                return View();
            }

            return View();
        }
    }
}

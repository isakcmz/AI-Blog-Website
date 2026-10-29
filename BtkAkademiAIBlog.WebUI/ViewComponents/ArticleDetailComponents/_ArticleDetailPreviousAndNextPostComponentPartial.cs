using BtkAkademiAIBlog.WebUI.Dtos.ArticleDtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BtkAkademiAIBlog.WebUI.ViewComponents.ArticleDetailComponents
{
    public class _ArticleDetailPreviousAndNextPostComponentPartial : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public _ArticleDetailPreviousAndNextPostComponentPartial(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }



        public async Task<IViewComponentResult> InvokeAsync(int id)
        {
            var client = _httpClientFactory.CreateClient();

            var responseMessage = await client.GetAsync($"https://localhost:7017/api/Articles/GetNextPost?id={id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                ViewBag.nextPost = await responseMessage.Content.ReadFromJsonAsync<ArticleNavigationDto>();
            }
            else
            {
                ViewBag.nextPost = null;
            }


            var responseMessage2 = await client.GetAsync($"https://localhost:7017/api/Articles/GetPreviousPost?id={id}");
            if (responseMessage2.IsSuccessStatusCode)
            {
                ViewBag.previousPost = await responseMessage2.Content.ReadFromJsonAsync<ArticleNavigationDto>();
            }
            else
            {
                ViewBag.previousPost = null;
            }


            return View();
        }
    }
}

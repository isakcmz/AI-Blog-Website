using BtkAkademiAIBlog.WebUI.Dtos.CategoryDtos;
using BtkAkademiAIBlog.WebUI.Dtos.MessageDtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BtkAkademiAIBlog.WebUI.ViewComponents.AdminNavbar
{
    public class _AdminNavbarUnreadMessageComponentPartial : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public _AdminNavbarUnreadMessageComponentPartial(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }



        public async Task<IViewComponentResult> InvokeAsync()
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync("https://localhost:7017/api/Messages/GetUnreadMessageList");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<ResultUnreadMessageDto>>(jsonData);
                return View(values);
            }

            return View();
        }
    }
}

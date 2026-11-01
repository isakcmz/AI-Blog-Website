using BtkAkademiAIBlog.WebUI.Dtos.CategoryDtos;
using BtkAkademiAIBlog.WebUI.Dtos.MessageDtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BtkAkademiAIBlog.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminMessageController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AdminMessageController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }



        public async Task<IActionResult> MessageList()
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync("https://localhost:7017/api/Messages");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<ResultMessageDto>>(jsonData);
                return View(values);
            }

            return View();
        }





        [HttpPost]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.PutAsync($"https://localhost:7017/api/Messages/MarkAsRead/{id}", null);
            return response.IsSuccessStatusCode ? Ok() : StatusCode(500);
        }


        [HttpPost]
        public async Task<IActionResult> MarkAllAsRead()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.PutAsync("https://localhost:7017/api/Messages/MarkAllAsRead", null);
            return response.IsSuccessStatusCode ? Ok() : StatusCode(500);
        }
    }
}

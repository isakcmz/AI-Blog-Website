using BtkAkademiAIBlog.WebUI.Dtos.ContactDtos;
using BtkAkademiAIBlog.WebUI.Dtos.MessageDtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace BtkAkademiAIBlog.WebUI.Controllers
{
    public class ContactController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ContactController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> SendMessage()
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync("https://localhost:7017/api/Contacts/GetFirstContact");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<ResultContactDto>(jsonData);
                ViewBag.PhoneNumber1 = values.PhoneNumber1;
                ViewBag.PhoneNumber2 = values.PhoneNumber2;
                ViewBag.Email = values.Email1;
                ViewBag.Email2 = values.Email2;
                ViewBag.Address = values.Address;
                ViewBag.MapLocation = values.MapLocation;

            }

            return View();
        }




        [HttpPost]
        public async Task<IActionResult> SendMessage(CreateMessageDto createMessageDto)
        {
            createMessageDto.SendDate = DateTime.Now;
            createMessageDto.IsRead = false;
            createMessageDto.AIStatus = "pending";

            var client = _httpClientFactory.CreateClient();
            var jsonData = JsonConvert.SerializeObject(createMessageDto);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            await client.PostAsync("https://localhost:7017/api/Messages", stringContent);
            return RedirectToAction("SendMessage");
        }
    }
}

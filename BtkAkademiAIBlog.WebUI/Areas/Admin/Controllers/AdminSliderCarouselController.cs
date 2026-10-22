using BtkAkademiAIBlog.WebUI.Dtos.SliderCarouselDtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace BtkAkademiAIBlog.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminSliderCarouselController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AdminSliderCarouselController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }





        public async Task<IActionResult> SliderCarouselList()
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync("https://localhost:7017/api/SliderCarousels");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<ResultSliderCarouselDto>>(jsonData);
                return View(values);
            }

            return View();
        }






        [HttpGet]
        public IActionResult CreateSliderCarousel()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateSliderCarousel(CreateSliderCarouselDto createSliderCarouselDto)
        {
            var client = _httpClientFactory.CreateClient();
            var jsonData = JsonConvert.SerializeObject(createSliderCarouselDto);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            await client.PostAsync("https://localhost:7017/api/SliderCarousels", stringContent);
            return RedirectToAction("SliderCarouselList");
        }







        public async Task<IActionResult> DeleteSliderCarousel(int id)
        {
            var client = _httpClientFactory.CreateClient();
            await client.DeleteAsync($"https://localhost:7017/api/SliderCarousels?id={id}");
            return RedirectToAction("SliderCarouselList");
        }








        [HttpGet]
        public async Task<IActionResult> UpdateSliderCarousel(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync($"https://localhost:7017/api/Contacts/SliderCarousel?id={id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<GetSliderCarouselByIdDto>(jsonData);
                return View(values);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateSliderCarousel(UpdateSliderCarouselDto updateSliderCarouselDto)
        {
            var client = _httpClientFactory.CreateClient();
            var jsonData = JsonConvert.SerializeObject(updateSliderCarouselDto);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            await client.PutAsync("https://localhost:7017/api/SliderCarousels/", stringContent);
            return RedirectToAction("SliderCarouselList");
        }
    }
}

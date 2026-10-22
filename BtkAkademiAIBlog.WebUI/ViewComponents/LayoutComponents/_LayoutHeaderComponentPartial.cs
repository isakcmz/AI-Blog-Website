using BtkAkademiAIBlog.WebUI.Dtos.SliderCarouselDtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BtkAkademiAIBlog.WebUI.ViewComponents.LayoutComponents
{
    public class _LayoutHeaderComponentPartial : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public _LayoutHeaderComponentPartial(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }



        public async Task<IViewComponentResult> InvokeAsync()
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
    }
}

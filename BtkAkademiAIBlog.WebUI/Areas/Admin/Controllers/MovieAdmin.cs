using BtkAkademiAIBlog.WebUI.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace BtkAkademiAIBlog.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MovieAdmin : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public MovieAdmin(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }




        public async Task<IActionResult> MovieList(int page=1)
        {
            var model = new MovieListPageViewModel();

            if (page < 1) page = 1;
            if (page > model.TotalPages) page = model.TotalPages;
            model.CurrentPage = page;

            try
            {
                var client = _httpClientFactory.CreateClient();
                var host = _configuration["RapidApi:ImdbHost"];

                var request = new HttpRequestMessage(HttpMethod.Get, $"https://{host}/list/{page}");
                request.Headers.Add("x-rapidapi-key", _configuration["RapidApi:Key"]);
                request.Headers.Add("x-rapidapi-host", host);

                using var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var body = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var apiResponse = JsonSerializer.Deserialize<MovieApiResponse>(body, options);
                model.Movies = apiResponse?.result ?? new();
            }
            catch (Exception ex)
            {
                model.ErrorMessage = "Filmler getirilemedi: " + ex.Message;
            }

            return View(model);
        }
    }
}












using AutoMapper;
using BtkAkademiAIBlog.WebApi.Context;
using BtkAkademiAIBlog.WebApi.Dtos.SliderCarouselDtos;
using BtkAkademiAIBlog.WebApi.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BtkAkademiAIBlog.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SliderCarouselsController : ControllerBase
    {
        private readonly BlogAIContext _context;
        private readonly IMapper _mapper;

        public SliderCarouselsController(BlogAIContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }






        [HttpGet]
        public IActionResult SliderCarouselList()
        {
            var values = _context.SliderCarousels.ToList();
            return Ok(values);
        }



        [HttpPost]
        public IActionResult CreateSliderCarousel(CreateSliderCarouselDto createSliderCarouselDto)
        {
            var value = _mapper.Map<SliderCarousel>(createSliderCarouselDto);
            _context.SliderCarousels.Add(value);
            _context.SaveChanges();
            return Ok("Ekleme İşlemi Başarılı");
        }



        [HttpDelete]
        public IActionResult DeleteSliderCarousel(int id)
        {
            var value = _context.SliderCarousels.Find(id);
            _context.SliderCarousels.Remove(value);
            _context.SaveChanges();
            return Ok("Silme İşlemi Başarılı");
        }




        [HttpPut]
        public IActionResult UpdateSliderCarousel(UpdateSliderCarouselDto updateSliderCarouselDto)
        {
            var value = _mapper.Map<SliderCarousel>(updateSliderCarouselDto);
            _context.SliderCarousels.Update(value);
            _context.SaveChanges();
            return Ok("Güncelleme İşlemi Başarılı");
        }



        [HttpGet("GetSliderCarousel")]
        public IActionResult GetSliderCarousel(int id)
        {
            var value = _context.SliderCarousels.Find(id);
            return Ok(value);
        }

    }
}

using BtkAkademiAIBlog.WebApi.Context;
using BtkAkademiAIBlog.WebApi.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BtkAkademiAIBlog.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TradingVideosController : ControllerBase
    {
        private readonly BlogAIContext _context;

        public TradingVideosController(BlogAIContext context)
        {
            _context = context;
        }



        [HttpGet]
        public IActionResult TradingVideoList()
        {
            var values = _context.TradingVideos.ToList();
            return Ok(values);
        }



        [HttpPost]
        public IActionResult CreateTradingVideo(TradingVideo tradingVideo)
        {
            _context.TradingVideos.Add(tradingVideo);
            _context.SaveChanges();
            return Ok("Ekleme İşlemi Başarılı");
        }



        [HttpDelete]
        public IActionResult DeleteTradingVideo(int id)
        {
            var value = _context.TradingVideos.Find(id);
            _context.TradingVideos.Remove(value);
            _context.SaveChanges();
            return Ok("Silme İşlemi Başarılı");
        }




        [HttpPut]
        public IActionResult UpdateTradingVideo(TradingVideo tradingVideo)
        {
            _context.TradingVideos.Update(tradingVideo);
            _context.SaveChanges();
            return Ok("Güncelleme İşlemi Başarılı");
        }



        [HttpGet("GetTradingVideo")]
        public IActionResult GetTradingVideo(int id)
        {
            var value = _context.TradingVideos.Find(id);
            return Ok(value);
        }
    }
}

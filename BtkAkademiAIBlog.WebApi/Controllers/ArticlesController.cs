using AutoMapper;
using BtkAkademiAIBlog.WebApi.Context;
using BtkAkademiAIBlog.WebApi.Dtos.ArticleDtos;
using BtkAkademiAIBlog.WebApi.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BtkAkademiAIBlog.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly BlogAIContext _context;
        private readonly IMapper _mapper;

        public ArticlesController(BlogAIContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }




        [HttpGet]
        public IActionResult ArticleList()
        {
            var values = _context.Articles.Include(x => x.Category).ToList();
            var dto = _mapper.Map<List<ResultArticleWithCategoryDto>>(values);
            return Ok(dto);
        }


        [HttpPost]
        public IActionResult CreateArticle(Article article)
        {
            article.CreatedDate = DateTime.Now;
            _context.Articles.Add(article);
            _context.SaveChanges();
            return Ok("Ekleme işlemi başarılı.");
        }



        [HttpDelete]
        public IActionResult DeleteArticle(int id)
        {
            var value = _context.Articles.Find(id);
            _context.Articles.Remove(value);
            _context.SaveChanges();
            return Ok("Silme işlemi başarılı.");
        }



        [HttpGet("GetArticle")]
        public IActionResult GetArticle(int id)
        {
            var value = _context.Articles.Find(id);
            return Ok(value);
        }



        [HttpPut]
        public IActionResult UpdateArticle(Article article)
        {
            _context.Articles.Update(article);
            _context.SaveChanges();
            return Ok("Güncelleme işlemi başarılı");
        }

    }
}

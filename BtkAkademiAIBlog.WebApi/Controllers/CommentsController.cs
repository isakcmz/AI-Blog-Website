using AutoMapper;
using BtkAkademiAIBlog.WebApi.Context;
using BtkAkademiAIBlog.WebApi.Dtos.CommentDtos;
using BtkAkademiAIBlog.WebApi.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BtkAkademiAIBlog.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly BlogAIContext _context;
        private readonly IMapper _mapper;

        public CommentsController(BlogAIContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }




        [HttpGet]
        public IActionResult CommentList()
        {
            var values = _context.Comments.ToList();
            return Ok(values);
        }



        [HttpPost]
        public IActionResult CreateComment(CreateCommentDto createCommentDto)
        {
            var value = _mapper.Map<Comment>(createCommentDto);
            _context.Comments.Add(value);
            _context.SaveChanges();
            return Ok("Yorum Ekleme İşlemi Başarılı");
        }



        [HttpDelete]
        public IActionResult DeleteComment(int id)
        {
            var value = _context.Comments.Find(id);
            _context.Comments.Remove(value);
            _context.SaveChanges();
            return Ok("Yorum Silme İşlemi Başarılı");
        }




        [HttpPut]
        public IActionResult UpdateComment(UpdateCommentDto updateCommentDto)
        {
            var value = _mapper.Map<Comment>(updateCommentDto);
            _context.Comments.Update(value);
            _context.SaveChanges();
            return Ok("Yorum Güncelleme İşlemi Başarılı");
        }



        [HttpGet("GetComment")]
        public IActionResult GetComment(int id)
        {
            var value = _context.Comments.Find(id);
            return Ok(value);
        }





        [HttpGet("CommentListWithArticleAndAuthor")]
        public IActionResult CommentListWithArticleAndAuthor()
        {
            var values = _context.Comments
                .Include(x => x.Article)
                .Include(y => y.AppUser)
                .ToList();
            
            var dto = _mapper.Map<List<ResultCommentWithArticleAndAuthorDto>>(values);
            return Ok(dto);
        }





        [HttpGet("GetCommentsWithUsersByArticleId")]
        public IActionResult GetCommentsWithUsersByArticleId(int id)
        {
            var values = _context.Comments
                .Where(x => x.ArticleId == id)
                .Select(y => new ResultCommentWithUserDto
                {
                    AppUserId = y.AppUserId,
                    CommentDate = y.CommentDate,
                    CommentDetail = y.CommentDetail,
                    CommentId = y.CommentId,
                    Name = y.AppUser.Name,
                    Surname = y.AppUser.Surname,
                    UserImageUrl = y.AppUser.ImageUrl,
                })
                .ToList();

            return Ok(values);
        }

    }
}

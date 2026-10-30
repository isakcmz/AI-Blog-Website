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
            var values = _context.Articles.Include(x => x.Category).Include(y => y.AppUser).ToList();
            var dto = _mapper.Map<List<ResultArticleWithCategoryDto>>(values);
            return Ok(dto);
        }


        [HttpPost]
        public IActionResult CreateArticle(CreateArticleDto createArticleDto)
        {
            createArticleDto.CreatedDate = DateTime.Now;
            var values = _mapper.Map<Article>(createArticleDto);
            _context.Articles.Add(values);
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
            return Ok(_mapper.Map<GetArticleByIdDto>(value));
        }



        [HttpPut]
        public IActionResult UpdateArticle(UpdateArticleDto updateArticleDto)
        {
            var value = _mapper.Map<Article>(updateArticleDto);
            _context.Articles.Update(value);
            _context.SaveChanges();
            return Ok("Güncelleme işlemi başarılı");
        }







        [HttpGet("GetArticlesFeatureSliderByTrue")]
        public IActionResult GetArticlesFeatureSliderByTrue()
        {
            var values = _context.Articles
                .Where(y => y.IsFeatureSlider == true)
                .Include(x => x.Category)
                .Include(y => y.AppUser)
                .ToList();
            return Ok(_mapper.Map<List<ResultArticleWithCategoryDto>>(values));
        }




        [HttpGet("ChangeIsFeatureSliderFromTrueToFalse")]
        public IActionResult ChangeIsFeatureSliderFromTrueToFalse(int id)
        {
            var value = _context.Articles.Find(id);
            value.IsFeatureSlider = false;
            _context.SaveChanges();
            return Ok("Değişiklikler kaydedildi");
        }


        [HttpGet("ChangeIsFeatureSliderFromFalseToTrue")]
        public IActionResult ChangeIsFeatureSliderFromFalseToTrue(int id)
        {
            var value = _context.Articles.Find(id);
            value.IsFeatureSlider = true;
            _context.SaveChanges();
            return Ok("Değişiklikler kaydedildi");
        }







        [HttpGet("GetLastTechnologyArticle")]
        public IActionResult GetLastTechnologyArticle()
        {
            var categoryId = _context.Categories
                .Where(x => x.CategoryName == "Teknoloji")
                .Select(y => y.CategoryId)
                .FirstOrDefault();

            var article = _context.Articles
                .Where(x => x.CategoryId == categoryId)
                .Include(x => x.AppUser)
                .OrderByDescending(y => y.ArticleId)
                .FirstOrDefault();

            return Ok(_mapper.Map<ResultLastTechnologyArticleDto>(article));
        }



        [HttpGet("GetLastSportArticle")]
        public IActionResult GetLastSportArticle()
        {
            var categoryId = _context.Categories
                .Where(x => x.CategoryName == "Spor")
                .Select(y => y.CategoryId)
                .FirstOrDefault();

            var article = _context.Articles
                .Where(x => x.CategoryId == categoryId)
                .Include(x => x.AppUser)
                .OrderByDescending(y => y.ArticleId)
                .FirstOrDefault();

            return Ok(_mapper.Map<ResultLastSportArticleDto>(article));
        }



        [HttpGet("GetLastTravelArticle")]
        public IActionResult GetLastTravelArticle()
        {
            var categoryId = _context.Categories
                .Where(x => x.CategoryName == "Seyehat")
                .Select(y => y.CategoryId)
                .FirstOrDefault();

            var article = _context.Articles
                .Where(x => x.CategoryId == categoryId)
                .Include(x => x.AppUser)
                .OrderByDescending(y => y.ArticleId)
                .FirstOrDefault();

            return Ok(_mapper.Map<ResultLastTravelArticleDto>(article));
        }




        [HttpGet("GetLastArticlesByDifferentCategories")]
        public IActionResult GetLastArticlesByDifferentCategories()
        {
            var list = _context.Articles
                .Include(x => x.Category)
                .OrderByDescending(y => y.CreatedDate)
                .Select(z => new LastArticleByCategoryDto
                {
                    ArticleId = z.ArticleId,
                    CategoryName = z.Category.CategoryName,
                    Title = z.Title,
                    CreatedDate = z.CreatedDate,
                    SliderCategoryImageUrl = z.SliderCategoryImageUrl
                }).ToList();

            var result = list
                .GroupBy(x => x.CategoryName)
                .Select(g => g.First())
                .OrderByDescending(y => y.CreatedDate)
                .Take(5)
                .ToList();

            return Ok(result);
        }




        [HttpGet("GetTrendingStoriesArticles")]
        public IActionResult GetTrendingStoriesArticles()
        {
            var values = _context.Articles
                .Where(x => x.IsTrendingStories == true)
                .Include(y => y.Category)
                .Include(z => z.AppUser)
                .Select(a => new ResultArticleTrendingStoriesDto
                {
                    ArticleId = a.ArticleId,
                    CategoryId = a.CategoryId,
                    Title = a.Title,
                    CreatedDate = a.CreatedDate,
                    CategoryName = a.Category.CategoryName,
                    Content = a.Content,
                    CoverImageUrl = a.CoverImageUrl,
                    FeatureImageUrl = a.FeatureImageUrl,
                    FeatureSliderImageUrl = a.FeatureSliderImageUrl,
                    IsFeatureSlider = a.IsFeatureSlider,
                    IsTrendingStories = a.IsTrendingStories,
                    MainImageUrl = a.MainImageUrl,
                    Name = a.AppUser.Name,
                    Surname = a.AppUser.Surname,
                    SliderCategoryImageUrl = a.SliderCategoryImageUrl,
                    FeaturedCoverImageUrl = a.FeaturedCoverImageUrl,
                    FeaturedCoverImageUrlStatus = a.FeaturedCoverImageUrlStatus

                }).ToList();

            return Ok(values);
        }





        [HttpGet("GetLastArticle")]
        public IActionResult GetLastArticle()
        {
            var values = _context.Articles
                .Where(x => x.IsLastArticle == true)
                .Include(y => y.Category)
                .Include(z => z.AppUser)
                .Select(a => new ResultLastArticleDto
                {
                    ArticleId = a.ArticleId,
                    CategoryId = a.CategoryId,
                    Title = a.Title,
                    CreatedDate = a.CreatedDate,
                    CategoryName = a.Category.CategoryName,
                    Content = a.Content,
                    CoverImageUrl = a.CoverImageUrl,
                    FeatureImageUrl = a.FeatureImageUrl,
                    FeatureSliderImageUrl = a.FeatureSliderImageUrl,
                    IsFeatureSlider = a.IsFeatureSlider,
                    IsTrendingStories = a.IsTrendingStories,
                    MainImageUrl = a.MainImageUrl,
                    Name = a.AppUser.Name,
                    Surname = a.AppUser.Surname,
                    SliderCategoryImageUrl = a.SliderCategoryImageUrl,
                    IsLastArticle = a.IsLastArticle,
                    LastArticleImageUrl = a.LastArticleImageUrl,

                }).FirstOrDefault();

            return Ok(values);
        }




        [HttpGet("GetLast4ArticlesWithCategory")]
        public IActionResult GetLast4ArticlesWithCategory()
        {
            var values = _context.Articles
                .Include(x => x.Category)
                .OrderByDescending(y => y.ArticleId)
                .Take(4)
                .Select(z => new ResultLast4ArticleWithCategoryDto
                {
                    ArticleId = z.ArticleId,
                    Title = z.Title,
                    CategoryName = z.Category.CategoryName,
                    Image300x300Url = z.Image300x300Url
                }).ToList();

            return Ok(values);
        }






        [HttpGet("GetArticlesSubFeatureStatusByTrue")]
        public IActionResult GetArticlesSubFeatureStatusByTrue()
        {
            var values = _context.Articles
                .Where(x => x.SubFeatureStatus == true)
                .Include(y => y.Category)
                .Include(z => z.AppUser)
                .Select(a => new ResultArticlesSubFeatureStatusByTrueDto
                {
                    ArticleId = a.ArticleId,
                    CategoryId = a.CategoryId,
                    Title = a.Title,
                    CreatedDate = a.CreatedDate,
                    CategoryName = a.Category.CategoryName,
                    Content = a.Content,
                    CoverImageUrl = a.CoverImageUrl,
                    FeatureImageUrl = a.FeatureImageUrl,
                    FeatureSliderImageUrl = a.FeatureSliderImageUrl,
                    IsFeatureSlider = a.IsFeatureSlider,
                    IsTrendingStories = a.IsTrendingStories,
                    MainImageUrl = a.MainImageUrl,
                    Name = a.AppUser.Name,
                    Surname = a.AppUser.Surname,
                    UserImageUrl = a.AppUser.ImageUrl,
                    SliderCategoryImageUrl = a.SliderCategoryImageUrl,
                    IsLastArticle = a.IsLastArticle,
                    LastArticleImageUrl = a.LastArticleImageUrl,
                    SubFeatureImageUrl = a.SubFeatureImageUrl,
                    SubFeatureStatus = a.SubFeatureStatus

                }).ToList();

            return Ok(values);
        }







        [HttpGet("GetNextPost")]
        public IActionResult GetNextPost(int id)
        {
            var values = _context.Articles
                .Where(x => x.ArticleId > id)
                .OrderBy(x => x.ArticleId)
                .Select(x => new
                {
                    x.Title,
                    x.ArticleId,
                })
                .FirstOrDefault();

            if (values == null)
            {
                return NotFound("Sonraki kayıt bulunamadı!");
            }

            return Ok(values);
        }



        [HttpGet("GetPreviousPost")]
        public IActionResult GetPreviousPost(int id)
        {
            var values = _context.Articles
                .Where(x => x.ArticleId < id)
                .OrderByDescending(x => x.ArticleId)
                .Select(x => new
                {
                    x.Title,
                    x.ArticleId,
                })
                .FirstOrDefault();

            if (values == null)
            {
                return NotFound("Önceki kayıt bulunamadı!");
            }

            return Ok(values);
        }






        [HttpGet("GetArticlesRelatedByCategory")]
        public IActionResult GetArticlesRelatedByCategory(int id)
        {
            var categoryId = _context.Articles
                .Where(x => x.ArticleId == id)
                .Select(y => y.CategoryId)
                .FirstOrDefault();

            var values = _context.Articles
                .Include(x => x.AppUser)
                .Where(y => y.CategoryId == int.Parse(categoryId.ToString()))
                .Skip(1)
                .Take(3)
                .Select(z => new Result3ArticlesByCategoryIdDto
                {
                    ArticleId = z.ArticleId,
                    Title = z.Title,
                    CoverImageUrl = z.CoverImageUrl,
                    AuthorId = z.AppUser.Id,
                    Name = z.AppUser.Name,
                    Surname = z.AppUser.Surname,
                    UserImageUrl = z.AppUser.ImageUrl,
                    CreatedDate = z.CreatedDate
                }).ToList();

            return Ok(values);
        }




    }
}

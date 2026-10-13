using AutoMapper;
using BtkAkademiAIBlog.WebApi.Dtos.ArticleDtos;
using BtkAkademiAIBlog.WebApi.Entities;

namespace BtkAkademiAIBlog.WebApi.Mapping
{
    public class GeneralMapping : Profile
    {
        public GeneralMapping()
        {
            CreateMap<Article, ResultArticleWithCategoryDto>().ReverseMap();
        }
    }
}

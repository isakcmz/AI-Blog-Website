using AutoMapper;
using BtkAkademiAIBlog.WebApi.Dtos.ArticleDtos;
using BtkAkademiAIBlog.WebApi.Dtos.CategoryDtos;
using BtkAkademiAIBlog.WebApi.Entities;

namespace BtkAkademiAIBlog.WebApi.Mapping
{
    public class GeneralMapping : Profile
    {
        public GeneralMapping()
        {
            CreateMap<Article, ResultArticleWithCategoryDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.AppUser.Name))
                .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.AppUser.Surname))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.AppUser.ImageUrl));

            CreateMap<Article, ResultLastTechnologyArticleDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.AppUser.Name))
                .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.AppUser.Surname))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.AppUser.ImageUrl));

            CreateMap<Article, ResultLastSportArticleDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.AppUser.Name))
                .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.AppUser.Surname))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.AppUser.ImageUrl));

            CreateMap<Article, ResultLastTravelArticleDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.AppUser.Name))
                .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.AppUser.Surname))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.AppUser.ImageUrl));


            CreateMap<Article, CreateArticleDto>().ReverseMap();
            CreateMap<Article, UpdateArticleDto>().ReverseMap();
            CreateMap<Article, GetArticleByIdDto>().ReverseMap();

            CreateMap<Category, CreateCategoryDto>().ReverseMap();
        }
    }
}

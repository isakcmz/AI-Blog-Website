using AutoMapper;
using BtkAkademiAIBlog.WebApi.Dtos.ArticleDtos;
using BtkAkademiAIBlog.WebApi.Dtos.CategoryDtos;
using BtkAkademiAIBlog.WebApi.Dtos.CommentDtos;
using BtkAkademiAIBlog.WebApi.Dtos.ContactDtos;
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
            CreateMap<Category, UpdateCategoryDto>().ReverseMap();

            CreateMap<Comment, ResultCommentDto>().ReverseMap();
            CreateMap<Comment, CreateCommentDto>().ReverseMap();
            CreateMap<Comment, UpdateCommentDto>().ReverseMap();
            CreateMap<Comment, GetCommentByIdDto>().ReverseMap();


            CreateMap<Comment, ResultCommentWithArticleAndAuthorDto>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Article.Title))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.AppUser.Name))
                .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.AppUser.Surname));


            CreateMap<Contact, ResultContactDto>().ReverseMap();
            CreateMap<Contact, CreateContactDto>().ReverseMap();
            CreateMap<Contact, UpdateContactDto>().ReverseMap();
            CreateMap<Contact, GetContactByIdDto>().ReverseMap();

        }
    }
}

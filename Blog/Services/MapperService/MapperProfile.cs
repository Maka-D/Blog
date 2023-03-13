using AutoMapper;
using Blog.Models;
using Blog.ViewModels;

namespace Blog.Services.MapperService
{
    public class MapperProfile :Profile
    {
        public MapperProfile()
        {
            CreateMap<PostViewModel, BlogPost>()
                .ReverseMap();
            CreateMap<BlogUser, RegisterViewModel>()
                .ReverseMap();
            CreateMap<BlogUser, LogInResponseViewModel>()
                .ForMember(src => src.UserId, c => c.MapFrom(dest => dest.Id))
                .ReverseMap();          
        }
    }
}

using AutoMapper;
using Blog.Services.MapperService;

namespace Blog.Extensions
{
    public static class AddMapperServiceExtension
    {
        public static void AddMapperService(this IServiceCollection service)
        {
            var config = new MapperConfiguration(conf =>
            {
                conf.AddProfile(new MapperProfile());
            });
            var mapper = config.CreateMapper();
            service.AddSingleton(mapper);
        }
    }
}

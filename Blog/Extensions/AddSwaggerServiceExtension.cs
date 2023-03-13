using Microsoft.OpenApi.Models;

namespace Blog.Extensions
{
    public static class AddSwaggerServiceExtension
    {
        public static void AddSwaggerService(this IServiceCollection service)
        {
            service.AddSwaggerGen(opt =>
            {
                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                opt.AddSecurityRequirement(new OpenApiSecurityRequirement {
                   {
                      new OpenApiSecurityScheme
                      {
                         Reference = new OpenApiReference
                         {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                         }
                      },
                      new string[] { }
                   }
                });
            });
        }
    }
}

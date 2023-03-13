using Blog.Data.Repository;
using Blog.Services.PostService;
using Blog.Extensions;
using Blog.Data.Repositories.AuthRepository;
using Blog.Services.AuthService;
using Blog.GlobalErrorHandler;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerService();

builder.Services.AddIdentityServices(builder.Configuration);

builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddTransient<IPostService, PostService>();
builder.Services.AddTransient<IAuthService, AuthService>();

builder.Services.AddMapperService();

builder.Services.AddJwtTokenService(builder.Configuration);

var MyAllowedOrigins = "_myAllowedOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(MyAllowedOrigins,
        builder =>
        {
            builder.WithOrigins("https://localhost:7032/")
            .AllowAnyHeader()
            .AllowAnyMethod();
        });
});

var app = builder.Build();

app.UseCors();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();

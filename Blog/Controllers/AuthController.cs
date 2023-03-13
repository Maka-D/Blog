using Blog.Services.AuthService;
using Blog.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController :Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("Register")]
        public async Task<bool> RegisterUser(RegisterViewModel userInput)
        {
            return await _authService.RegisterUserAsync(userInput);
        }

        [HttpPost("LogIn")]
        public async Task<LogInResponseViewModel> LogInUser(LogInRequestViewModel userInput)
        {
            return await _authService.LogInUserAsync(userInput);
        }
        [HttpPost("RefreshTokens")]
        public async Task<TokensViewModel> RefreshTokens(TokensViewModel tokens)
        {
            return await _authService.RefreshTokensAsync(tokens);
        }
        [HttpPost("RemoveTokens")]
        public async Task RemoveTokens(string userId)
        {
           await _authService.RemoveTokensForUserAsync(userId);
        }
    }
}

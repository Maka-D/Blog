using Blog.ViewModels;

namespace Blog.Services.AuthService
{
    public interface IAuthService
    {
        Task<LogInResponseViewModel> LogInUserAsync(LogInRequestViewModel userInput);
        Task<bool> RegisterUserAsync(RegisterViewModel userInput);
        Task<TokensViewModel> RefreshTokensAsync(TokensViewModel tokens);
        Task RemoveTokensForUserAsync(string userId);
    }
}
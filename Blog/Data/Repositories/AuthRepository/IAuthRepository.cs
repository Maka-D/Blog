using Blog.Models;
using Blog.ViewModels;
using System.Security.Claims;

namespace Blog.Data.Repositories.AuthRepository
{
    public interface IAuthRepository
    {
        Task<BlogUser> LogInUserAsync(LogInRequestViewModel userInfo);
        Task<BlogUser> RegisterUserAsync(BlogUser userInfo, string password);
        Task<BlogUser> ValidateUser(TokensViewModel tokens, ClaimsPrincipal principal);
        Task SetAuthenticationTokenForUserAsync(BlogUser user, string provider, TokenNameType tokenName, 
            string token, DateTime? expirationTime = null);
        Task RemoveAuthenticationTokensForUserAsync(string userId);
    }
}
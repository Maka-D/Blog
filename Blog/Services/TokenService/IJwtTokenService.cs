using Blog.Models;
using Blog.ViewModels;
using System.Security.Claims;

namespace Blog.Services.TokenService
{
    public interface IJwtTokenService
    {
        Task<TokenInfo> GenerateJwtAccessToken(BlogUser user);
        Task<TokenInfo> GenerateRefreshToken(DateTime? expirationTime = null);
        Task<ClaimsPrincipal> GetPrincipalFromToken(string token);
    }
}
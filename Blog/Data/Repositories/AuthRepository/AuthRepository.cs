using Blog.Data.CustomExceptions;
using Blog.Models;
using Blog.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace Blog.Data.Repositories.AuthRepository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly AppDbContext _context;
        private readonly UserManager<BlogUser> _userManager;
        private readonly SignInManager<BlogUser> _signInManager;

        public AuthRepository(AppDbContext context, UserManager<BlogUser> userManager, SignInManager<BlogUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<BlogUser> RegisterUserAsync(BlogUser userInfo, string password)
        {
            var result = await _userManager.CreateAsync(userInfo, password);

            if (result.Succeeded)
            {
                return await _userManager.FindByEmailAsync(userInfo.Email);
            }
            else
            {
                StringBuilder errors = new StringBuilder();
                foreach (var error in result.Errors)
                {
                    errors.AppendLine(error.Description.ToString());
                }
                throw new FailedRegistrationException(errors.ToString());
            }
        }

        public async Task<BlogUser> LogInUserAsync(LogInRequestViewModel userInfo)
        {
            var user = await _userManager.FindByNameAsync(userInfo.UserName);
            if (user is null)
                throw new UserDoesNotExistsException();
            var result = await _signInManager.CheckPasswordSignInAsync(user, userInfo.Password, false);

            if (result.Succeeded)
            {
                if (_context.UserTokens.Where(t => t.UserId == user.Id).Any())
                {
                    await RemoveAuthenticationTokensForUserAsync(user.Id);
                }
                return user;
            }

            else
                throw new FailedLogInException();
        }
        /// <summary>
        /// validates user who wants to get new access and refresh tokens
        /// </summary>
        /// <param name="tokens"> access and refresh tokens</param>
        /// <param name="principal"> claim principal of access token's user</param>
        /// <returns>user</returns>
        /// <exception cref="UserDoesNotExistsException"></exception>
        /// <exception cref="SecurityTokenException"></exception>
        public async Task<BlogUser> ValidateUser(TokensViewModel tokens, ClaimsPrincipal principal)
        {
            var userName = principal.Identity?.Name;
            var user = await _userManager.FindByNameAsync(userName);
            if (user is null)
                throw new UserDoesNotExistsException();
            var userRefreshToken = _context.UserTokens.Where(t => t.UserId == user.Id && t.Name == tokens.RefreshToken.TokenName.ToString())
                .SingleOrDefault();
            if (userRefreshToken is null || userRefreshToken.Value != tokens.RefreshToken.TokenValue
                || userRefreshToken.ExpirationTime <= DateTime.Now)
                throw new SecurityTokenException("Invalid Token!");
            return user;
        }
        public async Task SetAuthenticationTokenForUserAsync(BlogUser user, string provider, TokenNameType tokenName, string token, DateTime? expirationTime = null)
        {
            BlogUserTokens userToken = new()
            {
                UserId = user.Id,
                ExpirationTime = expirationTime,
                LoginProvider = provider,
                Name = tokenName.ToString(),
                Value = token
            };
            await _context.UserTokens.AddAsync(userToken);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveAuthenticationTokensForUserAsync(string userId)
        {
            var tokens = _context.UserTokens.Where(t => t.UserId == userId).ToList();
            if (tokens is not null && tokens.Count > 0)
            {
                foreach (var token in tokens)
                {
                    _context.UserTokens.Remove(token);
                }
                await _context.SaveChangesAsync();
            }
        }
    }
}

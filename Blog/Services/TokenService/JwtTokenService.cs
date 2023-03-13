using Blog.Models;
using Blog.ViewModels;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Blog.Services.TokenService
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly IConfiguration _config;

        public JwtTokenService(IConfiguration config)
        {
            _config = config;
        }

        public Task<TokenInfo> GenerateJwtAccessToken(BlogUser user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("FullName", user.FullName)
            };

            var result = GenerateJwtToken(_config["JWT:Issuer"], _config["JWT:Audience"], _config["JWT:SecretKey"], 10, claims);
            TokenInfo token = new()
            {
                TokenName = TokenNameType.Access,
                TokenValue = result.Result
            };
            return Task.FromResult(token);
        }

        public Task<TokenInfo> GenerateRefreshToken(DateTime? expirationTime = null)
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            TokenInfo refreshToken = new()
            {
                TokenName = TokenNameType.Refresh,
                TokenValue = Convert.ToBase64String(randomNumber),
                TokenExpirationTime = expirationTime ?? DateTime.Now.AddDays(3)
            };
            return Task.FromResult(refreshToken);
        }

        /// <summary>
        /// checks token validation and gets claim principal from it
        /// </summary>
        /// <param name="token">jwt access token</param>
        /// <returns>user claim principal</returns>
        /// <exception cref="SecurityTokenException"></exception>
        public Task<ClaimsPrincipal> GetPrincipalFromToken(string token)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            TokenValidationParameters validationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:SecretKey"])),
                ValidateLifetime = false
            };

            var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg
                .Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid Token!");
            return Task.FromResult(principal);

        }

        private Task<string> GenerateJwtToken(string issuer, string audience, string secretKey, double expirationTimeInMinutes, IEnumerable<Claim>? claims = null)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credintials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(issuer, audience, claims,
                expires: DateTime.Now.AddMinutes(expirationTimeInMinutes),
                signingCredentials: credintials);

            return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
        }

    }
}

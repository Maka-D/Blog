using AutoMapper;
using Blog.Data.Repositories.AuthRepository;
using Blog.Models;
using Blog.Services.TokenService;
using Blog.ViewModels;

namespace Blog.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IMapper _mapper;
        private readonly IJwtTokenService _tokenService;
        public AuthService(IAuthRepository authRepository, IMapper mapper, IJwtTokenService tokenService)
        {
            _authRepository = authRepository;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        /// <summary>
        /// if user registered successfully returns true if not false
        /// </summary>
        /// <param name="userInput"></param>
        /// <returns></returns>
        public async Task<bool> RegisterUserAsync(RegisterViewModel userInput)
        {
            if(userInput is null)
                throw new ArgumentNullException(nameof(userInput));
            var user = await _authRepository.RegisterUserAsync(_mapper.Map<BlogUser>(userInput), userInput.Password);
            return user != null;
        }
        /// <summary>
        /// if user loged in successfully returns user with access and refresh tokens
        /// </summary>
        /// <param name="userInput"></param>
        /// <returns></returns>
        public async Task<LogInResponseViewModel> LogInUserAsync(LogInRequestViewModel userInput)
        {
            if (userInput is null)
                throw new ArgumentNullException(nameof(userInput));

            var user = await _authRepository.LogInUserAsync(userInput);

            var accessToken = await _tokenService.GenerateJwtAccessToken(user);
            var refreshToken = await _tokenService.GenerateRefreshToken();

            await _authRepository.SetAuthenticationTokenForUserAsync(user, accessToken.Provider, 
                accessToken.TokenName, accessToken.TokenValue);
            await _authRepository.SetAuthenticationTokenForUserAsync(user, refreshToken.Provider,
                refreshToken.TokenName, refreshToken.TokenValue, refreshToken.TokenExpirationTime);
            
            var loggedUser = _mapper.Map<LogInResponseViewModel>(user);

            loggedUser.AccessToken = accessToken;
            loggedUser.RefreshToken = refreshToken;

            return loggedUser;
        }

        /// <summary>
        /// if user is validated removes old tokens from database
        /// creates new tokens da returns to the user 
        /// </summary>
        /// <param name="tokens">refresh and access tokens</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<TokensViewModel> RefreshTokensAsync(TokensViewModel tokens)
        {
            if(tokens is null)
                throw new ArgumentNullException(nameof(tokens));
            var principal = await _tokenService.GetPrincipalFromToken(tokens.AccessToken.TokenValue);
            var validatedUser = await _authRepository.ValidateUser(tokens, principal);

            await _authRepository.RemoveAuthenticationTokensForUserAsync(validatedUser.Id);
           
            TokensViewModel response = new()
            {
                AccessToken = await _tokenService.GenerateJwtAccessToken(validatedUser),
                RefreshToken = await _tokenService.GenerateRefreshToken()
            };

            await _authRepository.SetAuthenticationTokenForUserAsync(validatedUser, response.AccessToken.Provider, 
                response.AccessToken.TokenName, response.AccessToken.TokenValue);
            await _authRepository.SetAuthenticationTokenForUserAsync(validatedUser, response.RefreshToken.Provider, 
                response.RefreshToken.TokenName, response.RefreshToken.TokenValue, response.RefreshToken.TokenExpirationTime);

            return response;
        }
        public async Task RemoveTokensForUserAsync(string userId)
        {
            await _authRepository.RemoveAuthenticationTokensForUserAsync(userId);
        }
    }
}

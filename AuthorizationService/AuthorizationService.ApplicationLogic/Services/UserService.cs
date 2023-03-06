using AuthorizationService.API.Models;
using AuthorizationService.ApplicationLogic.Interfaces;
using AuthorizationService.DAL.Entities;
using AuthorizationService.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AuthorizationService.ApplicationLogic.Services
{
    public class UserService : IUserService
    {

        private readonly IRepository<User> _repository;
        private readonly IRepository<RefreshToken> _tokenRepository;

        public UserService(IRepository<User> repository, IRepository<RefreshToken> tokenRepository)
        {
            _repository = repository;
            _tokenRepository = tokenRepository;
        }

        public async Task<LoginResponse?> GenerateNewTokensPair(string token)
        {
            var requiredToken = (await _tokenRepository.GetAsync(filter: t => t.Token == token, include: t => t.Include(r => r.User).ThenInclude(u => u.Group))).FirstOrDefault();
            if (requiredToken is not null)
            {
                var (accessToken, refreshToken) = CreateTokens(requiredToken.User.Login, requiredToken.User.Group.Name);
                requiredToken.Token = refreshToken;
                await _tokenRepository.UpdateAsync(requiredToken);
                return new LoginResponse { AccessToken = accessToken, RefreshToken = refreshToken };
            }

            return null;
        }

        public async Task<LoginResponse?> TryGetJwtToken(LoginRequest model)
        {
            var requiredUser = (await _repository.GetAsync(filter: user => user.Login == model.Login, include: u => u.Include(user => user.Group).Include(user => user.RefreshTokens))).FirstOrDefault();
            if (requiredUser != null && PasswordHasher.Verify(model.Password, requiredUser.Password))
            {
                var (accessToken, refreshToken) = CreateTokens(requiredUser.Login, requiredUser.Group.Name);
                requiredUser.RefreshTokens.Add(new RefreshToken { Token = refreshToken, User = requiredUser });
                await _repository.UpdateAsync(requiredUser);

                return new LoginResponse { AccessToken = accessToken, RefreshToken = refreshToken };

            }

            return null;

        }

        private (string accessToken, string refreshToken) CreateTokens(string login, string role)
        {
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, login), new Claim("UserGroup", role) };
            var tokenHandler = new JwtSecurityTokenHandler();

            var accessToken = new JwtSecurityToken(
                    issuer: Constants.Issuer,
                    audience: Constants.Audience,
                    claims: claims,
                    expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(15)),
                    signingCredentials: new SigningCredentials(Constants.Key, SecurityAlgorithms.HmacSha256));

            var refreshToken = new JwtSecurityToken(
                  issuer: Constants.Issuer,
                  audience: Constants.Audience,
                  claims: claims,
                  expires: DateTime.UtcNow.Add(TimeSpan.FromDays(30)),
                  signingCredentials: new SigningCredentials(Constants.KeyForRefreshToken, SecurityAlgorithms.HmacSha256));

            return (tokenHandler.WriteToken(accessToken), tokenHandler.WriteToken(refreshToken));
        }

    }
}

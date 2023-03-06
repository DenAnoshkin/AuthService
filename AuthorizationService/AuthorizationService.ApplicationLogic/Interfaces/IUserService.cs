using AuthorizationService.API.Models;

namespace AuthorizationService.ApplicationLogic.Interfaces
{
    public interface IUserService
    {
        Task<LoginResponse?> TryGetJwtToken(LoginRequest model);

        Task<LoginResponse?> GenerateNewTokensPair(string token);

    }
}

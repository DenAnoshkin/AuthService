using AuthorizationService.API.Models;
using AuthorizationService.ApplicationLogic.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using AuthorizationService.ApplicationLogic;

namespace AuthorizationService.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var tokens = await _service.TryGetJwtToken(request);
            if (tokens != null)
            {
                return Ok(tokens);
            }

            return Unauthorized("Неверный логин или пароль");
        }


        [HttpGet("NewTokenPair")]
        public async Task<IActionResult> GetNewTokenPair()
        {
            var authorizationHeader = Request.Headers["Authorization"].ToString().Replace("Bearer ", string.Empty);
            var tokens = await _service.GenerateNewTokensPair(authorizationHeader);
            if (tokens != null)
            {
                return Ok(tokens);
            }

            return Unauthorized("Неверный refresh токен");
        }


        [HttpGet("test")]
        [Authorize]
        public IActionResult Test()  //тестовый метод, который бы что-то делал в финальной программе в зависимости от прав пользователя
        {
            var authorizationHeader = Request.Headers["Authorization"].ToString();
            var token = new JwtSecurityTokenHandler().ReadJwtToken(authorizationHeader.Replace("Bearer ", string.Empty));
            var permission = token.Claims.First(c => c.Type == "UserGroup").Value;
            return Ok("Ваши права: " + permission);
        }

        [HttpGet("PasswordHash")]
        public string GetPasswordHash(string password)    // вспомогательный метод для ручного добавления пароля в бд (его не будет в финальной версии)
        {
            return  PasswordHasher.Hash(password);
        }

    }
}

using System.ComponentModel.DataAnnotations;

namespace AuthorizationService.API.Models
{
    public class LoginRequest
    {
        [MinLength(5, ErrorMessage = "Минимальная длина логина 5 символов")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Логин не должен быть пустым")]
        public string Login { get; set; } = null!;

        [MinLength(5, ErrorMessage = "Минимальная длина пароля 5 символов")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Пароль не должен быть пустым")]
        public string Password { get; set; } = null!;

        [MinLength(5, ErrorMessage = "Минимальная длина пароля 5 символов")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Пароль для подтверждения не должен быть пустым")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; } = null!;

    }

    public class LoginResponse
    {
        public string AccessToken { get; set; } = null!;

        public string RefreshToken { get; set; } = null!;

    }
}

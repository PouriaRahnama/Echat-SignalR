using System.ComponentModel.DataAnnotations;

namespace Echat.Application.ViewModels.Auth
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "نام کاربری اجباری است")]
        public string UserName { get; set; }
        [Required(ErrorMessage = " کلمه عبور اجباری است")]
        public string Password { get; set; }
        [Compare("Password", ErrorMessage = "کلمه های عبور یکسان نیستند")]
        public string RePassword { get; set; }
    }
}
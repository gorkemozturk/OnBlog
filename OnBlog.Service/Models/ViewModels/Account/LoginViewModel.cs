using System.ComponentModel.DataAnnotations;

namespace OnBlog.Service.Models.ViewModels.Account
{
    public class LoginViewModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}

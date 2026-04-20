using System.ComponentModel.DataAnnotations;

namespace BniSittingManager.Models
{
    public class LoginViewModel
{
        [Required(ErrorMessage = "User Name is required")]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }

    }
}

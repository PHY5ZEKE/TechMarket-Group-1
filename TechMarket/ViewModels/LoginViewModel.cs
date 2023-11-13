using System.ComponentModel.DataAnnotations;

namespace TechMarket.ViewModels
{
    public class LoginViewModel
    {
            [Display(Name = "User Name")]
            [Required(ErrorMessage = "a username is required")]
            public string? UserName { get; set; }
            [DataType(DataType.Password)]
            [Required(ErrorMessage = "a password is required")]
            public string? Password { get; set; }
            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        
    }
}

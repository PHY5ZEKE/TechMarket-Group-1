using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace TechMarket.ViewModels
{
    public class RegisterViewModel
    {
        
        [Display(Name = "First Name")]
        [Required(ErrorMessage = "This field is required!")]
        public string? FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "This field is required!")]
        public string? LastName { get; set; }

        [Display(Name = "Email Address")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Email address is required!")]
       
        public string? Email { get; set; }

        [Display(Name = "User Name")]
        [Required(ErrorMessage = "A username is required")]
        [Remote("IsUsernameUnique", "User", ErrorMessage = "Username is already taken.")]
        public string? UserName { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "A password is required")]
        public string? Password { get; set; }

        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "You must confirm your password")]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string? ConfirmPassword { get; set; }

        [Display(Name = "Address")]
        [Required(ErrorMessage = "This field is required!")]
        public string Address { get; set; }

        [Display(Name = "Birthday")]
        [Required(ErrorMessage = "This field is required!")]
        [DataType(DataType.Date)]
        public DateTime Birthday { get; set; }

        [Display(Name = "Phone Number")]
        [Required(ErrorMessage = "This field is required1")]
        public string? Phone { get; set; }

        [Display(Name = "Profile Picture")]
        public IFormFile? ProfilePicture { get; set; }

        [Display(Name = "ID Picture")]
        public IFormFile? IdPicture { get; set; }

        
    }
}


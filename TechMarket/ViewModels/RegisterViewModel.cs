using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
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

        // Segmented Address Fields
        [Display(Name = "Building/House Number")]
        public string? BuildingNumber { get; set; }

        [Display(Name = "Street Name")]
        public string? StreetName { get; set; }

        [Display(Name = "Barangay")]
        public string? Barangay { get; set; }

        [Display(Name = "City/Municipality")]
        public string? CityOrMunicipality { get; set; }

        [Display(Name = "Province")]
        public string? Province { get; set; }

        [Display(Name = "Postal Code")]
        public string? PostalCode { get; set; }
        // End of Segmented Address Fields

        [Display(Name = "Birthday")]
        [Required(ErrorMessage = "This field is required!")]
        [DataType(DataType.Date)]
        public DateTime Birthday { get; set; }

        [Display(Name = "Phone Number")]
        [Required(ErrorMessage = "This field is required!")]
        public string? Phone { get; set; }

        [Display(Name = "Profile Picture")]
        public IFormFile? ProfilePicture { get; set; }

        [Display(Name = "ID Picture")]
        public IFormFile? IdPicture { get; set; }
    }
}

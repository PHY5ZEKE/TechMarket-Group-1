using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechMarket.Data
{
    public class User:IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set;}
        public DateTime? Birthday { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        [NotMapped]
        public IFormFile ProfilePicture { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public string? IdPictureUrl { get; set; }
        public string? ConfirmPassword { get; set; }
    }
}

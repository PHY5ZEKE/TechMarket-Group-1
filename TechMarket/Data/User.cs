using Microsoft.AspNetCore.Identity;

namespace TechMarket.Data
{
    public class User:IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set;}
        public DateTime? Birthday { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
    }
}

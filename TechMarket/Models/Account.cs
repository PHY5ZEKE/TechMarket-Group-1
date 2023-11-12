using System.ComponentModel.DataAnnotations;

namespace TechMarket.Models
{
    public class Account
    {
        [Key]
        public int AcctId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public DateTime Birthday { get; set; }
        public string ContactNo { get; set; }
    }
}

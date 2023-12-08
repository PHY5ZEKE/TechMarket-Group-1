using System.ComponentModel.DataAnnotations;

namespace TechMarket.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }
        public Guid SellerId { get; set; }
        public string ReviewerName { get; set; }
        public string Comment { get; set; }
    }
}

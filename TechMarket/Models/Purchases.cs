
using System.ComponentModel.DataAnnotations;

namespace TechMarket.Models
{
    public class Purchases
    {
        [Key]
        public int PurchasedProductId { get; set; }
        public string PurchaserId { get; set; } // User ID of the purchaser
        public int ProductId { get; set; } // ID of the purchased product
        public DateTime PurchaseDate { get; set; } // Date and time of the purchase
    }
}

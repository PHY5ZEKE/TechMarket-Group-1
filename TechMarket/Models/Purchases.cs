
using System.ComponentModel.DataAnnotations;
using TechMarket.Data;

namespace TechMarket.Models
{
    public class Purchases
    {
        [Key]
        public int PurchasedProductId { get; set; }
        public string PurchaserId { get; set; } // User ID of the purchaser
        public User Purchaser { get; set; }
        public int ProductId { get; set; } // ID of the purchased product
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public string ProductImageURL { get; set; }
        public int ProductPrice { get;set; }
        public DateTime PurchaseDate { get; set; } // Date and time of the purchase
    }
}

using System.ComponentModel.DataAnnotations;

namespace TechMarket.Models
{
    public class ToReceiveProduct
    {
        [Key]
        public int ToReceiveProductId { get; set; }
        public string BuyerId { get; set; }
        public string BuyerAddress { get; set; }
        public string BuyerName { get; set; }
        public string BuyerEmail { get; set; }
        public string BuyerContact { get; set; }
        public Guid SellerId { get; set; }
        public string SellerName {get; set; }
        public string SellerEmail { get; set; }
        public string SellerContact { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public string ProductImageURL { get; set; }
        public int ProductPrice { get; set; }
        public DateTime PurchaseDate { get; set; } 
    }
}

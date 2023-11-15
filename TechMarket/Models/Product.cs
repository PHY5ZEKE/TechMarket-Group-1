using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechMarket.Models
{
    public enum ProdTags
    {
        Smartphones, Computers, Audio, Cameras, Accessories, Misc
    }
    public class Product
    {
        [Key]
        public int ProdId { get; set; }
        public Guid AcctId { get; set; }
        public string Seller { get; set; }
        [Required]
        [NotMapped]
        public IFormFile ProdImage { get; set; }
        public string ProdName { get; set; }
        public string ProdDesc { get; set; }
        public ProdTags ProdTags { get; set; }
        public int ProdQuantity { get; set; }
        public string ProdPrice { get; set; }
       
    }
}

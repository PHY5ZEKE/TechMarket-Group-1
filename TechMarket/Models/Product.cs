using System.ComponentModel.DataAnnotations;

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
        public int AcctId { get; set; }
        public string Username { get; set; }
        public string ProdImage { get; set; }
        public string ProdName { get; set; }
        public string ProdDesc { get; set; }
        public ProdTags ProdTags { get; set; }
        public int ProdQuantity { get; set; }
        public string ProdPrice { get; set; }
       
    }
}

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
        public string SellerFName { get; set; }
        public string SellerLName { get; set; }
        public string SellerEmail { get; set; }
        public string SellerContact { get;set; }
        public string SellerPfp { get; set; }
        public string SellerID { get; set; }
        [NotMapped]
        public IFormFile ProdImage { get; set; }
        public string ProdName { get; set; }
        public string ProdDesc { get; set; }
        public ProdTags ProdTags { get; set; }
        /*public int ProdQuantity { get; set; }*/
        public int ProdPrice { get; set; }

        public string ProdImageURL { get; set; }

        

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using System.Linq;

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
        public string SellerContact { get; set; }
        public string SellerPfp { get; set; }
        public string SellerID { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "Image is required.")]
        [DataType(DataType.Upload)]
        [AllowedExtensions(new string[] { ".jpg", ".jpeg", ".png", ".gif" }, ErrorMessage = "Only image files are allowed.")]
        public IFormFile ProdImage { get; set; }

        public string ProdName { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(60, ErrorMessage = "Description cannot exceed 60 characters.")]
        public string ProdDesc { get; set; }

        [Required(ErrorMessage = "Category is required.")]
        public ProdTags ProdTags { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(50, 999999, ErrorMessage = "Price must be between 50 and 999999.")]
        public int ProdPrice { get; set; }
        public string ProdImageURL { get; set; }
        public ICollection<Review> Reviews { get; set; }
    }

    // Custom validation attribute to check file extensions
    public class AllowedExtensionsAttribute : ValidationAttribute
    {
        private readonly string[] _extensions;

        public AllowedExtensionsAttribute(string[] extensions)
        {
            _extensions = extensions;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is IFormFile file)
            {
                var extension = System.IO.Path.GetExtension(file.FileName);
                if (!_extensions.Contains(extension.ToLower()))
                {
                    return new ValidationResult(GetErrorMessage());
                }
            }

            return ValidationResult.Success;
        }

        public string GetErrorMessage()
        {
            return $"Allowed file extensions are: {string.Join(", ", _extensions)}";
        }
    }
}

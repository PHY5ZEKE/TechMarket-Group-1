using Microsoft.AspNetCore.Mvc;
using TechMarket.Data;
using TechMarket.Models;

namespace TechMarket.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _dbContext;

        public ProductController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("IsLoggedIn") != "true")
            {
                return RedirectToAction("Login", "Account");
            }
            return View(_dbContext.Products);
        }
        public IActionResult ShowDetails(int id)
        {

            Product? product = _dbContext.Products.FirstOrDefault(st => st.ProdId == id);

            if (product != null)
            {
                return View(product);
            }
            return NotFound();
        }
        [HttpGet]
        public IActionResult AddProduct()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddProduct(Product newProduct)
        {
            _dbContext.Products.Add(newProduct);
            _dbContext.SaveChanges();
            return RedirectToAction("Index");

        }
        [HttpGet]
        public IActionResult EditProduct(int id)
        {

            Product? product = _dbContext.Products.FirstOrDefault(st => st.ProdId == id);

            if (product != null)
            {
                return View(product);
            }
            return NotFound();
        }
        [HttpPost]
        public IActionResult EditProduct(Product updateProduct)
        {

           Product? product = _dbContext.Products.FirstOrDefault(st => st.ProdId == updateProduct.ProdId);

            if (product != null)
            {
                    product.ProdId = updateProduct.ProdId;
                   
      
                    product.ProdImage = updateProduct.ProdImage;
                    product.ProdName = updateProduct.ProdName;
                    product.ProdDesc = updateProduct.ProdDesc;
                    product.ProdTags = updateProduct.ProdTags;
                    product.ProdQuantity = updateProduct.ProdQuantity;
                    product.ProdPrice = updateProduct.ProdPrice;
            }
            _dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult DeleteProduct(int id)
        {
            Product? products = _dbContext.Products.FirstOrDefault(st => st.ProdId == id);

            if (products != null)
            {
                return View(products);
            }

            return NotFound();

        }
        [HttpPost]
        public IActionResult DeleteProduct(Product delProduct)
        {
            Product? products= _dbContext.Products.FirstOrDefault(st => st.ProdId == delProduct.ProdId);
            _dbContext.Products.Remove(products);
            _dbContext.SaveChanges();
            return RedirectToAction("Index");

        }
    }
}

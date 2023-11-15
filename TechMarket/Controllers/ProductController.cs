using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using TechMarket.Data;
using TechMarket.Models;

namespace TechMarket.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly UserManager<User> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(AppDbContext dbContext, UserManager<User> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            
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

        [HttpPost]
        public async Task<IActionResult> AddProduct(Product newProduct)
        {
            // Retrieve user ID and username from the currently logged-in user
            var user = _userManager.GetUserAsync(User).Result;

            if (user != null)
            {
                // Set the user ID and username for the new product
                newProduct.AcctId = Guid.Parse(user.Id);
                newProduct.Seller = user.UserName;

                if(newProduct.ProdImage!=null)
                {
                    string folder = "products";
                    folder += Guid.NewGuid().ToString()+"_"+newProduct.ProdImage.FileName;
                    string serverFolder =Path.Combine(_webHostEnvironment.WebRootPath,folder);

                   await newProduct.ProdImage.CopyToAsync(new FileStream(serverFolder,FileMode.Create));
                }
                // Add the product to the database
                _dbContext.Products.Add(newProduct);
                _dbContext.SaveChanges();

               
            }

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
            Product? product = _dbContext.Products.FirstOrDefault(st => st.ProdId == id);

            if (product != null)
            {
                return View(product);
            }

            return NotFound();

        }
        [HttpPost]
        public IActionResult DeleteProduct(Product delProduct)
        {
            Product? product= _dbContext.Products.FirstOrDefault(st => st.ProdId == delProduct.ProdId);
            _dbContext.Products.Remove(product);
            _dbContext.SaveChanges();
            return RedirectToAction("Index");

        }
    }
}

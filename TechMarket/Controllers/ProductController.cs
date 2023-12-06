using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http; 
using TechMarket.Data;
using TechMarket.Models;
using Stripe.Checkout;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace TechMarket.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly UserManager<User> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private const string UserProductsSessionKey = "UserProducts";

        public ProductController(AppDbContext dbContext, UserManager<User> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index(string searchQuery)
        {
            IQueryable<Product> products = _dbContext.Products;

            if (!string.IsNullOrEmpty(searchQuery))
            {
                // If a search query is provided, filter products based on the query
                products = products.Where(p =>
                    EF.Functions.Like(p.ProdName, $"%{searchQuery}%") ||
                    EF.Functions.Like(p.ProdDesc, $"%{searchQuery}%"));
            }

            if (User.Identity.IsAuthenticated)
            {
                var loggedInUser = _userManager.GetUserAsync(User).Result;
                HttpContext.Session.SetString("UserName", loggedInUser.UserName);
                HttpContext.Session.SetString("Address", loggedInUser.Address);
                HttpContext.Session.SetString("FirstName", loggedInUser.FirstName);
                HttpContext.Session.SetString("LastName", loggedInUser.LastName);
                HttpContext.Session.SetString("Email", loggedInUser.Email);
                HttpContext.Session.SetString("Phone", loggedInUser.Phone);
                HttpContext.Session.SetString("Birthday", loggedInUser.Birthday.ToString());
                HttpContext.Session.SetString("ProfilePictureUrl", loggedInUser.ProfilePictureUrl);
                HttpContext.Session.SetString("IdPictureUrl", loggedInUser.IdPictureUrl);
            }

            return View(products.ToList());
        }



        public IActionResult ShowDetails(int id)
        {
            Product product = _dbContext.Products.FirstOrDefault(st => st.ProdId == id);
            if (product != null)
            {
                HttpContext.Session.SetObject("SelectedProduct", product);
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
        public async Task<IActionResult> AddProduct(Product newProduct)
        {
            var user = _userManager.GetUserAsync(User).Result;
            if (user != null)
            {
                newProduct.AcctId = Guid.Parse(user.Id);
                newProduct.Seller = user.UserName;
                newProduct.SellerEmail = user.Email;
                newProduct.SellerContact = user.Phone;
                if (newProduct.ProdImage != null)
                {
                    string folder = "products/image/";
                    folder += Guid.NewGuid().ToString() + "_" + newProduct.ProdImage.FileName;
                    newProduct.ProdImageURL = folder;
                    string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, folder);
                    await newProduct.ProdImage.CopyToAsync(new FileStream(serverFolder, FileMode.Create));
                }
                _dbContext.Products.Add(newProduct);
                _dbContext.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult EditProduct(int id)
        {
            Product product = _dbContext.Products.FirstOrDefault(st => st.ProdId == id);
            if (product != null)
            {
                return View(product);
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult EditProduct(Product updateProduct, IFormFile newImage)
        {
            Product product = _dbContext.Products.FirstOrDefault(st => st.ProdId == updateProduct.ProdId);
            if (product != null)
            {
                product.ProdName = updateProduct.ProdName;
                product.ProdDesc = updateProduct.ProdDesc;
                product.ProdTags = updateProduct.ProdTags;
                product.ProdPrice = updateProduct.ProdPrice;
                if (newImage != null)
                {
                    if (!string.IsNullOrEmpty(product.ProdImageURL))
                    {
                        string oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, product.ProdImageURL);
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    string folder = "products/image/";
                    folder += Guid.NewGuid().ToString() + "_" + newImage.FileName;
                    product.ProdImageURL = folder;
                    string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, folder);
                    using (var stream = new FileStream(serverFolder, FileMode.Create))
                    {
                        newImage.CopyTo(stream);
                    }
                }
            }
            _dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult DeleteProduct(int id)
        {
            Product product = _dbContext.Products.FirstOrDefault(p => p.ProdId == id);
            if (product != null)
            {
                return View(product);
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult DeleteProduct(Product delProduct)
        {
            Product product = _dbContext.Products.FirstOrDefault(p => p.ProdId == delProduct.ProdId);
            _dbContext.Products.Remove(product);
            _dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Smartphones(string searchQuery)
        {
            IQueryable<Product> products = _dbContext.Products;

            if (!string.IsNullOrEmpty(searchQuery))
            {
                // If a search query is provided, filter products based on the query
                products = products.Where(p =>
                    EF.Functions.Like(p.ProdName, $"%{searchQuery}%") ||
                    EF.Functions.Like(p.ProdDesc, $"%{searchQuery}%"));
            }

            // Filter products with ProdTags.Smartphones
            var smartphones = products.Where(p => p.ProdTags == ProdTags.Smartphones).ToList();

            return View("Smartphones", smartphones);
        }


        public IActionResult Computers(string searchQuery)
        {
            IQueryable<Product> products = _dbContext.Products;

            if (!string.IsNullOrEmpty(searchQuery))
            {
                products = products.Where(p =>
                    EF.Functions.Like(p.ProdName, $"%{searchQuery}%") ||
                    EF.Functions.Like(p.ProdDesc, $"%{searchQuery}%"));
            }
            var computers = products.Where(p => p.ProdTags == ProdTags.Computers).ToList();

            return View("Computers", computers);
        }

        public IActionResult Audio(string searchQuery)
        {
            IQueryable<Product> products = _dbContext.Products;
            if (!string.IsNullOrEmpty(searchQuery))
            {
                products = products.Where(p =>
                    EF.Functions.Like(p.ProdName, $"%{searchQuery}%") ||
                    EF.Functions.Like(p.ProdDesc, $"%{searchQuery}%"));
            }
            var audioProducts = products.Where(p => p.ProdTags == ProdTags.Audio).ToList();
            return View("Audio", audioProducts);
        }

        public IActionResult Cameras(string searchQuery)
        {
            IQueryable<Product> products = _dbContext.Products;
            if (!string.IsNullOrEmpty(searchQuery))
            {
                products = products.Where(p =>
                    EF.Functions.Like(p.ProdName, $"%{searchQuery}%") ||
                    EF.Functions.Like(p.ProdDesc, $"%{searchQuery}%"));
            }
            var cameras = products.Where(p => p.ProdTags == ProdTags.Cameras).ToList();
            return View("Cameras", cameras);
        }

        public IActionResult Accessories(string searchQuery)
        {
            IQueryable<Product> products = _dbContext.Products;
            if (!string.IsNullOrEmpty(searchQuery))
            {
                products = products.Where(p =>
                    EF.Functions.Like(p.ProdName, $"%{searchQuery}%") ||
                    EF.Functions.Like(p.ProdDesc, $"%{searchQuery}%"));
            }
            var accessories = products.Where(p => p.ProdTags == ProdTags.Accessories).ToList();
            return View("Accessories", accessories);
        }

        public IActionResult Misc(string searchQuery)
        {
            IQueryable<Product> products = _dbContext.Products;
            if (!string.IsNullOrEmpty(searchQuery))
            {
                products = products.Where(p =>
                    EF.Functions.Like(p.ProdName, $"%{searchQuery}%") ||
                    EF.Functions.Like(p.ProdDesc, $"%{searchQuery}%"));
            }
            var miscProducts = products.Where(p => p.ProdTags == ProdTags.Misc).ToList();
            return View("Misc", miscProducts);
        }

        public IActionResult Listing()
        {
            var loggedInUser = _userManager.GetUserAsync(User).Result;
            var userProducts = _dbContext.Products.Where(p => p.AcctId == Guid.Parse(loggedInUser.Id)).ToList();
            return View(userProducts);
        }

        public IActionResult Checkout()
        {
            var selectedProduct = HttpContext.Session.GetObject<Product>("SelectedProduct");
            if (selectedProduct == null)
            {
                return NotFound();
            }
            var domain = "http://localhost:5123/";
            var unitAmountInCentavos = (long)(selectedProduct.ProdPrice * 100);
            var options = new SessionCreateOptions
            {
                SuccessUrl = domain + $"Product/OrderConfirmation",
                CancelUrl = domain + "Product/Failure",
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
            };
            var sessionListItem = new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmount = unitAmountInCentavos,
                    Currency = "php",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = selectedProduct.ProdName,
                    }
                },
                Quantity = 1
            };
            options.LineItems.Add(sessionListItem);
            var service = new SessionService();
            Session session = service.Create(options);
            TempData["Session"] = session.Id;

            var loggedInUser = _userManager.GetUserAsync(User).Result;
            var purchasedProduct = new Purchases
            {
                PurchaserId = loggedInUser.Id,
                ProductId = selectedProduct.ProdId,
                PurchaseDate = DateTime.UtcNow,
                ProductName = selectedProduct.ProdName, // Add product name
                ProductImageURL = selectedProduct.ProdImageURL, // Add product image URL
                ProductPrice = selectedProduct.ProdPrice, // Add product price
                ProductDescription = selectedProduct.ProdDesc,
            };
            var toShipProduct = new ToShipProduct
            {
                BuyerId = loggedInUser.Id, // Add the buyer's ID
                BuyerAddress = loggedInUser.Address, // Add the buyer's address
                BuyerEmail = loggedInUser.Email,
                BuyerName = loggedInUser.FirstName+" " +loggedInUser.LastName,
                BuyerContact = loggedInUser.Phone,
                SellerId = selectedProduct.AcctId,
                SellerName = selectedProduct.Seller,
                SellerEmail = selectedProduct.SellerEmail,
                SellerContact = selectedProduct.SellerContact,
                ProductId = selectedProduct.ProdId,
                PurchaseDate = DateTime.UtcNow,
                ProductName = selectedProduct.ProdName, // Add product name
                ProductImageURL = selectedProduct.ProdImageURL, // Add product image URL
                ProductPrice = selectedProduct.ProdPrice, // Add product price
                ProductDescription = selectedProduct.ProdDesc,
            };
            _dbContext.ToShipProducts.Add(toShipProduct);
            _dbContext.PurchasedProducts.Add(purchasedProduct);
            _dbContext.SaveChanges();
            var productToDelete = _dbContext.Products.FirstOrDefault(p => p.ProdId == selectedProduct.ProdId);
            if (productToDelete != null)
            {
                _dbContext.Products.Remove(productToDelete);
                _dbContext.SaveChanges();
            }

            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }

        public IActionResult OrderConfirmation()
        {
            var service = new SessionService();
            Session session = service.Get(TempData["Session"].ToString());
            if (session.PaymentStatus == "paid")
            {
                var transaction = session.PaymentIntentId.ToString();
                return View("Success");
            }
            return View("Failure");
        }

        public IActionResult Success()
        {
            return View("Success");
        }

        public IActionResult Failure()
        {
            return View("Failure");
        }

        public IActionResult Purchases()
        {
            var loggedInUser = _userManager.GetUserAsync(User).Result;
            if (loggedInUser != null)
            {
                var userPurchases = _dbContext.PurchasedProducts
                    .Where(p => p.PurchaserId == loggedInUser.Id)
                    .ToList();

                return View(userPurchases);
            }

            return RedirectToAction("Index"); // Redirect to the product listing if the user is not logged in
        }

        public IActionResult ShowDetailsPurchases(int id)
        {
            var loggedInUser = _userManager.GetUserAsync(User).Result;
            if (loggedInUser != null)
            {
                var purchasedProduct = _dbContext.PurchasedProducts
                    .FirstOrDefault(p => p.PurchaserId == loggedInUser.Id && p.ProductId == id);

                if (purchasedProduct != null)
                {
                    // Assuming you have a view named "ShowDetailsPurchases" to display the details
                    return View(purchasedProduct);
                }
            }

            return NotFound();
        }
        public IActionResult ShowDetailsToShip(int id)
        {
            var loggedInUser = _userManager.GetUserAsync(User).Result;
            if (loggedInUser != null)
            {
                var toshipProducts = _dbContext.ToShipProducts
                    .FirstOrDefault(p => p.SellerId == Guid.Parse(loggedInUser.Id) && p.ProductId == id);

                if (toshipProducts != null)
                {
                    // Assuming you have a view named "ShowDetailsPurchases" to display the details
                    return View(toshipProducts);
                }
            }

            return NotFound();
        }
        public IActionResult ShowDetailsToReceive(int id)
        {
            var loggedInUser = _userManager.GetUserAsync(User).Result;
            if (loggedInUser != null)
            {
                var toReceiveProducts = _dbContext.ToReceiveProducts
                    .FirstOrDefault(p => p.BuyerId == loggedInUser.Id && p.ToReceiveProductId == id);

                if (toReceiveProducts != null)
                {
                    // Assuming you have a view named "ShowDetailsPurchases" to display the details
                    return View(toReceiveProducts);
                }
            }

            return NotFound();
        }
        [HttpPost]
        public IActionResult DeleteToReceiveProduct(int id)
        {
            var loggedInUser = _userManager.GetUserAsync(User).Result;
            if (loggedInUser != null)
            {
                var toReceiveProduct = _dbContext.ToReceiveProducts
                    .FirstOrDefault(p => p.BuyerId == loggedInUser.Id && p.ToReceiveProductId == id);

                if (toReceiveProduct != null)
                {
                    // Perform the actual deletion from the database
                    _dbContext.ToReceiveProducts.Remove(toReceiveProduct);
                    _dbContext.SaveChanges();

                    // Redirect to a success page or another appropriate action
                    return RedirectToAction("ToReceive", "Product");
                }
            }

            return NotFound();
        }

        public IActionResult ToShip()
        {
            var loggedInUser = _userManager.GetUserAsync(User).Result;
            if (loggedInUser != null)
            {
                // Fetch the products that the logged-in user has uploaded and that are purchased
                var productsToShip = _dbContext.ToShipProducts
                    .Where(p => p.SellerId == Guid.Parse(loggedInUser.Id))
                    .ToList();

                return View(productsToShip);
            }

            return RedirectToAction("Index"); // Redirect to the product listing if the user is not logged in
        }
        public IActionResult TransferToReceive(int toShipProductId)
        {
            // Get the logged-in user
            var loggedInUser = _userManager.GetUserAsync(User).Result;

            // Get the ToShipProduct that needs to be transferred
            var toShipProduct = _dbContext.ToShipProducts
                .FirstOrDefault(p => p.ToShipProductId == toShipProductId);

            if (toShipProduct != null)
            {
                // Create a new ToReceiveProduct instance and populate its properties
                var toReceiveProduct = new ToReceiveProduct
                {
                    BuyerId = toShipProduct.BuyerId,
                    BuyerAddress = toShipProduct.BuyerAddress,
                    BuyerEmail = loggedInUser.Email,
                    BuyerName = loggedInUser.FirstName + " " + loggedInUser.LastName,
                    BuyerContact = loggedInUser.Phone,
                    SellerId = toShipProduct.SellerId,
                    SellerName = toShipProduct.SellerName,
                    SellerEmail = toShipProduct.SellerEmail,
                    SellerContact = toShipProduct.SellerContact,
                    ProductId = toShipProduct.ProductId,
                    ProductName = toShipProduct.ProductName,
                    ProductDescription = toShipProduct.ProductDescription,
                    ProductImageURL = toShipProduct.ProductImageURL,
                    ProductPrice = toShipProduct.ProductPrice,
                    PurchaseDate = toShipProduct.PurchaseDate
                };

                // Add the ToReceiveProduct to the ToReceiveProducts database
                _dbContext.ToReceiveProducts.Add(toReceiveProduct);
                _dbContext.SaveChanges();

                // Remove the transferred product from the ToShipProducts database
                _dbContext.ToShipProducts.Remove(toShipProduct);
                _dbContext.SaveChanges();

                return RedirectToAction("ToShip"); // Redirect to the ToShipProducts page after transfer
            }

            return NotFound();
        }
        public IActionResult ToReceive()
        {
            var loggedInUser = _userManager.GetUserAsync(User).Result;
            if (loggedInUser != null)
            {
                // Fetch the products that the logged-in user is about to receive
                var productsToReceive = _dbContext.ToReceiveProducts
                    .Where(p => p.BuyerId == loggedInUser.Id)
                    .ToList();

                return View(productsToReceive);
            }

            return RedirectToAction("Index"); // Redirect to the product listing if the user is not logged in
        }






    }







    public static class SessionExtensions
    {
        public static void SetObject(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObject<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonConvert.DeserializeObject<T>(value);
        }
    }
}










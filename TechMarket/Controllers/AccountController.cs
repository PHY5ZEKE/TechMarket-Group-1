using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TechMarket.Data;
using TechMarket.Models;
using TechMarket.ViewModels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;

namespace TechMarket.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly AppDbContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AccountController(SignInManager<User> signInManager, AppDbContext dbContext, UserManager<User> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _dbContext = dbContext;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult login()
        {
            return View();
        }
     
        
        public IActionResult Profile()
        {

           
            return View("Profile");
        }




        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginInfo)
        {
            var result = await _signInManager.PasswordSignInAsync(loginInfo.UserName, loginInfo.Password, loginInfo.RememberMe, false);

            if (result.Succeeded)
            {


                // Check if the user is authenticated

            
                return RedirectToAction("Index", "Product");
            }
            else
            {
                ModelState.AddModelError("", "Failed to Login");
            }
            var products = _dbContext.Products.ToList();
            return View(loginInfo);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        public IActionResult Index()
        {
            var users = _userManager.Users.ToList();
            return View(users);
        }
        public IActionResult ShowDetails(Guid id)
        {

            User? user = _userManager.Users.FirstOrDefault(u => u.Id == id.ToString());

            if (user != null)
            {
                return View(user);
            }

            return NotFound();
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View("Register");
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel userEnteredData)
        {
            if (ModelState.IsValid)
            {
                User newUser = new User();
                newUser.UserName = userEnteredData.UserName;
                newUser.FirstName = userEnteredData.FirstName;
                newUser.LastName = userEnteredData.LastName;
                newUser.Email = userEnteredData.Email;
                newUser.Phone = userEnteredData.Phone;
                newUser.Birthday = userEnteredData.Birthday;
                newUser.Address = userEnteredData.Address;

                // Check and save profile picture
                if (userEnteredData.ProfilePicture != null && userEnteredData.ProfilePicture.Length > 0)
                {
                    var profilePictureFileName = $"{Guid.NewGuid()}_{userEnteredData.ProfilePicture.FileName}";
                    var profilePicturePath = Path.Combine(_webHostEnvironment.WebRootPath, "profile", "pfp", profilePictureFileName);

                    using (var stream = new FileStream(profilePicturePath, FileMode.Create))
                    {
                        await userEnteredData.ProfilePicture.CopyToAsync(stream);
                    }

                    newUser.ProfilePictureUrl = $"profile/pfp/{profilePictureFileName}";
                }

                // Check and save ID picture
                if (userEnteredData.IdPicture != null && userEnteredData.IdPicture.Length > 0)
                {
                    var idPictureFileName = $"{Guid.NewGuid()}_{userEnteredData.IdPicture.FileName}";
                    var idPicturePath = Path.Combine(_webHostEnvironment.WebRootPath, "profile", "id", idPictureFileName);

                    using (var stream = new FileStream(idPicturePath, FileMode.Create))
                    {
                        await userEnteredData.IdPicture.CopyToAsync(stream);
                    }

                    newUser.IdPictureUrl = $"profile/id/{idPictureFileName}";
                }

                var result = await _userManager.CreateAsync(newUser, userEnteredData.Password);

                if (result.Succeeded)
                {
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            return View(userEnteredData);
        }





        public IActionResult EditAccount(Guid id)
        {

            User? user = _userManager.Users.FirstOrDefault(u => u.Id == id.ToString());

            if (user != null)
            {
                return View(user);
            }
            return NotFound();
        }
        [HttpPost]
        public IActionResult EditAccount(User updateAccount)
        {
            // Get the user by Id
          User user = _userManager.FindByIdAsync(updateAccount.Id).Result;

            if (user != null)
            {
                // Update user properties
                user.FirstName = updateAccount.FirstName;
                user.LastName = updateAccount.LastName;
                user.Email = updateAccount.Email;
                user.UserName = updateAccount.UserName;
                user.Address = updateAccount.Address;
                user.Birthday = updateAccount.Birthday;
                user.Phone = updateAccount.Phone;


                // You may need to update other properties as needed

                // Update the user using UserManager
                var result = _userManager.UpdateAsync(user).Result;

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    // Handle the case where the update failed
                    ModelState.AddModelError(string.Empty, "Error updating user information.");
                    return View(updateAccount);
                }
            }

            return NotFound();
        }

        [HttpGet]
        public IActionResult DeleteAccount(Guid id)
        {
            User? user = _userManager.Users.FirstOrDefault(u => u.Id == id.ToString());

            if (user != null)
            {
                return View(user);
            }
            return NotFound();

        }
        [HttpPost]
        public IActionResult DeleteAccount(string id)
        {
            User user = _userManager.FindByIdAsync(id).Result;

            if (user != null)
            {
                // Use UserManager to delete the user
                var result = _userManager.DeleteAsync(user).Result;

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    // Handle the case where the deletion failed
                    ModelState.AddModelError(string.Empty, "Error deleting user.");
                    return View("Index"); // You might want to display an error message or redirect to a different view
                }
            }

            return NotFound();
        }
        









        // Existing code...


    }




}





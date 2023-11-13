using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TechMarket.Data;
using TechMarket.Models;
using TechMarket.ViewModels;
using System.Threading.Tasks;

namespace TechMarket.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly AppDbContext _dbContext;
        public AccountController(SignInManager<User> signInManager, AppDbContext dbContext, UserManager<User> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _dbContext = dbContext;
        }
        public IActionResult login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginInfo)
        {
            var result = await _signInManager.PasswordSignInAsync(loginInfo.UserName, loginInfo.Password, loginInfo.RememberMe, false);
            ;

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Product");
            }
            else
            {
                ModelState.AddModelError("", "Failed to Login");
            }

            return View(loginInfo);
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("IsLoggedIn") != "true")
            {
                return RedirectToAction("Login", "Account");
            }
            return View(_dbContext.Accounts);
        }
        public IActionResult ShowDetails(int id)
        {

            Account? account = _dbContext.Accounts.FirstOrDefault(st => st.AcctId == id);

            if (account != null)
            {
                return View(account);
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
                newUser.PhoneNumber = userEnteredData.Phone;
                newUser.Birthday = userEnteredData.Birthday;

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



        public IActionResult EditAccount(int id)
        {

            Account? account = _dbContext.Accounts.FirstOrDefault(st => st.AcctId == id);

            if (account != null)
            {
                return View(account);
            }
            return NotFound();
        }
        [HttpPost]
        public IActionResult EditAccount(Account updateAccount)
        {

            Account? account = _dbContext.Accounts.FirstOrDefault(st => st.AcctId == updateAccount.AcctId);

            if (account != null)
            {
                account.AcctId = updateAccount.AcctId;
                account.FirstName = updateAccount.FirstName;
                account.LastName = updateAccount.LastName;
                account.Email = updateAccount.Email;
                account.Username = updateAccount.Username;
                account.Password = updateAccount.Password;
                account.Address = updateAccount.Address;
                account.Birthday = updateAccount.Birthday;
                account.Phone = updateAccount.Phone;
            }
            _dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult DeleteAccount(int id)
        {
            Account? account = _dbContext.Accounts.FirstOrDefault(st => st.AcctId == id);

            if (account != null)
            {
                return View(account);
            }

            return NotFound();

        }
        [HttpPost]
        public IActionResult DeleteAccount(Account delAccount)
        {
            Account? account = _dbContext.Accounts.FirstOrDefault(st => st.AcctId == delAccount.AcctId);
            _dbContext.Accounts.Remove(account);
            _dbContext.SaveChanges();
            return RedirectToAction("Index");

        }
        


    }

}



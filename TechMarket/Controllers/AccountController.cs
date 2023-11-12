using Microsoft.AspNetCore.Mvc;
using TechMarket.Data;
using TechMarket.Models;

namespace TechMarket.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _dbContext;

        public AccountController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
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
        public IActionResult AddAccount()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddAccount(Account newAccount)
        {
            _dbContext.Accounts.Add(newAccount);
            _dbContext.SaveChanges();
            return RedirectToAction("Index");

        }
        [HttpGet]
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
                account.ContactNo = updateAccount.ContactNo;
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
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(Login login)
        {
            if (ModelState.IsValid && ValidateLogin(login.Username, login.Password))
            {
                // Retrieve user from the database based on the username
                Account user = _dbContext.Accounts.SingleOrDefault(a => a.Username == login.Username);

                // Check if the user exists
                if (user != null)
                {
                    // Set session variables for user ID and username
                    HttpContext.Session.SetInt32("UserId", user.AcctId);
                    HttpContext.Session.SetString("Username", user.Username);
                    HttpContext.Session.SetString("IsLoggedIn", "true");

                    return RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError(string.Empty, "Invalid username or password");
            return View("Login", login);
        }


        private bool ValidateLogin(string username, string password)
        {
            return _dbContext.Accounts.Any(a => a.Username == username && a.Password == password);
        }

    }
}


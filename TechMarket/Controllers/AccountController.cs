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
        public IActionResult EditAcount(Account updateAccount)
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
                account.Birthdate = updateAccount.Birthdate;
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

    }
}


using Microsoft.AspNetCore.Mvc;
using MoneyAccountingAppADONET.Data;
using MoneyAccountingAppADONET.Models;
using System.Collections.Generic;
using System.Data;

namespace MoneyAccountingAppADONET.Controllers
{
    public class AccountController : Controller
    {
        private readonly DatabaseHelper _databaseHelper;

        public AccountController(DatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
        }

        public IActionResult Index()
        {
            string query = "SELECT * FROM Accounts";
            DataTable dataTable = _databaseHelper.GetData(query);

            List<Account> accounts = new List<Account>();
            foreach (DataRow row in dataTable.Rows)
            {
                accounts.Add(new Account
                {
                    Id = (int)row["Id"],
                    Name = (string)row["Name"],
                    Balance = (decimal)row["Balance"]
                });
            }

            return View(accounts);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Account account)
        {
            if (ModelState.IsValid)
            {
                string query = $"INSERT INTO Accounts (Name, Balance) VALUES ('{account.Name}', {account.Balance})";
                _databaseHelper.ExecuteCommand(query);
                return RedirectToAction(nameof(Index));
            }
            return View(account);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            string query = $"DELETE FROM Accounts WHERE Id = {id}";
            _databaseHelper.ExecuteCommand(query);
            return RedirectToAction(nameof(Index));
        }
    }
}

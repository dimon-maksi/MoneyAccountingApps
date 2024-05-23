using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MoneyAccountingAppADONET.Data;
using MoneyAccountingAppADONET.Models;
using System.Collections.Generic;
using System.Data;

namespace MoneyAccountingAppADONET.Controllers
{
    public class SavingController : Controller
    {
        private readonly DatabaseHelper _databaseHelper;

        public SavingController(DatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
        }

        public IActionResult Index()
        {
            string query = "SELECT * FROM Savings";
            DataTable dataTable = _databaseHelper.GetData(query);

            List<Saving> savings = new List<Saving>();
            foreach (DataRow row in dataTable.Rows)
            {
                savings.Add(new Saving
                {
                    Id = (int)row["Id"],
                    Sum = (decimal)row["Sum"],
                    Date = (DateTime)row["Date"],
                    AccountId = (int)row["AccountId"],
                    CategoryId = (int)row["CategoryId"],
                    Goal = (string)row["Goal"]
                });
            }

            return View(savings);
        }

        public IActionResult Create()
        {
            ViewBag.Accounts = GetAccounts(); // Populate dropdown for accounts
            ViewBag.Categories = GetCategories(); // Populate dropdown for categories
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Saving saving)
        {
            if (ModelState.IsValid)
            {
                // Retrieve the selected account and category from the form
                int accountId = saving.AccountId;
                int categoryId = saving.CategoryId;

                // Your SQL INSERT statement using accountId and categoryId
                string query = $"INSERT INTO Savings (Sum, Date, AccountId, CategoryId, Goal) VALUES ({saving.Sum}, '{saving.Date}', {saving.AccountId}, {saving.CategoryId}, '{saving.Goal}')";
                _databaseHelper.ExecuteCommand(query);

                // Update account balance
                UpdateAccountBalance(saving.AccountId, saving.Sum);

                return RedirectToAction(nameof(Index));
            }
            ViewBag.Accounts = GetAccounts(); // Populate dropdown for accounts
            ViewBag.Categories = GetCategories(); // Populate dropdown for categories
            return View(saving);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            // Retrieve the saving entry to be deleted
            Saving savingToDelete = GetSavingById(id);

            if (savingToDelete == null)
            {
                return NotFound();
            }

            // Delete the saving entry from the database
            string deleteQuery = $"DELETE FROM Savings WHERE Id = {id}";
            _databaseHelper.ExecuteCommand(deleteQuery);

            // Update the balance in the associated account
            UpdateAccountBalance(savingToDelete.AccountId, -savingToDelete.Sum);

            return RedirectToAction(nameof(Index));
        }
        private Saving GetSavingById(int id)
        {
            // Retrieve the saving entry by its ID
            string query = $"SELECT * FROM Savings WHERE Id = {id}";
            DataTable dataTable = _databaseHelper.GetData(query);

            if (dataTable.Rows.Count == 0)
            {
                return null;
            }

            DataRow row = dataTable.Rows[0];

            return new Saving
            {
                Id = (int)row["Id"],
                Sum = (decimal)row["Sum"],
                Date = (DateTime)row["Date"],
                AccountId = (int)row["AccountId"],
                CategoryId = (int)row["CategoryId"],
                Goal = row["Goal"].ToString()
            };
        }

        private List<SelectListItem> GetAccounts()
        {
            List<SelectListItem> accountsList = new List<SelectListItem>();

            // Query to retrieve accounts from the database
            string query = "SELECT Id, Name FROM Accounts";
            DataTable accountsData = _databaseHelper.GetData(query);

            // Transform retrieved data into SelectListItem objects
            foreach (DataRow row in accountsData.Rows)
            {
                int accountId = (int)row["Id"];
                string accountName = row["Name"].ToString();
                accountsList.Add(new SelectListItem(accountName, accountId.ToString()));
            }

            return accountsList;
        }

        private List<SelectListItem> GetCategories()
        {
            List<SelectListItem> categoriesList = new List<SelectListItem>();

            // Retrieve categories from the database
            string query = "SELECT Id, Name FROM Categories";
            DataTable categoriesData = _databaseHelper.GetData(query);

            // Transform retrieved data into SelectListItem objects
            foreach (DataRow row in categoriesData.Rows)
            {
                int categoryId = (int)row["Id"];
                string categoryName = row["Name"].ToString();
                categoriesList.Add(new SelectListItem(categoryName, categoryId.ToString()));
            }

            return categoriesList;
        }

        private void UpdateAccountBalance(int accountId, decimal amount)
        {
            // Determine whether the transaction is an income or an expense/saving
            bool isIncome = amount >= 0;

            // Determine the appropriate SQL operation based on the transaction type
            string operation = isIncome ? "-" : "+";

            // Calculate the absolute value of the amount
            decimal absoluteAmount = Math.Abs(amount);

            // Construct the SQL query to update the account balance
            string query = $"UPDATE Accounts SET Balance = Balance {operation} {absoluteAmount} WHERE Id = {accountId}";

            // Execute the SQL query
            _databaseHelper.ExecuteCommand(query);
        }
    }
}

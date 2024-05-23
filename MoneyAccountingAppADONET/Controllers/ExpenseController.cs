using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MoneyAccountingAppADONET.Data;
using MoneyAccountingAppADONET.Models;
using System.Collections.Generic;
using System.Data;

namespace MoneyAccountingAppADONET.Controllers
{
    public class ExpenseController : Controller
    {
        private readonly DatabaseHelper _databaseHelper;

        public ExpenseController(DatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
        }

        public IActionResult Index()
        {
            string query = "SELECT * FROM Expenses";
            DataTable dataTable = _databaseHelper.GetData(query);

            List<Expense> expenses = new List<Expense>();
            foreach (DataRow row in dataTable.Rows)
            {
                expenses.Add(new Expense
                {
                    Id = (int)row["Id"],
                    Sum = (decimal)row["Sum"],
                    Date = (DateTime)row["Date"],
                    AccountId = (int)row["AccountId"],
                    CategoryId = (int)row["CategoryId"],
                    Type = (string)row["Type"]
                });
            }

            return View(expenses);
        }
        public IActionResult Create()
        {
            ViewBag.Accounts = GetAccounts(); // Populate dropdown for accounts
            ViewBag.Categories = GetCategories(); // Populate dropdown for categories
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Expense expense)
        {
            if (ModelState.IsValid)
            {
                // Retrieve the selected account and category from the form
                int accountId = expense.AccountId;
                int categoryId = expense.CategoryId;

                // Your SQL INSERT statement using accountId and categoryId
                string query = $"INSERT INTO Expenses (Sum, Date, AccountId, CategoryId, Type) VALUES ({expense.Sum}, '{expense.Date}', {expense.AccountId}, {expense.CategoryId}, '{expense.Type}')";
                _databaseHelper.ExecuteCommand(query);

                // Update account balance
                UpdateAccountBalance(expense.AccountId, expense.Sum);

                return RedirectToAction(nameof(Index));
            }
            ViewBag.Accounts = GetAccounts(); // Populate dropdown for accounts
            ViewBag.Categories = GetCategories(); // Populate dropdown for categories
            return View(expense);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            // Retrieve the expense entry to be deleted
            Expense expenseToDelete = GetExpenseById(id);

            if (expenseToDelete == null)
            {
                return NotFound();
            }

            // Delete the expense entry from the database
            string deleteQuery = $"DELETE FROM Expenses WHERE Id = {id}";
            _databaseHelper.ExecuteCommand(deleteQuery);

            // Update the balance in the associated account
            UpdateAccountBalance(expenseToDelete.AccountId, -expenseToDelete.Sum);

            return RedirectToAction(nameof(Index));
        }

        private Expense GetExpenseById(int id)
        {
            // Retrieve the expense entry by its ID
            string query = $"SELECT * FROM Expenses WHERE Id = {id}";
            DataTable dataTable = _databaseHelper.GetData(query);

            if (dataTable.Rows.Count == 0)
            {
                return null;
            }

            DataRow row = dataTable.Rows[0];

            return new Expense
            {
                Id = (int)row["Id"],
                Sum = (decimal)row["Sum"],
                Date = (DateTime)row["Date"],
                AccountId = (int)row["AccountId"],
                CategoryId = (int)row["CategoryId"],
                Type = row["Type"].ToString()
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

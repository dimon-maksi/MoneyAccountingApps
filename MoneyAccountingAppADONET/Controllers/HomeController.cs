using Microsoft.AspNetCore.Mvc;
using MoneyAccountingAppADONET.Data;
using MoneyAccountingAppADONET.Models;
using System.Data;
using System.Diagnostics;

namespace MoneyAccountingAppADONET.Controllers
{
    public class HomeController : Controller
    {
        private readonly DatabaseHelper _databaseHelper;

        public HomeController(DatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
        }

        public IActionResult Index()
        {
            List<AccountSummaryViewModel> accountSummaries = new List<AccountSummaryViewModel>();


            string accountsQuery = @"
            SELECT 
                A.Name AS AccountName,
                A.Balance,
                COALESCE(SUM(I.Sum), 0) AS TotalIncomes,
                COALESCE(SUM(E.Sum), 0) AS TotalExpenses,
                COALESCE(SUM(S.Sum), 0) AS TotalSavings
            FROM 
                Accounts A
            LEFT JOIN 
                Income I ON A.Id = I.AccountId
            LEFT JOIN 
                Expenses E ON A.Id = E.AccountId
            LEFT JOIN 
                Savings S ON A.Id = S.AccountId
            GROUP BY 
                A.Name, A.Balance";

            DataTable accountsDataTable = _databaseHelper.GetData(accountsQuery);

            foreach (DataRow row in accountsDataTable.Rows)
            {
                AccountSummaryViewModel accountSummary = new AccountSummaryViewModel
                {
                    AccountName = row["AccountName"].ToString(),
                    Balance = (decimal)row["Balance"],
                    TotalIncomes = (decimal)row["TotalIncomes"],
                    TotalExpenses = (decimal)row["TotalExpenses"],
                    TotalSavings = (decimal)row["TotalSavings"]
                };
                accountSummaries.Add(accountSummary);
            }

            List<CategorySummaryViewModel> categorySummaries = new List<CategorySummaryViewModel>();

            string categoriesQuery = @"
        SELECT 
            C.Name AS CategoryName,
            COALESCE(SUM(I.Sum), 0) AS TotalIncomes,
            COALESCE(SUM(E.Sum), 0) AS TotalExpenses,
            COALESCE(SUM(S.Sum), 0) AS TotalSavings
        FROM 
            Categories C
        LEFT JOIN 
            Income I ON C.Id = I.CategoryId
        LEFT JOIN 
            Expenses E ON C.Id = E.CategoryId
        LEFT JOIN 
            Savings S ON C.Id = S.CategoryId
        GROUP BY 
            C.Name";

            DataTable categoriesDataTable = _databaseHelper.GetData(categoriesQuery);

            foreach (DataRow row in categoriesDataTable.Rows)
            {
                CategorySummaryViewModel categorySummary = new CategorySummaryViewModel
                {
                    CategoryName = row["CategoryName"].ToString(),
                    TotalIncomes = (decimal)row["TotalIncomes"],
                    TotalExpenses = (decimal)row["TotalExpenses"],
                    TotalSavings = (decimal)row["TotalSavings"]
                };
                categorySummaries.Add(categorySummary);
            }

            HomeViewModel viewModel = new HomeViewModel
            {
                AccountSummaries = accountSummaries,
                CategorySummaries = categorySummaries
            };

            return View(viewModel);
        }
    }
}

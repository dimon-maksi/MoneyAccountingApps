using Microsoft.AspNetCore.Mvc;
using MoneyAccountingAppADONET.Data;
using MoneyAccountingAppADONET.Models;
using System.Collections.Generic;
using System.Data;

namespace MoneyAccountingAppADONET.Controllers
{
    public class CategoryController : Controller
    {
        private readonly DatabaseHelper _databaseHelper;

        public CategoryController(DatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
        }

        public IActionResult Index()
        {
            string query = "SELECT * FROM Categories";
            DataTable dataTable = _databaseHelper.GetData(query);

            List<Category> categories = new List<Category>();
            foreach (DataRow row in dataTable.Rows)
            {
                categories.Add(new Category
                {
                    Id = (int)row["Id"],
                    Name = (string)row["Name"]
                });
            }

            return View(categories);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                string query = $"INSERT INTO Categories (Name) VALUES ('{category.Name}')";
                _databaseHelper.ExecuteCommand(query);
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            string query = $"DELETE FROM Categories WHERE Id = {id}";
            _databaseHelper.ExecuteCommand(query);
            return RedirectToAction(nameof(Index));
        }
    }
}

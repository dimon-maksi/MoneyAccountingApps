using Microsoft.AspNetCore.Mvc;
using MoneyAccountingAppEF.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyAccountingAppEF.Controllers
{
    public class HomeController : Controller
    {
        private readonly MoneyAccountingDbContext _context;

        public HomeController(MoneyAccountingDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var accounts = await _context.Accounts // Eager loading
                .Include(a => a.Expenses)
                .Include(a => a.Incomes)
                .Include(a => a.Savings)
                .ToListAsync();

            var categories = await _context.Categories.ToListAsync(); // Explicit loading

            foreach (var category in categories)
            {
                await _context.Entry(category)
                    .Collection(c => c.Expenses)
                    .Query()
                    .LoadAsync();

                await _context.Entry(category)
                    .Collection(c => c.Incomes)
                    .Query()
                    .LoadAsync();

                await _context.Entry(category)
                    .Collection(c => c.Savings)
                    .Query()
                    .LoadAsync();
            }

            return View(new Tuple<IEnumerable<MoneyAccountingAppEF.Models.Account>, IEnumerable<MoneyAccountingAppEF.Models.Category>>(accounts, categories));
        }
    }
}

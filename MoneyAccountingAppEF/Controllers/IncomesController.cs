using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MoneyAccountingAppEF.Data;
using MoneyAccountingAppEF.Models;

namespace MoneyAccountingAppEF.Controllers
{
    public class IncomesController : Controller
    {
        private readonly MoneyAccountingDbContext _context;
        private readonly AccountBalance accountBalance;

        public IncomesController(MoneyAccountingDbContext context)
        {
            _context = context;
            accountBalance = new AccountBalance(context);
        }

        // GET: Incomes
        public async Task<IActionResult> Index()
        {
            var moneyAccountingDbContext = _context.Incomes.Include(i => i.Account).Include(i => i.Category);
            return View(await moneyAccountingDbContext.ToListAsync());
        }

        // GET: Incomes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var income = await _context.Incomes
                .Include(i => i.Account)
                .Include(i => i.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (income == null)
            {
                return NotFound();
            }

            return View(income);
        }

        // GET: Incomes/Create
        public IActionResult Create()
        {
            ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Id");
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Sum,Date,AccountId,CategoryId,Source")] Income income)
        {
            if (ModelState.IsValid)
            {
                _context.Add(income);
                await _context.SaveChangesAsync();
                await accountBalance.Update(income.AccountId, income.Sum, true); // Add balance
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Id", income.AccountId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", income.CategoryId);
            return View(income);
        }


        // GET: Incomes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var income = await _context.Incomes.FindAsync(id);
            if (income == null)
            {
                return NotFound();
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Id", income.AccountId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", income.CategoryId);
            return View(income);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Sum,Date,AccountId,CategoryId,Source")] Income income)
        {
            if (id != income.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var oldIncome = await _context.Incomes.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);
                    if (oldIncome != null)
                    {
                        await accountBalance.Update(oldIncome.AccountId, oldIncome.Sum, false); // Subtract old balance
                    }

                    _context.Update(income);
                    await _context.SaveChangesAsync();

                    await accountBalance.Update(income.AccountId, income.Sum, true); // Add new balance
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IncomeExists(income.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Id", income.AccountId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", income.CategoryId);
            return View(income);
        }


        // GET: Incomes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var income = await _context.Incomes
                .Include(i => i.Account)
                .Include(i => i.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (income == null)
            {
                return NotFound();
            }

            return View(income);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var income = await _context.Incomes.FindAsync(id);
            if (income != null)
            {
                await accountBalance.Update(income.AccountId, income.Sum, false); // Subtract balance
                _context.Incomes.Remove(income);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }


        private bool IncomeExists(int id)
        {
            return _context.Incomes.Any(e => e.Id == id);
        }
    }
}

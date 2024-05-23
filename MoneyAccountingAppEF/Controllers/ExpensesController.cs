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
    public class ExpensesController : Controller
    {
        private readonly MoneyAccountingDbContext _context;
        private readonly AccountBalance accountBalance;

        public ExpensesController(MoneyAccountingDbContext context)
        {
            _context = context;
            accountBalance = new AccountBalance(context);
        }

        // GET: Expenses
        public async Task<IActionResult> Index()
        {
            var moneyAccountingDbContext = _context.Expenses.Include(e => e.Account).Include(e => e.Category);
            return View(await moneyAccountingDbContext.ToListAsync());
        }

        // GET: Expenses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var expense = await _context.Expenses
                .Include(e => e.Account)
                .Include(e => e.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (expense == null)
            {
                return NotFound();
            }

            return View(expense);
        }

        // GET: Expenses/Create
        public IActionResult Create()
        {
            ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Id");
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id");
            return View();
        }

        // POST: Expenses/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Sum,Date,AccountId,CategoryId,Type")] Expense expense)
        {
            if (ModelState.IsValid)
            {
                _context.Add(expense);
                await _context.SaveChangesAsync();
                await accountBalance.Update(expense.AccountId, expense.Sum, false); // Deduct balance
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Id", expense.AccountId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", expense.CategoryId);
            return View(expense);
        }


        // GET: Expenses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var expense = await _context.Expenses.FindAsync(id);
            if (expense == null)
            {
                return NotFound();
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Id", expense.AccountId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", expense.CategoryId);
            return View(expense);
        }

        // POST: Expenses/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Sum,Date,AccountId,CategoryId,Type")] Expense expense)
        {
            if (id != expense.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var oldExpense = await _context.Expenses.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
                    if (oldExpense != null)
                    {
                        await accountBalance.Update(oldExpense.AccountId, oldExpense.Sum, true); // Refund old balance
                    }

                    _context.Update(expense);
                    await _context.SaveChangesAsync();

                    await accountBalance.Update(expense.AccountId, expense.Sum, false); // Deduct new balance
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExpenseExists(expense.Id))
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
            ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Id", expense.AccountId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", expense.CategoryId);
            return View(expense);
        }


        // GET: Expenses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var expense = await _context.Expenses
                .Include(e => e.Account)
                .Include(e => e.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (expense == null)
            {
                return NotFound();
            }

            return View(expense);
        }

        // POST: Expenses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var expense = await _context.Expenses.FindAsync(id);
            if (expense != null)
            {
                await accountBalance.Update(expense.AccountId, expense.Sum, true); // Refund balance
                _context.Expenses.Remove(expense);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }


        private bool ExpenseExists(int id)
        {
            return _context.Expenses.Any(e => e.Id == id);
        }
    }
}

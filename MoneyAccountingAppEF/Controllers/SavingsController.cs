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
    public class SavingsController : Controller
    {
        private readonly MoneyAccountingDbContext _context;
        private readonly AccountBalance accountBalance;

        public SavingsController(MoneyAccountingDbContext context)
        {
            _context = context;
            accountBalance = new AccountBalance(context);
        }

        // GET: Savings
        public async Task<IActionResult> Index()
        {
            var moneyAccountingDbContext = _context.Savings.Include(s => s.Account).Include(s => s.Category);
            return View(await moneyAccountingDbContext.ToListAsync());
        }

        // GET: Savings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var saving = await _context.Savings
                .Include(s => s.Account)
                .Include(s => s.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (saving == null)
            {
                return NotFound();
            }

            return View(saving);
        }

        // GET: Savings/Create
        public IActionResult Create()
        {
            ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Id");
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Sum,Date,AccountId,CategoryId,Goal")] Saving saving)
        {
            if (ModelState.IsValid)
            {
                _context.Add(saving);
                await _context.SaveChangesAsync();
                await accountBalance.Update(saving.AccountId, saving.Sum, false); // Add balance
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Id", saving.AccountId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", saving.CategoryId);
            return View(saving);
        }


        // GET: Savings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var saving = await _context.Savings.FindAsync(id);
            if (saving == null)
            {
                return NotFound();
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Id", saving.AccountId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", saving.CategoryId);
            return View(saving);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Sum,Date,AccountId,CategoryId,Goal")] Saving saving)
        {
            if (id != saving.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var oldSaving = await _context.Savings.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);
                    if (oldSaving != null)
                    {
                        await accountBalance.Update(oldSaving.AccountId, oldSaving.Sum, true); // Subtract old balance
                    }

                    _context.Update(saving);
                    await _context.SaveChangesAsync();

                    await accountBalance.Update(saving.AccountId, saving.Sum, false); // Add new balance
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SavingExists(saving.Id))
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
            ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Id", saving.AccountId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", saving.CategoryId);
            return View(saving);
        }


        // GET: Savings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var saving = await _context.Savings
                .Include(s => s.Account)
                .Include(s => s.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (saving == null)
            {
                return NotFound();
            }

            return View(saving);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var saving = await _context.Savings.FindAsync(id);
            if (saving != null)
            {
                await accountBalance.Update(saving.AccountId, saving.Sum, true); // Subtract balance
                _context.Savings.Remove(saving);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }


        private bool SavingExists(int id)
        {
            return _context.Savings.Any(e => e.Id == id);
        }
    }
}

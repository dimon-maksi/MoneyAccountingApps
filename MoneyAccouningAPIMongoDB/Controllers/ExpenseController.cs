using Microsoft.AspNetCore.Mvc;
using MoneyAccountingAPIMongoDB.Models;
using MoneyAccountingAPIMongoDB.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoneyAccountingAPIMongoDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        private readonly ExpenseService _expenseService;

        public ExpenseController(ExpenseService expenseService)
        {
            _expenseService = expenseService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Expense>>> GetExpenses()
        {
            return await _expenseService.GetExpensesAsync();
        }

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Expense>> GetExpense(string id)
        {
            var expense = await _expenseService.GetExpenseAsync(id);

            if (expense == null)
            {
                return NotFound();
            }

            return expense;
        }

        [HttpPost]
        public async Task<ActionResult<Expense>> CreateExpense(Expense expense)
        {
            await _expenseService.CreateExpenseAsync(expense);
            return CreatedAtAction(nameof(GetExpense), new { id = expense.Id }, expense);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> UpdateExpense(string id, Expense expenseIn)
        {
            var expense = await _expenseService.GetExpenseAsync(id);

            if (expense == null)
            {
                return NotFound();
            }

            await _expenseService.UpdateExpenseAsync(id, expenseIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> DeleteExpense(string id)
        {
            var expense = await _expenseService.GetExpenseAsync(id);

            if (expense == null)
            {
                return NotFound();
            }

            await _expenseService.RemoveExpenseAsync(expense.Id);

            return NoContent();
        }
    }
}

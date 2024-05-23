using Microsoft.AspNetCore.Mvc;
using MoneyAccountingAPIMongoDB.Models;
using MoneyAccountingAPIMongoDB.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoneyAccountingAPIMongoDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncomeController : ControllerBase
    {
        private readonly IncomeService _incomeService;

        public IncomeController(IncomeService incomeService)
        {
            _incomeService = incomeService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Income>>> GetIncomes()
        {
            return await _incomeService.GetIncomesAsync();
        }

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Income>> GetIncome(string id)
        {
            var income = await _incomeService.GetIncomeAsync(id);

            if (income == null)
            {
                return NotFound();
            }

            return income;
        }

        [HttpPost]
        public async Task<ActionResult<Income>> CreateIncome(Income income)
        {
            await _incomeService.CreateIncomeAsync(income);
            return CreatedAtAction(nameof(GetIncome), new { id = income.Id }, income);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> UpdateIncome(string id, Income incomeIn)
        {
            var income = await _incomeService.GetIncomeAsync(id);

            if (income == null)
            {
                return NotFound();
            }

            await _incomeService.UpdateIncomeAsync(id, incomeIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> DeleteIncome(string id)
        {
            var income = await _incomeService.GetIncomeAsync(id);

            if (income == null)
            {
                return NotFound();
            }

            await _incomeService.RemoveIncomeAsync(income.Id);

            return NoContent();
        }
    }
}

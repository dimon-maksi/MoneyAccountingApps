using Microsoft.AspNetCore.Mvc;
using MoneyAccountingAPIMongoDB.Models;
using MoneyAccountingAPIMongoDB.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoneyAccountingAPIMongoDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SavingController : ControllerBase
    {
        private readonly SavingsService _savingsService;

        public SavingController(SavingsService savingsService)
        {
            _savingsService = savingsService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Saving>>> GetSavings()
        {
            return await _savingsService.GetSavingsAsync();
        }

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Saving>> GetSaving(string id)
        {
            var saving = await _savingsService.GetSavingAsync(id);

            if (saving == null)
            {
                return NotFound();
            }

            return saving;
        }

        [HttpPost]
        public async Task<ActionResult<Saving>> CreateSaving(Saving saving)
        {
            await _savingsService.CreateSavingAsync(saving);
            return CreatedAtAction(nameof(GetSaving), new { id = saving.Id }, saving);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> UpdateSaving(string id, Saving savingIn)
        {
            var saving = await _savingsService.GetSavingAsync(id);

            if (saving == null)
            {
                return NotFound();
            }

            await _savingsService.UpdateSavingAsync(id, savingIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> DeleteSaving(string id)
        {
            var saving = await _savingsService.GetSavingAsync(id);

            if (saving == null)
            {
                return NotFound();
            }

            await _savingsService.RemoveSavingAsync(saving.Id);

            return NoContent();
        }
    }
}

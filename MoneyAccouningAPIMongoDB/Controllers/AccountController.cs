using Microsoft.AspNetCore.Mvc;
using MoneyAccountingAPIMongoDB.Models;
using MoneyAccountingAPIMongoDB.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoneyAccountingAPIMongoDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly AccountService _accountService;

        public AccountController(AccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Account>>> GetAccounts()
        {
            return await _accountService.GetAccountsAsync();
        }

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Account>> GetAccount(string id)
        {
            var account = await _accountService.GetAccountAsync(id);

            if (account == null)
            {
                return NotFound();
            }

            return account;
        }

        [HttpPost]
        public async Task<ActionResult<Account>> CreateAccount(Account account)
        {
            await _accountService.CreateAccountAsync(account);
            return CreatedAtAction(nameof(GetAccount), new { id = account.Id }, account);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> UpdateAccount(string id, Account accountIn)
        {
            var account = await _accountService.GetAccountAsync(id);

            if (account == null)
            {
                return NotFound();
            }

            await _accountService.UpdateAccountAsync(id, accountIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> DeleteAccount(string id)
        {
            var account = await _accountService.GetAccountAsync(id);

            if (account == null)
            {
                return NotFound();
            }

            await _accountService.RemoveAccountAsync(account.Id);

            return NoContent();
        }
    }
}

using Microsoft.EntityFrameworkCore;

namespace MoneyAccountingAppEF.Data
{
    public class AccountBalance
    {
        private readonly MoneyAccountingDbContext _context;

        public AccountBalance(MoneyAccountingDbContext context)
        {
            _context = context;
        }
        public async Task Update(int accountId, decimal amount, bool isAddition)
        {
            var account = await _context.Accounts.FindAsync(accountId);
            if (account != null)
            {
                if (isAddition)
                {
                    account.Balance += amount;
                }
                else
                {
                    account.Balance -= amount;
                }
                _context.Update(account);
                await _context.SaveChangesAsync();
            }
        }
    }
}

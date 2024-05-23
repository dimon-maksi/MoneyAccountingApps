using Microsoft.Extensions.Options;
using MoneyAccountingAPIMongoDB.Models;
using MongoDB.Driver;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;

namespace MoneyAccountingAPIMongoDB.Services
{
    public class AccountService
    {
        private readonly IMongoCollection<Account> _accountCollection;

        public AccountService(IOptions<MongoDbSettings> mongoDbSettings)
        {
            var client = new MongoClient(mongoDbSettings.Value.ConnectionString);
            var database = client.GetDatabase(mongoDbSettings.Value.DatabaseName);
            _accountCollection = database.GetCollection<Account>(mongoDbSettings.Value.AccountsCollectionName);
        }

        public async Task<List<Account>> GetAccountsAsync() =>
            await _accountCollection.Find(account => true).ToListAsync();

        public async Task<Account> GetAccountAsync(string id) =>
            await _accountCollection.Find(account => account.Id == id).FirstOrDefaultAsync();

        public async Task CreateAccountAsync(Account account) =>
            await _accountCollection.InsertOneAsync(account);

        public async Task UpdateAccountAsync(string id, Account accountIn) =>
            await _accountCollection.ReplaceOneAsync(account => account.Id == id, accountIn);

        public async Task RemoveAccountAsync(string id) =>
            await _accountCollection.DeleteOneAsync(account => account.Id == id);
    }
}
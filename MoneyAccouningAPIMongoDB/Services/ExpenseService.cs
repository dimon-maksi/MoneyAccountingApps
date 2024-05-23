using Microsoft.Extensions.Options;
using MoneyAccountingAPIMongoDB.Models;
using MongoDB.Driver;

namespace MoneyAccountingAPIMongoDB.Services
{
    public class ExpenseService
    {
        private readonly IMongoCollection<Expense> _expenseCollection;

        public ExpenseService(IOptions<MongoDbSettings> mongoDbSettings)
        {
            var client = new MongoClient(mongoDbSettings.Value.ConnectionString);
            var database = client.GetDatabase(mongoDbSettings.Value.DatabaseName);
            _expenseCollection = database.GetCollection<Expense>(mongoDbSettings.Value.ExpensesCollectionName);
        }

        public async Task<List<Expense>> GetExpensesAsync() =>
            await _expenseCollection.Find(expense => true).ToListAsync();

        public async Task<Expense> GetExpenseAsync(string id) =>
            await _expenseCollection.Find(expense => expense.Id == id).FirstOrDefaultAsync();

        public async Task CreateExpenseAsync(Expense expense) =>
            await _expenseCollection.InsertOneAsync(expense);

        public async Task UpdateExpenseAsync(string id, Expense expenseIn) =>
            await _expenseCollection.ReplaceOneAsync(expense => expense.Id == id, expenseIn);

        public async Task RemoveExpenseAsync(string id) =>
            await _expenseCollection.DeleteOneAsync(expense => expense.Id == id);
    }
}
using Microsoft.Extensions.Options;
using MoneyAccountingAPIMongoDB.Models;
using MongoDB.Driver;

namespace MoneyAccountingAPIMongoDB.Services
{
    public class IncomeService
    {
        private readonly IMongoCollection<Income> _incomeCollection;

        public IncomeService(IOptions<MongoDbSettings> mongoDbSettings)
        {
            var client = new MongoClient(mongoDbSettings.Value.ConnectionString);
            var database = client.GetDatabase(mongoDbSettings.Value.DatabaseName);
            _incomeCollection = database.GetCollection<Income>(mongoDbSettings.Value.IncomeCollectionName);
        }

        public async Task<List<Income>> GetIncomesAsync() =>
            await _incomeCollection.Find(income => true).ToListAsync();

        public async Task<Income> GetIncomeAsync(string id) =>
            await _incomeCollection.Find(income => income.Id == id).FirstOrDefaultAsync();

        public async Task CreateIncomeAsync(Income income) =>
            await _incomeCollection.InsertOneAsync(income);

        public async Task UpdateIncomeAsync(string id, Income incomeIn) =>
            await _incomeCollection.ReplaceOneAsync(income => income.Id == id, incomeIn);

        public async Task RemoveIncomeAsync(string id) =>
            await _incomeCollection.DeleteOneAsync(income => income.Id == id);
    }
}
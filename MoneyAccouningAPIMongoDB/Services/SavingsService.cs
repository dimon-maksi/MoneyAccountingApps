using Microsoft.Extensions.Options;
using MoneyAccountingAPIMongoDB.Models;
using MongoDB.Driver;

namespace MoneyAccountingAPIMongoDB.Services
{
    public class SavingsService
    {
        private readonly IMongoCollection<Saving> _savingCollection;

        public SavingsService(IOptions<MongoDbSettings> mongoDbSettings)
        {
            var client = new MongoClient(mongoDbSettings.Value.ConnectionString);
            var database = client.GetDatabase(mongoDbSettings.Value.DatabaseName);
            _savingCollection = database.GetCollection<Saving>(mongoDbSettings.Value.SavingsCollectionName);
        }

        public async Task<List<Saving>> GetSavingsAsync() =>
            await _savingCollection.Find(saving => true).ToListAsync();

        public async Task<Saving> GetSavingAsync(string id) =>
            await _savingCollection.Find(saving => saving.Id == id).FirstOrDefaultAsync();

        public async Task CreateSavingAsync(Saving saving) =>
            await _savingCollection.InsertOneAsync(saving);

        public async Task UpdateSavingAsync(string id, Saving savingIn) =>
            await _savingCollection.ReplaceOneAsync(saving => saving.Id == id, savingIn);

        public async Task RemoveSavingAsync(string id) =>
            await _savingCollection.DeleteOneAsync(saving => saving.Id == id);
    }
}
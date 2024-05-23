using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MoneyAccountingAPIMongoDB.Models;
using MongoDB.Driver;

namespace MoneyAccountingAPIMongoDB.Services
{
    public class CategoryService
    {
        private readonly IMongoCollection<Category> _categoryCollection;
        private readonly ILogger<CategoryService> _logger;

        public CategoryService(
            IOptions<MongoDbSettings> mongoDbSettings,
            ILogger<CategoryService> logger)
        {
            _logger = logger;

            try
            {
                var client = new MongoClient(mongoDbSettings.Value.ConnectionString);
                var database = client.GetDatabase(mongoDbSettings.Value.DatabaseName);
                _categoryCollection = database.GetCollection<Category>(mongoDbSettings.Value.CategoriesCollectionName);
                _logger.LogInformation("Successfully connected to MongoDB.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error connecting to MongoDB.");
                throw;
            }
        }

        public async Task<List<Category>> GetCategoriesAsync() =>
            await _categoryCollection.Find(category => true).ToListAsync();

        public async Task<Category> GetCategoryAsync(string id) =>
            await _categoryCollection.Find(category => category.Id == id).FirstOrDefaultAsync();

        public async Task CreateCategoryAsync(Category category) =>
            await _categoryCollection.InsertOneAsync(category);

        public async Task UpdateCategoryAsync(string id, Category categoryIn) =>
            await _categoryCollection.ReplaceOneAsync(category => category.Id == id, categoryIn);

        public async Task RemoveCategoryAsync(string id) =>
            await _categoryCollection.DeleteOneAsync(category => category.Id == id);
    }
}

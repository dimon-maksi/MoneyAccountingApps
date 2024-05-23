namespace MoneyAccountingAPIMongoDB.Models
{
    public class MongoDbSettings
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string IncomeCollectionName { get; set; } = null!;
        public string ExpensesCollectionName { get; set; } = null!;
        public string SavingsCollectionName { get; set; } = null!;
        public string AccountsCollectionName { get; set; } = null!;
        public string CategoriesCollectionName { get; set; } = null!;
    }
}

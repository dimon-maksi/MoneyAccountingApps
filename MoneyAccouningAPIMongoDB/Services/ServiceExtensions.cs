namespace MoneyAccountingAPIMongoDB.Services
{
    public static class ServiceExtensions
    {
        public static void ConfigureMongoDbServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddSingleton<IncomeService>();
            services.AddSingleton<ExpenseService>();
            services.AddSingleton<SavingsService>();
            services.AddSingleton<AccountService>();
            services.AddSingleton<CategoryService>();
        }
    }
}

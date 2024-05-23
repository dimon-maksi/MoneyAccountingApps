using MoneyAccountingAPIMongoDB.Models;
using MoneyAccountingAPIMongoDB.Services;
using MoneyAccountingAPIMongoDB.Controllers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));

builder.Services.AddSingleton<IncomeService>();
builder.Services.AddSingleton<ExpenseService>();
builder.Services.AddSingleton<SavingsService>();
builder.Services.AddSingleton<AccountService>();
builder.Services.AddSingleton<CategoryService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

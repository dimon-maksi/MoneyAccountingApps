namespace MoneyAccountingAppADONET.Models
{
    public class CategorySummaryViewModel
    {
        public string CategoryName { get; set; }
        public decimal TotalIncomes { get; set; }
        public decimal TotalExpenses { get; set; }
        public decimal TotalSavings { get; set; }
    }
}

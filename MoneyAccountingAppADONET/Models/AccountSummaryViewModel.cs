namespace MoneyAccountingAppADONET.Models
{
    public class AccountSummaryViewModel
    {
        public string AccountName { get; set; }
        public decimal Balance { get; set; }
        public decimal TotalIncomes { get; set; }
        public decimal TotalExpenses { get; set; }
        public decimal TotalSavings { get; set; }
    }

}

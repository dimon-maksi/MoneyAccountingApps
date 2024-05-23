using System.ComponentModel.DataAnnotations;

namespace MoneyAccountingAppADONET.Models
{
    public class Expense
    {
        public int Id { get; set; }
        [Required]
        public decimal Sum { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public int AccountId { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        [StringLength(255)]
        public string Type { get; set; }
    }
}

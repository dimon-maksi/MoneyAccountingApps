using System.ComponentModel.DataAnnotations;

namespace MoneyAccountingAppADONET.Models
{
    public class Account
    {
        public int Id { get; set; }
        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        [Required]
        public decimal Balance { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace MoneyAccountingAppEF.Models;

public partial class Saving
{
    public int Id { get; set; }

    public decimal Sum { get; set; }

    public DateTime Date { get; set; }

    public int AccountId { get; set; }

    public int CategoryId { get; set; }

    public string Goal { get; set; } = null!;

    public virtual Account Account { get; set; } = null!;

    public virtual Category Category { get; set; } = null!;
}

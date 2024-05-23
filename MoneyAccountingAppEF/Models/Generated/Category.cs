using System;
using System.Collections.Generic;

namespace MoneyAccountingAppEF.Models;

public partial class Category
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Expense> Expenses { get; set; } = new List<Expense>();

    public virtual ICollection<Income> Incomes { get; set; } = new List<Income>();

    public virtual ICollection<Saving> Savings { get; set; } = new List<Saving>();
}

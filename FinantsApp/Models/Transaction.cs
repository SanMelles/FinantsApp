namespace FinantsApp.Models;

using SQLite;

public class Transaction
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public decimal Amount { get; set; }
    public TransactionReason Reason { get; set; }
    public string? Description { get; set; }
    public DateTime Date { get; set; }

    public bool IsIncome => Amount > 0;
}

public enum TransactionReason
{
    // Income Reasons
    Salary,
    Bonus,
    Gift,
    InvestmentReturn,

    // Expense Reasons
    LivingCosts,
    Entertainment,
    Transportation,

    Other
}

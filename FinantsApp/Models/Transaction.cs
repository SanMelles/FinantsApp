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

    public bool IsIncome { 
        get 
        {
            return Reason switch
            {
                TransactionReason.Salary 
                    or TransactionReason.Bonus 
                    or TransactionReason.Gift 
                    or TransactionReason.InvestmentReturn => true,

                _ => false,
            };
        } 
    }
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

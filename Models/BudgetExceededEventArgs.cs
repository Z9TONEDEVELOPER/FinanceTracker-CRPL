using FinanceTracker.CLI.Enums;

namespace FinanceTracker.CLI.Models;

public class BudgetExceededEventArgs : EventArgs
{
    public BudgetExceededEventArgs(CategoryType category, decimal limit, decimal spent)
    {
        Category = category;
        Limit = limit;
        Spent = spent;
        OverAmount = spent - limit;
    }
    
    public CategoryType Category { get; }
    public decimal Limit { get; }
    public decimal Spent { get; }
    public decimal OverAmount { get; }
}
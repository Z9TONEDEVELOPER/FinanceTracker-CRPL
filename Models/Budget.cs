using System.Text.Json.Serialization;
using FinanceTracker.CLI.Enums;
namespace FinanceTracker.CLI.Models;

public class Budget
{
    [JsonConstructor]
    public Budget(CategoryType category, decimal limit, decimal spent)
    {
        Category = category;
        Limit = limit;
        Spent = spent;
    }
    public Budget(CategoryType category, decimal limit)
    {
        Category = category;
        Limit = limit;
        Spent = 0;
    }

    public CategoryType Category { get; }
    public decimal Limit { get; }
    [JsonInclude]
    public decimal Spent { get; private set; }

    public event EventHandler<BudgetExceededEventArgs>? OnBudgetExceeded;

    public void AddExpense(decimal amount)
    {
        Spent += amount;
        if (Spent > Limit)
        {
            var args = new BudgetExceededEventArgs(Category,Limit,Spent);
            OnBudgetExceeded?.Invoke(this, args);
        }
    }
}
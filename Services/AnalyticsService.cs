using FinanceTracker.CLI.Enums;
using FinanceTracker.CLI.Models;

namespace FinanceTracker.CLI.Services;

public class AnalyticsService
{
    public decimal CalculateBalance(IReadOnlyList<Transaction> transactions)
    {
        decimal incomeAmount = transactions.Where(t => t.Type == TransactionType.Income).Sum(t => t.Amount);
        decimal expenseAmount = transactions.Where(t => t.Type == TransactionType.Expense).Sum(t => t.Amount);
        return incomeAmount - expenseAmount;
    }

    public Dictionary<CategoryType, decimal> GetTotalByCategory(IReadOnlyList<Transaction> transactions) //TODO: разделить на категории и поправить ui
    {
        return transactions.GroupBy(t => t.Category).ToDictionary(g => g.Key, g => g.Sum(t => t.Amount));
    }

    public IReadOnlyList<Transaction> GetTopExpenses(IReadOnlyList<Transaction> transactions, int count)
    {
        return transactions.Where(t => t.Type == TransactionType.Expense).OrderByDescending(t => t.Amount).Take(count).ToList();
    }

    public IReadOnlyList<Transaction> GetTransactionsByPeriod(IReadOnlyList<Transaction> transactions, DateTime from,
        DateTime to)
    {
        return transactions.Where(t => t.Date >= from && t.Date <= to).ToList();
    }

    public decimal GetAverageTransactionAmount(IReadOnlyList<Transaction> transactions)
    {
        var exp = transactions.Where(t => t.Type == TransactionType.Expense);
        if (exp.Any())
        {
            return 0;
        }
        else
        {
            return exp.Average(t => t.Amount);
        }
    }

    public CategoryType? GetMostFrequentCategory(IReadOnlyList<Transaction> transactions)
    {
        return transactions.GroupBy(t => t.Category).OrderByDescending(g => g.Count()).FirstOrDefault()?.Key;
    }
}
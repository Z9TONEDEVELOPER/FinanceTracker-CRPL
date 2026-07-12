using FinanceTracker.CLI.Enums;
using FinanceTracker.CLI.Models;

namespace FinanceTracker.CLI.Contracts;

public interface IAnalyticsService
{
    decimal CalculateBalance(IReadOnlyList<Transaction> transactions);
    Dictionary<CategoryType, decimal> GetTotalByCategory(IReadOnlyList<Transaction> transactions);
    IReadOnlyList<Transaction> GetTopExpenses(IReadOnlyList<Transaction> transactions, int count);

    IReadOnlyList<Transaction> GetTransactionsByPeriod(IReadOnlyList<Transaction> transactions, DateTime from,
        DateTime to);

    decimal GetAverageTransactionAmount(IReadOnlyList<Transaction> transactions);
    CategoryType? GetMostFrequentCategory(IReadOnlyList<Transaction> transactions);
}
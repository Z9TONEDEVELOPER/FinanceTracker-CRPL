using FinanceTracker.CLI.Enums;
using FinanceTracker.CLI.Models;

namespace FinanceTracker.CLI.Contracts;

public interface IStorage
{
    Task SaveTransactions(IReadOnlyList<Transaction> transactions);
    Task<List<Transaction>> LoadTransactions();
    Task SaveBudgets(Dictionary<CategoryType, Budget> budgets);
    Task<Dictionary<CategoryType, Budget>> LoadBudgets();
}
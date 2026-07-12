using FinanceTracker.CLI.Enums;
using FinanceTracker.CLI.Models;

namespace FinanceTracker.CLI.Contracts;

public interface IFinanceManager
{
    event EventHandler<BudgetExceededEventArgs>? OnBudgetExceeded;
    void AddTransactions(Transaction transactionNew);
    bool RemoveTransaction(int id, out Transaction? removedTran);
    IReadOnlyList<Transaction> GetAllTransaction();
    void SetBudget(CategoryType category, decimal limit);
    (decimal limit, decimal spent, decimal remaining) CheckBudget(CategoryType category);
    Dictionary<CategoryType, Budget> GetAllBudgets(); //return _budgets
    Task LoadFromStorageTransactions();
    Task LoadFromStorageBudgets();
}
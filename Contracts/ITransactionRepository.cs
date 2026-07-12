using FinanceTracker.CLI.Models;

namespace FinanceTracker.CLI.Contracts;

public interface ITransactionRepository
{
    Task AddAsync(Transaction transaction);
    Task<bool> RemoveAsync(int id);
    Task<IReadOnlyList<Transaction>> GetAllAsync();
    Task<Transaction?> GetByIdAsync(int id);
}
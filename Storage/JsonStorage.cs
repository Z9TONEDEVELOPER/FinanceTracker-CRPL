using FinanceTracker.CLI.Enums;
using FinanceTracker.CLI.Models;
using System.Text.Json;
using FinanceTracker.CLI.UI;

namespace FinanceTracker.CLI.Storage;

public class JsonStorage
{
    private static readonly string FilePathTransactions = Path.Combine(AppContext.BaseDirectory, "transactions.json");
    private static readonly string FilePathBudget = Path.Combine(AppContext.BaseDirectory, "budgets.json");
    public static async Task SaveTransactions(IReadOnlyList<Transaction> transactions)
    {
        await using FileStream fs = new FileStream(FilePathTransactions, FileMode.Create, FileAccess.Write);
        await JsonSerializer.SerializeAsync(fs, transactions);
    }

    public static async Task<List<Transaction>> LoadTransactions()
    {
        if (!File.Exists(FilePathTransactions)) return new List<Transaction>();
        await using FileStream fs = new FileStream(FilePathTransactions, FileMode.Open, FileAccess.Read);
        return await JsonSerializer.DeserializeAsync<List<Transaction>>(fs) ?? new List<Transaction>();
    }

    public static async Task SaveBudgets(Dictionary<CategoryType, Budget> budgets)
    {
        await using FileStream fs = new FileStream(FilePathBudget, FileMode.Create, FileAccess.Write);
        await JsonSerializer.SerializeAsync(fs, budgets);
    }

    public static async Task<Dictionary<CategoryType, Budget>> LoadBudgets()
    {
            if (!File.Exists(FilePathBudget)) return new Dictionary<CategoryType, Budget>();
            await using FileStream fs = new FileStream(FilePathBudget, FileMode.Open, FileAccess.Read);
            return await JsonSerializer.DeserializeAsync<Dictionary<CategoryType, Budget>>(fs) ??
                   new Dictionary<CategoryType, Budget>();
        
    }
}
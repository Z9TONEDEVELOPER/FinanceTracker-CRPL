using FinanceTracker.CLI.Models;
using FinanceTracker.CLI.Services;
using FinanceTracker.CLI.Enums;
using FinanceTracker.CLI.Exceptions;
using FinanceTracker.CLI.Storage;
using FinanceTracker.CLI.UI;

namespace FinanceTracker.CLI;

class Program
{
    static async Task Main(string[] args)
    {
        FinanceManager manager = new FinanceManager();
        manager.OnBudgetExceeded += (sender, e) =>
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"WARNING: Budget exceeded for {e.Category}!");
            Console.WriteLine($"Limit: {e.Limit}, Spent: {e.Spent}, Over by: {e.OverAmount}");
            Console.ResetColor();
        }; 
        FinanceUI ui = new FinanceUI();
        AnalyticsService analyticsService = new AnalyticsService();
        bool isRunning = true;
        await manager.LoadFromStorageBudgets();
        await manager.LoadFromStorageTransactions();
        while (isRunning)
        {
            ui.ShowMainMenu();
            int choice = ui.ReadUserChoice();
            switch (choice)
            {
                case 1:
                    decimal amount = ui.ReadAmount();
                    TransactionType transaction = ui.ReadTransaction();
                    CategoryType category = ui.ReadCategory();
                    string? description = ui.ReadDescription();
                    DateTime? time = ui.GetDate();
                    try
                    {
                        Transaction newTransaction = new Transaction(amount, transaction, category, description, time);
                        manager.AddTransactions(newTransaction);
                        ui.ShowMessage("Transaction added");
                        await JsonStorage.SaveBudgets(manager.GetAllBudgets());
                        await JsonStorage.SaveTransactions(manager.GetAllTransaction());
                    }
                    catch (TransactionValidationException ex)
                    {
                        ui.ShowError(ex.Message);
                    }
                    break;
                case 2:
                    ui.ShowAllTransaction(manager.GetAllTransaction());
                    int choiceTransaction = ui.ReadTransactionId();
                    bool removed = manager.RemoveTransaction(choiceTransaction, out var removedTran);
                    if (removed)
                    {
                        ui.ShowMessage($"The transaction {ui.DeletedTransactionPrint(removedTran)} was successfully deleted");
                        await JsonStorage.SaveBudgets(manager.GetAllBudgets());
                        await JsonStorage.SaveTransactions(manager.GetAllTransaction());
                    }
                    else
                    {
                        ui.ShowMessage("Transaction not found");
                    }
                    break;
                case 3:
                    var allTransaction = manager.GetAllTransaction();
                    ui.ShowAllTransaction(allTransaction);
                    break;
                case 4:
                    ui.ShowMessage($"Balance: {analyticsService.CalculateBalance(manager.GetAllTransaction())}");
                    break;
                case 5:
                    var stat = analyticsService.GetTotalByCategory(manager.GetAllTransaction());
                    ui.ShowMessage("=== Statistics by Category ===");
                    foreach (var kvp in stat)
                    {
                        ui.ShowMessage($"{kvp.Key}: {kvp.Value}");
                    }
                    break;
                case 6:
                    var fExpense = analyticsService.GetTopExpenses(manager.GetAllTransaction(), 5);
                    ui.ShowMessage("=== Top 5 Expenses ===");
                    foreach (var t in fExpense)
                    {
                        ui.ShowMessage($"ID: {t.Id} | Amount: {t.Amount} | Category: {t.Category}"); 
                    }
                    break;
                case 7:
                    var (from, to) = ui.GetDatePeriod();
                    ui.ShowMessage($"Transactions for the period: {analyticsService.GetTransactionsByPeriod(manager.GetAllTransaction(), from, to)}");
                    break;
                case 8:
                    ui.ShowMessage($"Average check: {analyticsService.GetAverageTransactionAmount(manager.GetAllTransaction())}");
                    break;
                case 9:
                    ui.ShowMessage($"The most popular category: {analyticsService.GetMostFrequentCategory(manager.GetAllTransaction())}");
                    break;
                case 10:
                    var (categoryBudget, limit) = ui.SetUserBudget();
                    manager.SetBudget(categoryBudget,limit);
                    ui.ShowMessage("The budget has been set");
                    break;
                case 11:
                    var (limitB, spent, rem) = manager.CheckBudget(ui.CheckUserBudget());
                    ui.ShowMessage($"Limit: {limitB}| Spent: {spent}| Remaining: {rem}");
                    break;
                case 0:
                    await JsonStorage.SaveBudgets(manager.GetAllBudgets());
                    await JsonStorage.SaveTransactions(manager.GetAllTransaction());
                    isRunning = false;
                    break;
            }
        }
        Console.WriteLine("\nPress any key...");
        Console.ReadKey();
        Console.Clear();
    }
}
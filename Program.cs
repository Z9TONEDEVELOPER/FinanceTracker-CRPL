using FinanceTracker.CLI.Models;
using FinanceTracker.CLI.Services;
using FinanceTracker.CLI.Enums;
using FinanceTracker.CLI.Exceptions;
using FinanceTracker.CLI.UI;

namespace FinanceTracker.CLI;

class Program
{
    static void Main(string[] args)
    {
        FinanceManager manager = new FinanceManager();
        FinanceUI ui = new FinanceUI();
        AnalyticsService analyticsService = new AnalyticsService();
        bool isRunning = true;
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
                        manager.AddTransaction(newTransaction);
                        ui.ShowMessage("Transaction added");
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
                case 0:
                    isRunning = false;
                    break;
            }
        }
        Console.WriteLine("\nPress any key...");
        Console.ReadKey();
        Console.Clear();
    }
}
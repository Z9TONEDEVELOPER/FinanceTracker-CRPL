using FinanceTracker.CLI.Enums;
using FinanceTracker.CLI.Exceptions;
using FinanceTracker.CLI.Models;
using FinanceTracker.CLI.Services;

namespace FinanceTracker.CLI.UI;

public class FinanceUI
{
    public void ShowMainMenu()
    {
        Console.WriteLine("==========FinanceTracker - your personal money tracker==========");
        Console.WriteLine("Menu (Enter number): ");
        Console.WriteLine("1. Add Transaction");
        Console.WriteLine("2. Remove Transaction");
        Console.WriteLine("3. Show All Transactions");
        Console.WriteLine("0. Exit");
        Console.WriteLine("==========Analytics==========");
        Console.WriteLine("4. Show Balance");
        Console.WriteLine("5. Show statistics by category");
        Console.WriteLine("6. Show the top 5 expenses");
        Console.WriteLine("7. Show transactions for the period");
        Console.WriteLine("8. Show average check");
        Console.WriteLine("9. Show the most common category");
        Console.WriteLine("==========Budget==========");
        Console.WriteLine("10. Set Budget");
        Console.WriteLine("11. Check Budget");
    }

    public void ShowAllTransaction(IReadOnlyList<Transaction> transactions)
    {
        Console.WriteLine("==========List of All Transactions==========");
        foreach (var t in transactions)
        {
            Console.WriteLine($"ID: {t.Id}|Amount: {t.Amount}|Type: {t.Type}|Category: {t.Category}|Description: {t.Description}|Date: {t.Date:dd.MM.yyyy HH:mm}");
        }
    }

    public void ShowMessage(string message)
    {
        Console.WriteLine(message);
    }

    public void ShowError(string error)
    {
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine(error);
        Console.ResetColor();
    }

    public int ReadUserChoice()
    {
        while (true)
        {
            Console.WriteLine("Enter the choice number: ");
            string? res = Console.ReadLine();
            if (int.TryParse(res, out int choice))
            {
                return choice;
            }
            else
            {
                ShowError("Invalid choice");
            }
        }
        
    }

    public string? ReadUserString()
    {
        string? res = Console.ReadLine();
        return res;
    }

    public decimal ReadAmount()
    {
        while (true)
        {
            Console.Write("Enter the amount: ");
            string? input = ReadUserString();
            if (decimal.TryParse(input, out decimal amount))
            {
                var (isValid, error) = TransactionValidator.ValidateAmount(amount);
                if (isValid)
                {
                    return amount;
                }
                else
                {
                    ShowError(error);
                }
            }
            else
            {
                ShowError("Invalid format, Try again");
            }
        }
    }

    public TransactionType ReadTransaction()
    {
        while (true)
        {
            Console.WriteLine("Select a transaction type (Enter number)");
            Console.WriteLine("1. Income");
            Console.WriteLine("2. Expense");
            int choice = ReadUserChoice();
            switch (choice)
            {
                case 1: 
                    return TransactionType.Income;
                case 2:
                    return TransactionType.Expense;
                default:
                    ShowError("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    public CategoryType ReadCategory()
    {
        while (true)
        {
            Console.WriteLine("Select a category (Enter number)");
            Console.WriteLine("1. Food");
            Console.WriteLine("2. Transport");
            Console.WriteLine("3. Housing");
            Console.WriteLine("4. Entertainment");
            Console.WriteLine("5. Salary");
            Console.WriteLine("6. Bills");
            Console.WriteLine("7. Healthcare");
            Console.WriteLine("8. Savings");
            Console.WriteLine("9. Other");
            int choice = ReadUserChoice();
            switch (choice)
            {
                case 1:
                    return CategoryType.Food;
                case 2:
                    return CategoryType.Transport;
                case 3:
                    return CategoryType.Housing;
                case 4:
                    return CategoryType.Entertainment;
                case 5:
                    return CategoryType.Salary;
                case 6:
                    return CategoryType.Bills;
                case 7:
                    return CategoryType.Healthcare;
                case 8:
                    return CategoryType.Savings;
                case 9:
                    return CategoryType.Other;
                default:
                    ShowError("Invalid category.");
                    break;
            }
        }
    }

    public string? ReadDescription()
    {
        while (true)
        {
            Console.WriteLine("Enter a description: ");
            string? input = ReadUserString();
                var (isValid, error) = TransactionValidator.ValidateDescription(input);
                if (isValid)
                {
                    return input;
                }
                else
                {
                    ShowError(error);
                }
        }
    }

    public int ReadTransactionId()
    {
        Console.WriteLine("Enter the transaction number you want to delete: ");
        int result = ReadUserChoice();
        return result;
    }
    
    public string? DeletedTransactionPrint(Transaction? transaction)
    {
        if (transaction == null)
        {
            return null;
        }
    
        return $"ID: {transaction.Id}|Amount: {transaction.Amount}|Type: {transaction.Type}|Category: {transaction.Category}|Description: {transaction.Description}|Date: {transaction.Date:dd.MM.yyyy HH:mm}";
    }

    public DateTime? GetDate()
    {
        while (true)
        {
            Console.WriteLine("Enter date (dd.MM.yyyy HH:mm): ");
            string? input = ReadUserString();
            if (DateTime.TryParse(input, out DateTime date))
            {
                var (isValid, error) = TransactionValidator.ValidateDate(date);
                if (isValid)
                {
                    return date;
                }
                else
                {
                    ShowError(error);
                }
            }
            else
            {
                ShowError("Invalid Format, Try again");
            }
        }
    }
    
    public (DateTime from,DateTime to) GetDatePeriod()
    {
        DateTime from = ValidDate("Enter date from: ");
        DateTime to = ValidDate("Enter date to: ");
        return (from, to);
    }

    private DateTime ValidDate(string text)
    {
        while (true)
        {
            Console.WriteLine(text);
            string? input = Console.ReadLine();
            if (DateTime.TryParse(input, out var dateTime))
            {
                return dateTime;
            }
            else
            {
                ShowError("Invalid format");
            }
        }
    }

    public (CategoryType, decimal) SetUserBudget()
    {
        CategoryType category = ReadCategory();
        decimal limit = ReadAmount();
        return (category, limit);
    }

    public CategoryType CheckUserBudget()
    {
        CategoryType category = ReadCategory();
        return category;
    }
}
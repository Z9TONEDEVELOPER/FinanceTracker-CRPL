using FinanceTracker.CLI.Enums;
using System.Text.Json.Serialization;
namespace FinanceTracker.CLI.Models;

public class Transaction
{
    [JsonConstructor]
    public Transaction(
        int id, 
        decimal amount, 
        TransactionType type, 
        CategoryType category, 
        string? description, 
        DateTime date)
    {
        Id = id;
        Amount = amount;
        Type = type;
        Category = category;
        Description = description;
        Date = date;
    }
    public Transaction(
        decimal amount,
        TransactionType type,
        CategoryType category,
        string? description,
        DateTime? time)
    {
        Id = _nextId++;
        Amount = amount;
        Type = type;
        Category = category;
        Description = description;
        Date = time ?? DateTime.Now;
    }
    private static int _nextId = 1;
    public int Id
    {
        get;
    }

    public decimal Amount
    {
        get;
        init;
    }

    public TransactionType Type
    {
        get;
        init;
    }

    public CategoryType Category
    {
        get;
        init;
    }

    public string? Description
    {
        get;
        init;
    }

    public DateTime Date
    {
        get;
        init;
    }
    
    public static void SetNextId(int value)
    {
        _nextId = value;
    }
    
}
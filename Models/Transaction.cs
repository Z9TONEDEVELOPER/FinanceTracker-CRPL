using FinanceTracker.CLI.Enums;
namespace FinanceTracker.CLI.Models;

public class Transaction
{
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
    
    
}
using FinanceTracker.CLI.Contracts;
using FinanceTracker.CLI.Enums;
using FinanceTracker.CLI.Exceptions;
using FinanceTracker.CLI.Models;
using FinanceTracker.CLI.Storage;

namespace FinanceTracker.CLI.Services;

public class FinanceManager : IFinanceManager
{
    public FinanceManager(IStorage storage)
    {
        _storage = storage;
        _transactions = new List<Transaction>();
        _budgets = new Dictionary<CategoryType, Budget>();
    }
    private readonly IStorage _storage;
    private readonly List<Transaction> _transactions;
    private Dictionary<CategoryType, Budget> _budgets;
    public void AddTransactions(Transaction transactionNew)
    {
        var (isValidAmount, errorAmount) = TransactionValidator.ValidateAmount(transactionNew.Amount);
        if (!isValidAmount)
        {
            throw new TransactionValidationException(errorAmount);
        }
        var (isValidDate, errorDate) = TransactionValidator.ValidateDate(transactionNew.Date);
        if (!isValidDate)
        {
            throw new TransactionValidationException(errorDate);
        }
        var (isValidDesc, errorDesc) = TransactionValidator.ValidateDescription(transactionNew.Description);
        if (!isValidDesc)
        {
            throw new TransactionValidationException(errorDesc);
        }
        
        _transactions.Add(transactionNew);

        if (transactionNew.Type == TransactionType.Expense)
        {
            if (_budgets.TryGetValue(transactionNew.Category, out Budget? budget))
            {
                budget.AddExpense(transactionNew.Amount);
            }
        }
        
    }
    
    private void OnBudgetExceededHandler(object? sender, BudgetExceededEventArgs e)
    {
        OnBudgetExceeded?.Invoke(this, e);
    }
    
    public event EventHandler<BudgetExceededEventArgs>? OnBudgetExceeded;
    
    public bool RemoveTransaction(int id, out Transaction? removedTran)
    {
        int idT = _transactions.FindIndex(t => t.Id == id);
        if (idT >= 0)
        {
            removedTran = _transactions[idT];
            _transactions.RemoveAt(idT);
            return true;
        }
        else
        {
            removedTran = null;
            return false;
        }
    }

    public IReadOnlyList<Transaction> GetAllTransaction()
    {
        IReadOnlyList<Transaction> privateList = _transactions.AsReadOnly();
        return privateList;
    }

    public void SetBudget(CategoryType category, decimal limit)
    {
        decimal currentSpent = 0;
        
        if (_budgets.TryGetValue(category, out var oldBudget))
        {
            currentSpent = oldBudget.Spent;
            oldBudget.OnBudgetExceeded -= OnBudgetExceededHandler;
        }
        
        var newBudget = new Budget(category, limit, currentSpent);
        newBudget.OnBudgetExceeded += OnBudgetExceededHandler;
        _budgets[category] = newBudget;
    }

    public (decimal limit, decimal spent, decimal remaining) CheckBudget(CategoryType category)
    {
        if (_budgets.TryGetValue(category, out var budget))
        {
            return (budget.Limit, budget.Spent, budget.Limit - budget.Spent);
        }
        else
        {
            return (0, 0, 0);
        }
    }
    
    public Dictionary<CategoryType, Budget> GetAllBudgets()
    {
        return _budgets;
    }

    public async Task LoadFromStorageTransactions()
    {
        try
        {
            var loadedTransactions = await _storage.LoadTransactions();
            foreach (var transaction in loadedTransactions)
            {
                _transactions.Add(transaction);
            }
            if (_transactions.Any())
            {
                int maxId = _transactions.Max(t => t.Id);
                Transaction.SetNextId(maxId + 1);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    public async Task LoadFromStorageBudgets()
    {
        try
        {
            var loadedBudgets = await _storage.LoadBudgets();
            foreach (var kvp in loadedBudgets)
            {
                _budgets[kvp.Key] = kvp.Value;
                kvp.Value.OnBudgetExceeded += OnBudgetExceededHandler;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}
using FinanceTracker.CLI.Exceptions;
using FinanceTracker.CLI.Models;
namespace FinanceTracker.CLI.Services;

public class FinanceManager
{
    public FinanceManager()
    {
        _transactions = new List<Transaction>();
    }
    private readonly List<Transaction> _transactions;

    public void AddTransaction(Transaction transactionNew)
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
        
        
    }

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
}
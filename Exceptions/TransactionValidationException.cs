namespace FinanceTracker.CLI.Exceptions;

public class TransactionValidationException : Exception
{
    public TransactionValidationException() : base() { }

    public TransactionValidationException(string message) : base(message) { }

    public TransactionValidationException(string message, Exception innerException) : base(message, innerException) { }
}
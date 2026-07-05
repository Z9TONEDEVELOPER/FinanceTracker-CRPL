using FinanceTracker.CLI.UI;

namespace FinanceTracker.CLI.Services;

public class TransactionValidator
{
    public static (bool, string) ValidateAmount(decimal amount)
    {
        if (amount > 0)
            return (true, "");
        else
            return (false, "Enter valid amount");
    }

    public static (bool, string) ValidateDate(DateTime date)
    {
        if (DateTime.Now < date)
            return (false, "The date cannot be in the future");
        else
            return (true, "");
    }

    public static (bool, string) ValidateDescription(string? desc)
    {
        if (!string.IsNullOrEmpty(desc))
            return (true, "");
        else
            return (false, "Enter the description");
    }
}
using MoneyChange.model;

namespace MoneyChange.service
{
    public interface ITaxCalculator
    {
        decimal CalculateTax(Money income, decimal exchangeRate = 1.0m);
    }
}



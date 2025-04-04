using ArgumentException = System.ArgumentException;
using ArgumentNullException = System.ArgumentNullException;
using InvalidOperationException = System.InvalidOperationException;

using System;

namespace MoneyChange.model
{
    public class Money
    {
        private const string BaseCurrency = "GBP";
        private const string ErrorMessageEmptyCurrency = "La devise ne peut pas être vide";
        private const string ErrorMessageInvalidExchangeRate = "Le taux de change doit être supérieur à 0";
        private const string ErrorMessageDirectConversion = "La conversion directe entre devises non-base n'est pas supportée";

        public decimal Amount { get; private set; }
        public string Currency { get; private set; }

        public Money(decimal amount, string currency)
        {
            Amount = amount;
            ValidateCurrency(currency);
            Currency = currency;
        }

        public Money ConvertTo(string targetCurrency, decimal exchangeRate)
        {
            ValidateConversionParameters(targetCurrency, exchangeRate);

            if (Currency == targetCurrency)
            {
                return this;
            }

            decimal convertedAmount = CalculateConvertedAmount(targetCurrency, exchangeRate);
            return new Money(convertedAmount, targetCurrency);
        }

        private static void ValidateCurrency(string currency)
        {
            if (currency == null)
                throw new ArgumentNullException(nameof(currency));

            if (string.IsNullOrWhiteSpace(currency))
                throw new ArgumentException(ErrorMessageEmptyCurrency, nameof(currency));
        }

        private void ValidateConversionParameters(string targetCurrency, decimal exchangeRate)
        {
            ValidateCurrency(targetCurrency);

            if (exchangeRate <= 0)
                throw new ArgumentException(ErrorMessageInvalidExchangeRate, nameof(exchangeRate));
        }

        private decimal CalculateConvertedAmount(string targetCurrency, decimal exchangeRate)
        {
            if (Currency == BaseCurrency)
                return Amount * exchangeRate;

            if (targetCurrency == BaseCurrency)
                return Amount / exchangeRate;

            throw new InvalidOperationException(ErrorMessageDirectConversion);
        }
    }
    
    
}
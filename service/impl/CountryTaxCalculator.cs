using System;
using System.Collections.Generic;
using MoneyChange.model;

namespace MoneyChange.service.impl
{
    public class CountryTaxCalculator : ITaxCalculator
    {
        private readonly List<TaxBracket> _taxBrackets;
        private readonly decimal _personalAllowance;

        public CountryTaxCalculator(List<TaxBracket> taxBrackets, decimal personalAllowance)
        {
            _taxBrackets = taxBrackets ?? throw new ArgumentNullException(nameof(taxBrackets));
            _personalAllowance = personalAllowance;
        }

        public decimal CalculateTax(Money income, decimal exchangeRate = 1.0m)
        {
            if (exchangeRate <= 0)
            {
                throw new ArgumentException("Exchange rate must be greater than 0.");
            }

            decimal incomeInGBPAmount;
            if (income.Currency == "GBP")
            {
                incomeInGBPAmount = income.Amount;
            }
            else
            {
                incomeInGBPAmount = income.Amount / exchangeRate;
            }

            if (incomeInGBPAmount <= _personalAllowance)
            {
                return 0;
            }

            decimal taxableIncome = incomeInGBPAmount - _personalAllowance;
            decimal tax = 0;

            var sortedBrackets = new List<TaxBracket>(_taxBrackets);
            sortedBrackets.Sort((a, b) => a.LowerLimit.CompareTo(b.LowerLimit));

            decimal previousLimit = 0;
            foreach (var bracket in sortedBrackets)
            {
                if (taxableIncome <= previousLimit) 
                    continue;

                decimal lowerAmount = Math.Max(previousLimit, bracket.LowerLimit);
                decimal upperAmount = Math.Min(taxableIncome, bracket.UpperLimit);
                decimal amountInBracket = upperAmount - lowerAmount;

                if (amountInBracket > 0)
                {
                    tax += amountInBracket * bracket.Rate;
                }

                previousLimit = bracket.UpperLimit;
            }

            if (income.Currency != "GBP")
            {
                tax *= exchangeRate;
            }

            return tax;
        }
    }
}
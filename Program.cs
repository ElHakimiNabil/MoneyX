using System;
using System.Collections.Generic;
using MoneyChange.model;
using MoneyChange.service;
using MoneyChange.service.impl;

namespace MoneyChange.ConsoleApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Tax Calculator Application");
            Console.WriteLine("=========================");
                
            try
            {
                var taxBrackets = new List<TaxBracket>
                {
                    new TaxBracket(0, 37700, 0.2m),         
                    new TaxBracket(37700, 150000, 0.4m),    
                    new TaxBracket(150000, decimal.MaxValue, 0.45m)
                };
                
                decimal personalAllowance = 12570; 
                var calculator = new CountryTaxCalculator(taxBrackets, personalAllowance);
                
                while (true)
                {
                    Console.WriteLine("\nEnter income amount (or type 'exit' to quit):");
                    string incomeInput = Console.ReadLine();
                    
                    if (incomeInput.ToLower() == "exit")
                        break;
                    
                    if (!decimal.TryParse(incomeInput, out decimal incomeAmount))
                    {
                        Console.WriteLine("Invalid income amount. Please enter a valid number.");
                        continue;
                    }
                    
                    Console.WriteLine("Enter currency (default: GBP):");
                    string currency = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(currency))
                    {
                        currency = "GBP";
                    }
                    
                    decimal exchangeRate = 1.0m;
                    if (currency != "GBP")
                    {
                        Console.WriteLine($"Enter exchange rate from {currency} to GBP (e.g., if 1 {currency} = 0.8 GBP, enter 0.8):");
                        string rateInput = Console.ReadLine();
                        
                        if (!decimal.TryParse(rateInput, out exchangeRate) || exchangeRate <= 0)
                        {
                            Console.WriteLine("Invalid exchange rate. Using default rate of 1.0");
                            exchangeRate = 1.0m;
                        }
                    }
                    
                    try
                    {
                        var income = new Money(incomeAmount, currency);
                        decimal tax = calculator.CalculateTax(income, exchangeRate);
                        
                        Console.WriteLine($"Income: {incomeAmount} {currency}");
                        Console.WriteLine($"Tax: {tax:F2} {currency}");
                        
                        decimal netIncome = incomeAmount - tax;
                        Console.WriteLine($"Net income: {netIncome:F2} {currency}");
                        
                        decimal taxPercentage = (tax / incomeAmount) * 100;
                        Console.WriteLine($"Effective tax rate: {taxPercentage:F2}%");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error calculating tax: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            
            Console.WriteLine("\nThank you for using the Tax Calculator. Press any key to exit.");
            Console.ReadKey();
        }
    }
}
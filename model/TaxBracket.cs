namespace MoneyChange.model
{
    public class TaxBracket
    {
        public decimal LowerLimit { get; }
        public decimal UpperLimit { get; }
        public decimal Rate { get; }

        public TaxBracket(decimal lowerLimit, decimal upperLimit, decimal rate)
        {
            LowerLimit = lowerLimit;
            UpperLimit = upperLimit;
            Rate = rate;
        }
    }
}
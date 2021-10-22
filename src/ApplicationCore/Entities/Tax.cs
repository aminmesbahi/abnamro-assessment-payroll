namespace Assessment.ApplicationCore.Entities
{
    public class TaxBracket : BaseEntity
    {
        public TaxBracket()
        {

        }

        public TaxBracket(int taxReferenceId, decimal low, decimal high, decimal rate)
        {
            TaxReferenceId = taxReferenceId;
            Low = low;
            High = high;
            Rate = rate;
        }
        public int TaxReferenceId { get; set; }
        public TaxReference TaxReference { get; set; }
        public decimal Low { get; set; }
        public decimal High { get; set; }
        public decimal Rate { get; set; }
    }
    public class TaxReference : BaseEntity
    {
        public TaxReference()
        {

        }
        public TaxReference(int year, IList<TaxBracket> taxBrackets)
        {
            Year = year;
            TaxBrackets = taxBrackets;
        }
        public TaxReference(int year)
        {
            Year = year;
        }
        public int Year { get; set; }
        public IList<TaxBracket> TaxBrackets { get; set; }
    }

}
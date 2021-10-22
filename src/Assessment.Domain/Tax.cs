namespace Assessment.Domain
{
    public class TaxBracket
    {
        public int Low { get; set; }
        public int High { get; set; }
        public decimal Rate { get; set; }
    }
    public class TaxReference
    {
        public int Year { get; set; }
        public IList<TaxBracket> TaxBrackets { get; set; }
    }

}
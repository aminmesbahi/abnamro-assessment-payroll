namespace Assessment.Domain
{
    public class PaymentHistory
    {
        public int Id { get; set; }
        public int TimesheetId { get; set; }
        public Timesheet Timesheet { get; set; }
        public DateTime CalculationTime { get; set; }
        public DateTime PaymentTime { get; set; }
        public IDictionary<string,decimal> Deductions { get; set; }
        public IDictionary<string, decimal> Benefits { get; set; }
        public double GrossIncome { get; set; }
        public double NetIncome { get; set; }

    }

}
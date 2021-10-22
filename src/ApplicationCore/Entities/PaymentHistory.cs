using System.ComponentModel.DataAnnotations.Schema;

namespace Assessment.ApplicationCore.Entities
{
    public class PaymentHistory : BaseEntity
    {
        public PaymentHistory()
        {

        }

        public PaymentHistory(int timesheetId, DateTime calculationTime, DateTime paymentTime, decimal grossIncome, decimal netIncome, decimal tax)
        {
            TimesheetId = timesheetId;
            CalculationTime = calculationTime;
            PaymentTime = paymentTime;
            GrossIncome = grossIncome;
            NetIncome = netIncome;
        }

        public PaymentHistory(int timesheetId, DateTime calculationTime, DateTime paymentTime, List<PaymentFactor> paymentFactors, decimal grossIncome, decimal netIncome, decimal tax)
        {
            TimesheetId = timesheetId;
            CalculationTime = calculationTime;
            PaymentTime = paymentTime;
            PaymentFactors = paymentFactors;
            GrossIncome = grossIncome;
            NetIncome = netIncome;
            Tax = Tax;
        }

        public int TimesheetId { get; set; }
        public Timesheet Timesheet { get; set; }
        public DateTime CalculationTime { get; set; }
        public DateTime PaymentTime { get; set; }
        public List<PaymentFactor> PaymentFactors { get; set; }
        public decimal GrossIncome { get; set; }
        public decimal NetIncome { get; set; }
        public decimal Tax { get; set; }

    }
    public enum PaymentFactorType
    {
        Deduction,
        Benefit
    }
    public class PaymentFactor : BaseEntity
    {
        public PaymentFactor()
        {

        }
        public PaymentFactor(int paymentHistoryId, PaymentFactorType paymentFactorType, string name, decimal amount)
        {
            PaymentHistoryId = paymentHistoryId;
            PaymentFactorType = paymentFactorType;
            Name = name;
            Amount = amount;
        }

        public int PaymentHistoryId { get; set; }
        public PaymentHistory PaymentHistory { get; set; }
        public PaymentFactorType PaymentFactorType { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }

    }
}
namespace Assessment.Domain
{
    public enum PayMethod
    {
        Hourly,
        Monthly
    }
    public class Contract
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public PayMethod PayMethod { get; set; }
        public decimal Wage { get; set; }
        public int WorkingWeekHours { get; set; }
        public int MaxAllowedSickLeaveHours { get; set; }
        public int MaxAllowedVacationHours { get; set; }
    }

}
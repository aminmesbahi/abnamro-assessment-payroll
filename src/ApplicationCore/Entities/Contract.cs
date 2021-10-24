namespace Assessment.ApplicationCore.Entities
{
    public enum PayMethod
    {
        Hourly,
        Monthly
    }
    public class Contract : BaseEntity
    {
        public Contract()
        {

        }
        public Contract(int employeeId, DateTime startDate, DateTime endDate, PayMethod payMethod, decimal wage, int workingWeekHours, int maxAllowedSickLeaveHours, int maxAllowedVacationHours)
        {
            EmployeeId = employeeId;
            StartDate = startDate;
            EndDate = endDate;
            PayMethod = payMethod;
            Wage = wage;
            WorkingWeekHours = workingWeekHours;
            MaxAllowedSickLeaveHours = maxAllowedSickLeaveHours;
            MaxAllowedVacationHours = maxAllowedVacationHours;
        }

        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public PayMethod PayMethod { get; set; }
        public decimal Wage { get; set; }
        public int WorkingWeekHours { get; set; }
        public int MaxAllowedSickLeaveHours { get; set; }
        public int MaxAllowedVacationHours { get; set; }
        public IList<Timesheet> Timesheets { get; set; }
    }

}
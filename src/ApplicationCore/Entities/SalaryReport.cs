using Assessment.ApplicationCore.Entities;

namespace Assessment.ApplicationCore.Entities
{
    public class SalaryReport
    {
        public SalaryReport(int employeeId, string firstName, string lastName, DateTime birthDate, int age, DateTime contractStartDate, DateTime contractEndDate, PayMethod payMethod, decimal wage, int workingWeekHours, int maxAllowedSickLeaveHours, int maxAllowedVacationHours, DateTime timesheetFromDate, DateTime timesheetToDate, int timesheetWorkingHours, int sickLeaveHours, int vacationHours, string payed, DateTime calculationTime, DateTime paymentTime, decimal benefitsTotal, decimal deductionsTotal, decimal grossIncome, decimal netIncome, decimal tax)
        {
            EmployeeId = employeeId;
            FirstName = firstName;
            LastName = lastName;
            BirthDate = birthDate;
            Age = age;
            ContractStartDate = contractStartDate;
            ContractEndDate = contractEndDate;
            PayMethod = payMethod;
            Wage = wage;
            WorkingWeekHours = workingWeekHours;
            MaxAllowedSickLeaveHours = maxAllowedSickLeaveHours;
            MaxAllowedVacationHours = maxAllowedVacationHours;
            TimesheetFromDate = timesheetFromDate;
            TimesheetToDate = timesheetToDate;
            TimesheetWorkingHours = timesheetWorkingHours;
            SickLeaveHours = sickLeaveHours;
            VacationHours = vacationHours;
            Payed = payed;
            CalculationTime = calculationTime;
            PaymentTime = paymentTime;
            BenefitsTotal = benefitsTotal;
            DeductionsTotal = deductionsTotal;
            GrossIncome = grossIncome;
            NetIncome = netIncome;
            Tax = tax;
        }
        public SalaryReport()
        {

        }
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public int Age { get; set; }
        public DateTime ContractStartDate { get; set; }
        public DateTime ContractEndDate { get; set; }
        public PayMethod PayMethod { get; set; }
        public decimal Wage { get; set; }
        public int WorkingWeekHours { get; set; }
        public int MaxAllowedSickLeaveHours { get; set; }
        public int MaxAllowedVacationHours { get; set; }
        public DateTime TimesheetFromDate { get; set; }
        public DateTime TimesheetToDate { get; set; }
        public int TimesheetWorkingHours { get; set; }
        public int SickLeaveHours { get; set; }
        public int VacationHours { get; set; }
        public string Payed { get; set; }
        public DateTime CalculationTime { get; set; }
        public DateTime PaymentTime { get; set; }
        public decimal BenefitsTotal { get; set; }
        public decimal DeductionsTotal { get; set; }
        public decimal GrossIncome { get; set; }
        public decimal NetIncome { get; set; }
        public decimal Tax { get; set; }
    }
}

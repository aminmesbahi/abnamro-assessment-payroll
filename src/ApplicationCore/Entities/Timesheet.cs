namespace Assessment.ApplicationCore.Entities
{
    public class Timesheet : BaseEntity
    {
        public Timesheet()
        {

        }
        public Timesheet(int contractId, DateTime fromDate, DateTime toDate, int workingHours, int sickLeaveHours, int vacationHours, bool isPayed)
        {
            ContractId = contractId;
            FromDate = fromDate;
            ToDate = toDate;
            WorkingHours = workingHours;
            SickLeaveHours = sickLeaveHours;
            VacationHours = vacationHours;
            IsPayed = isPayed;
        }

        public int ContractId { get; set; }
        public Contract Contract { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int WorkingHours { get; set; }
        public int SickLeaveHours { get; set; }
        public int VacationHours { get; set; }
        public bool IsPayed { get; set; }
        public IList<PaymentHistory> PaymentHistories { get; set; }
    }

}
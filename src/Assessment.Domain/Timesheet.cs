namespace Assessment.Domain
{
    public class Timesheet
    {
        public int Id { get; set; }
        public int ContractId { get; set; }
        public Contract Contract { get; set; }
        public DateOnly FromDate { get; set; }
        public DateOnly ToDate { get; set; }
        public int WorkingHours { get; set; }
        public int SickLeaveHours { get; set; }
        public int VacationHours { get; set; }
        public bool IsPayed { get; set; }
    }

}
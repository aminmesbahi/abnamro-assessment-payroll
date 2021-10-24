namespace Assessment.Api.EmployeeEndpoints
{
    public class EmployeeDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public int Age { get { return DateTime.Now.Subtract(BirthDate).Days / 365; } }

        public bool HaveContract { get; set; }

    }
}
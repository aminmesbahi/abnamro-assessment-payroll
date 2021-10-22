namespace Assessment.Api.EmployeeEndpoints
{
    public class CreateEmployeeRequest : BaseRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
    }
}

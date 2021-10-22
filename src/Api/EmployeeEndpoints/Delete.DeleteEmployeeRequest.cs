namespace Assessment.Api.EmployeeEndpoints
{
    public class DeleteEmployeeRequest : BaseRequest
    {
        //[FromRoute]
        public int EmployeeId { get; set; }
    }
}

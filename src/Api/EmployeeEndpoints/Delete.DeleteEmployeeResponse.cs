namespace Assessment.Api.EmployeeEndpoints
{
    public class DeleteEmployeeResponse : BaseResponse
    {
        public DeleteEmployeeResponse(Guid correlationId) : base(correlationId)
        {
        }

        public DeleteEmployeeResponse()
        {
        }

        public string Status { get; set; } = "Deleted";
    }
}

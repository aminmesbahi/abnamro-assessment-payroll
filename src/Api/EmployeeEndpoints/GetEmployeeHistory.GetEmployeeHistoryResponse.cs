using Assessment.ApplicationCore.Entities;

namespace Assessment.Api.EmployeeEndpoints
{
    public class GetEmployeeHistoryResponse : BaseResponse
    {
        public GetEmployeeHistoryResponse(Guid correlationId) : base(correlationId)
        {
        }

        public GetEmployeeHistoryResponse()
        {
        }

        public Employee Employee { get; set; }
    }
}
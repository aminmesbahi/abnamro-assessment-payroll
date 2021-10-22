using System;

namespace Assessment.Api.EmployeeEndpoints
{
    public class GetByIdEmployeeResponse : BaseResponse
    {
        public GetByIdEmployeeResponse(Guid correlationId) : base(correlationId)
        {
        }

        public GetByIdEmployeeResponse()
        {
        }

        public EmployeeDto Employee { get; set; }
    }
}

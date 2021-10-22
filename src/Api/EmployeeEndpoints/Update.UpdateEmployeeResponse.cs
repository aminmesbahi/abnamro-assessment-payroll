using System;

namespace Assessment.Api.EmployeeEndpoints
{
    public class UpdateEmployeeResponse : BaseResponse
    {
        public UpdateEmployeeResponse(Guid correlationId) : base(correlationId)
        {
        }

        public UpdateEmployeeResponse()
        {
        }

        public EmployeeDto Employee { get; set; }
    }
}

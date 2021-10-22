using System;
using System.Collections.Generic;

namespace Assessment.Api.EmployeeEndpoints
{
    public class ListPagedEmployeeResponse : BaseResponse
    {
        public ListPagedEmployeeResponse(Guid correlationId) : base(correlationId)
        {
        }

        public ListPagedEmployeeResponse()
        {
        }

        public List<EmployeeDto> Employees { get; set; } = new List<EmployeeDto>();
        public int PageCount { get; set; }
    }
}

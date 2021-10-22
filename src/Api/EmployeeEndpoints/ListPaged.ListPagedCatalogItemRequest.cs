using Assessment.ApplicationCore.Entities;

namespace Assessment.Api.EmployeeEndpoints
{
    public class ListPagedEmployeeRequest : BaseRequest 
    {
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public PayMethod? PayMethod { get; set; }
    }
}

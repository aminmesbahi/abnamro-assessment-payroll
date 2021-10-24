using Assessment.Api;

namespace Assessment.Api.EmployeeEndpoints
{
    public class GetSalaryReportCsvByIdRequest : BaseRequest
    {
        public int PaymentHistoryId { get; set; }
    }
}

using Assessment.ApplicationCore.Entities;

namespace Assessment.Api.EmployeeEndpoints
{
    public class GetAllSalaryReportCsvByEmployeeResponse : BaseResponse
    {
        public GetAllSalaryReportCsvByEmployeeResponse(Guid correlationId) : base(correlationId)
        {

        }
        public GetAllSalaryReportCsvByEmployeeResponse()
        {

        }
        public IList<SalaryReport> SalaryReport { get; set; }
    }
}

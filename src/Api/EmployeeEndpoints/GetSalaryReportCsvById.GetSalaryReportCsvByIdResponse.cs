namespace Assessment.Api.EmployeeEndpoints
{
    public class GetSalaryReportCsvByIdResponse : BaseResponse
    {
        public GetSalaryReportCsvByIdResponse(Guid correlationId) : base(correlationId)
        {

        }
        public GetSalaryReportCsvByIdResponse()
        {

        }
        public SalaryReportDto SalaryReport { get; set; }
    }
}

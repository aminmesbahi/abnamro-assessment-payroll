using Ardalis.ApiEndpoints;
using Assessment.ApplicationCore.Entities;
using Assessment.ApplicationCore.Interfaces;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Text;

namespace Assessment.Api.EmployeeEndpoints
{
public class GetAllSalaryReportCsvByEmployee : EndpointBaseAsync
    .WithRequest<GetAllSalaryReportCsvByEmployeeRequest>
    .WithActionResult<GetAllSalaryReportCsvByEmployeeResponse>
{
        private readonly IAsyncRepository<Employee> _employeeRepository;

        public GetAllSalaryReportCsvByEmployee(IAsyncRepository<Employee> employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        [HttpGet("api/employees/salary-report/all/{EmployeeId}")]
        [SwaggerOperation(
            Summary = "Get a report including all payments for an employee (.csv)",
            Description = "Get a report including all payments for an employee (.csv)",
            OperationId = "employee.GetAllSalaryReportCsvByEmployee",
            Tags = new[] { "EmployeeEndpoints" })
        ]
        public override async Task<ActionResult<GetAllSalaryReportCsvByEmployeeResponse>> HandleAsync([FromRoute] GetAllSalaryReportCsvByEmployeeRequest request, CancellationToken cancellationToken)
        {
            var response = new GetAllSalaryReportCsvByEmployeeResponse(request.CorrelationId());

            var report = await _employeeRepository.GetEmployeePaymentsHistory(request.EmployeeId, cancellationToken);
            if (report is null) return NotFound();

            response.SalaryReport = report.ToList();
           

            var cc = new CsvConfiguration(new System.Globalization.CultureInfo("en-US"));
            using (var ms = new MemoryStream())
            {
                using (var sw = new StreamWriter(stream: ms, encoding: new UTF8Encoding(true)))
                {
                    using (var cw = new CsvWriter(sw, cc))
                    {
                        var export = new List<SalaryReport>();
                        export.AddRange(response.SalaryReport.ToList());
                        cw.WriteRecords(export);
                    }// The stream gets flushed here.
                    return File(ms.ToArray(), "text/csv", $"SalaryExport_{response.SalaryReport[0].FirstName}_{response.SalaryReport[0].LastName}_all.csv");
                }
            }
        }
    }
}

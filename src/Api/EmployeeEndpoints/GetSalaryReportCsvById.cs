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
    public class GetSalaryReportCsvById : EndpointBaseAsync
        .WithRequest<GetSalaryReportCsvByIdRequest>
        .WithActionResult<GetSalaryReportCsvByIdResponse>
    {
        private readonly IAsyncRepository<Employee> _employeeRepository;

        public GetSalaryReportCsvById(IAsyncRepository<Employee> employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        [HttpGet("api/employees/salary-report/{PaymentHistoryId}")]
        [SwaggerOperation(
            Summary = "Get a Salary Report by Id",
            Description = "Get a Salary Report by Id",
            OperationId = "employee.GetSalaryReportById",
            Tags = new[] { "EmployeeEndpoints" })
        ]
        public override async Task<ActionResult<GetSalaryReportCsvByIdResponse>> HandleAsync([FromRoute] GetSalaryReportCsvByIdRequest request, CancellationToken cancellationToken)
        {
            var response = new GetSalaryReportCsvByIdResponse(request.CorrelationId());

            var report = await _employeeRepository.GetSalaryReportByPaymentHistoryIdAsync(request.PaymentHistoryId, cancellationToken);
            if (report is null) return NotFound();

            response.SalaryReport = new SalaryReportDto
            {
                FirstName = report.FirstName,
                LastName = report.LastName,
                BirthDate = report.BirthDate,
                Age = report.Age,
                BenefitsTotal = report.BenefitsTotal,
                DeductionsTotal = report.DeductionsTotal,
                Tax = report.Tax,
                NetIncome = report.NetIncome,
                GrossIncome = report.GrossIncome,
                EmployeeId = report.EmployeeId,
                CalculationTime = report.CalculationTime,
                ContractEndDate = report.ContractEndDate,
                ContractStartDate = report.ContractStartDate,
                MaxAllowedSickLeaveHours = report.MaxAllowedSickLeaveHours,
                MaxAllowedVacationHours = report.MaxAllowedVacationHours,
                Payed = report.Payed,
                PaymentTime = report.PaymentTime,
                PayMethod = report.PayMethod,
                SickLeaveHours = report.SickLeaveHours,
                VacationHours = report.VacationHours,
                TimesheetFromDate = report.TimesheetFromDate,
                TimesheetToDate = report.TimesheetToDate,
                TimesheetWorkingHours = report.TimesheetWorkingHours,
                Wage = report.Wage,
                WorkingWeekHours = report.WorkingWeekHours



            };
            // return Ok(response);

            var cc = new CsvConfiguration(new System.Globalization.CultureInfo("en-US"));
            using (var ms = new MemoryStream())
            {
                using (var sw = new StreamWriter(stream: ms, encoding: new UTF8Encoding(true)))
                {
                    using (var cw = new CsvWriter(sw, cc))
                    {
                        var export = new List<SalaryReportDto>();
                        export.Add(response.SalaryReport);
                        cw.WriteRecords(export);
                    }// The stream gets flushed here.
                    return File(ms.ToArray(), "text/csv", $"SalaryExport_{response.SalaryReport.FirstName}_{response.SalaryReport.LastName}_{response.SalaryReport.PaymentTime}.csv");
                }
            }
        }
    }
}

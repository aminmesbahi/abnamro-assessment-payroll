using Ardalis.ApiEndpoints;
using Assessment.ApplicationCore.Entities;
using Assessment.ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Assessment.Api.EmployeeEndpoints
{
    public class GetEmployeeHistory : EndpointBaseAsync
        .WithRequest<GetEmployeeHistoryRequest>
        .WithActionResult<GetEmployeeHistoryResponse>
    {
        private readonly IAsyncRepository<Employee> _employeeRepository;
        public GetEmployeeHistory(IAsyncRepository<Employee> employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }
        [HttpGet("api/employees/history/{EmployeeId}")]
        [SwaggerOperation(
            Summary = "Get an Employee history by Id",
            Description = "Get an Employee history by Id",
            OperationId = "employee.GetGetEmployeeHistory",
            Tags = new[] { "EmployeeEndpoints" })
        ]
        public override async Task<ActionResult<GetEmployeeHistoryResponse>> HandleAsync([FromRoute] GetEmployeeHistoryRequest request, CancellationToken cancellationToken)
        {
            var response = new GetEmployeeHistoryResponse(request.CorrelationId());

            var employee = await _employeeRepository.GetEmployeeHistory(request.EmployeeId, cancellationToken);
            if (employee is null) return NotFound();

            response.Employee = employee;
            JsonSerializerOptions options = new()
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                WriteIndented = true
            };
            return Ok(JsonSerializer.Serialize(response, options));
        }
    }
}

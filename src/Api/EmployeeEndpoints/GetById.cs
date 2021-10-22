using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Assessment.ApplicationCore.Entities;
using Assessment.ApplicationCore.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;

namespace Assessment.Api.EmployeeEndpoints
{
    public class GetById : EndpointBaseAsync
        .WithRequest<GetByIdEmployeeRequest>
        .WithActionResult<GetByIdEmployeeResponse>
    {
        private readonly IAsyncRepository<Employee> _employeeRepository;

        public GetById(IAsyncRepository<Employee> employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        [HttpGet("api/employees/{EmployeeId}")]
        [SwaggerOperation(
            Summary = "Get an Employee by Id",
            Description = "Get an Employee by Id",
            OperationId = "employee.GetById",
            Tags = new[] { "EmployeeEndpoints" })
        ]
        public override async Task<ActionResult<GetByIdEmployeeResponse>> HandleAsync([FromRoute] GetByIdEmployeeRequest request, CancellationToken cancellationToken)
        {
            var response = new GetByIdEmployeeResponse(request.CorrelationId());

            var employee = await _employeeRepository.GetByIdAsync(request.EmployeeId, cancellationToken);
            if (employee is null) return NotFound();

            response.Employee = new EmployeeDto
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                BirthDate = employee.BirthDate
            };
            return Ok(response);
        }
    }
}

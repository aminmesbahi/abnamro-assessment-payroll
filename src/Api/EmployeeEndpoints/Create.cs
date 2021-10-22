using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Assessment.ApplicationCore.Entities;
using Assessment.ApplicationCore.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
namespace Assessment.Api.EmployeeEndpoints
{
    public class Create : EndpointBaseAsync
        .WithRequest<CreateEmployeeRequest>
        .WithActionResult<CreateEmployeeResponse>
    {
        private readonly IAsyncRepository<Employee> _employeeRepository;

        public Create(IAsyncRepository<Employee> employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        [HttpPost("api/employees")]
        [SwaggerOperation(
            Summary = "Creates a new Employee",
            Description = "Creates an Employee",
            OperationId = "employee.create",
            Tags = new[] { "EmployeeEndpoints" })
        ]
        public override async Task<ActionResult<CreateEmployeeResponse>> HandleAsync(CreateEmployeeRequest request, CancellationToken cancellationToken)
        {
            var response = new CreateEmployeeResponse(request.CorrelationId());

            var newEmployee = new Employee(request.FirstName, request.LastName, request.BirthDate);

            newEmployee = await _employeeRepository.AddAsync(newEmployee, cancellationToken);

            var dto = new EmployeeDto
            {
                Id = newEmployee.Id,
                FirstName = newEmployee.FirstName,
                LastName = newEmployee.LastName,
                BirthDate = newEmployee.BirthDate

            };
            response.CatalogItem = dto;
            return response;
        }


    }
}

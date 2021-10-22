using Ardalis.ApiEndpoints;
using Assessment.ApplicationCore.Entities;
using Assessment.ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Assessment.Api.EmployeeEndpoints
{
    public class Delete : EndpointBaseAsync
        .WithRequest<DeleteEmployeeRequest>
        .WithActionResult<DeleteEmployeeResponse>
    {
        private readonly IAsyncRepository<Employee> _employeeRepository;

        public Delete(IAsyncRepository<Employee> employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        [HttpDelete("api/employees/{EmployeeId}")]
        [SwaggerOperation(
            Summary = "Deletes an Employee",
            Description = "Deletes an Employee",
            OperationId = "employees.Delete",
            Tags = new[] { "EmployeeEndpoints" })
        ]
        public override async Task<ActionResult<DeleteEmployeeResponse>> HandleAsync([FromRoute] DeleteEmployeeRequest request, CancellationToken cancellationToken)
        {
            var response = new DeleteEmployeeResponse(request.CorrelationId());

            var itemToDelete = await _employeeRepository.GetByIdAsync(request.EmployeeId, cancellationToken);
            if (itemToDelete is null) return NotFound();

            await _employeeRepository.DeleteAsync(itemToDelete, cancellationToken);

            return Ok(response);
        }
    }
}

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
    public class Update : EndpointBaseAsync
       .WithRequest<UpdateEmployeeRequest>
       .WithActionResult<UpdateEmployeeResponse>
    {
        private readonly IAsyncRepository<Employee> _employeeRepository;      

        public Update(IAsyncRepository<Employee> employeeRepository)
        {
            _employeeRepository = employeeRepository;           
        }

        [HttpPut("api/employees")]
        [SwaggerOperation(
            Summary = "Updates an Employee",
            Description = "Updates an Employee",
            OperationId = "employee.update",
            Tags = new[] { "EmployeeEndpoints" })
        ]
        public override async Task<ActionResult<UpdateEmployeeResponse>> HandleAsync(UpdateEmployeeRequest request, CancellationToken cancellationToken)
        {
            var response = new UpdateEmployeeResponse(request.CorrelationId());

            var existingItem = await _employeeRepository.GetByIdAsync(request.Id, cancellationToken);

            existingItem.UpdateDetails(request.FirstName, request.LastName, request.BirthDate);          

            await _employeeRepository.UpdateAsync(existingItem, cancellationToken);

            var dto = new EmployeeDto
            {
                Id = existingItem.Id,
                FirstName = existingItem.FirstName,
                LastName= existingItem.LastName,
                BirthDate=existingItem.BirthDate
            };
            response.Employee = dto;
            return response;
        }
    }
}

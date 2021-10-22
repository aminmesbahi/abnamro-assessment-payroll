using Ardalis.ApiEndpoints;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Assessment.ApplicationCore.Entities;
using Assessment.ApplicationCore.Interfaces;
using Assessment.ApplicationCore.Specifications;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Assessment.Api.EmployeeEndpoints
{
    public class ListPaged : EndpointBaseAsync
        .WithRequest<ListPagedEmployeeRequest>
        .WithActionResult<ListPagedEmployeeResponse>
    {
        private readonly IAsyncRepository<Employee> _employeeRepository;
        private readonly IMapper _mapper;
        public ListPaged(IAsyncRepository<Employee> employeeRepository, IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }               

        [HttpGet("api/employees")]
        [SwaggerOperation(
            Summary = "List Employees (paged)",
            Description = "List Employees (paged)",
            OperationId = "employees.ListPaged",
            Tags = new[] { "EmployeeEndpoints" })
        ]
        public override async Task<ActionResult<ListPagedEmployeeResponse>> HandleAsync([FromQuery] ListPagedEmployeeRequest request, CancellationToken cancellationToken)
        {
            var response = new ListPagedEmployeeResponse(request.CorrelationId());

            var filterSpec = new EmployeeFilterSpecification(request.PayMethod);
            int totalItems = await _employeeRepository.CountAsync(filterSpec, cancellationToken);

            var pagedSpec = new EmployeeFilterPaginatedSpecification(
                skip: request.PageIndex * request.PageSize,
                take: request.PageSize,
                payMethod: request.PayMethod
               );

            var items = await _employeeRepository.ListAsync(pagedSpec, cancellationToken);

            response.Employees.AddRange(items.Select(_mapper.Map<EmployeeDto>));

            response.PageCount = int.Parse(Math.Ceiling((decimal)totalItems / request.PageSize).ToString());

            return Ok(response);
        }
    }
}

using Assessment.ApplicationCore.Entities;
using Assessment.ApplicationCore.Interfaces;
using Assessment.Web.Interfaces;

namespace Assessment.Web.Services
{
    public class EmployeeViewModelService : IEmployeeViewModelService
    {
        private readonly IAsyncRepository<Employee> _employeeRepository;

        public EmployeeViewModelService(IAsyncRepository<Employee> employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }
        public async Task<IEnumerable<EmployeeViewModel>> GetEmployees()
        {
            //_logger.LogInformation("GetBrands called.");
            var employees = await _employeeRepository.ListAllAsync();

            var items = employees
                .Select(employee => new EmployeeViewModel() { FirstName = employee.FirstName, LastName = employee.LastName, BirthDate = employee.BirthDate })
                .OrderBy(e => e.LastName)
                .ToList();

            return items;
        }
    }
}

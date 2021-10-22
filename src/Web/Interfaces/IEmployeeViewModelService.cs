using Assessment.ApplicationCore.Entities;

namespace Assessment.Web.Interfaces
{
    public interface IEmployeeViewModelService
    {
        
        Task<IEnumerable<EmployeeViewModel>> GetEmployees();
    }
}

using Ardalis.Specification;
using Assessment.ApplicationCore.Entities;

namespace Assessment.ApplicationCore.Specifications
{
    public class EmployeeFilterSpecification : Specification<Employee>
    {
        public EmployeeFilterSpecification(PayMethod? payMethod)
        {
            Query.Where(i => !payMethod.HasValue || (i.Contracts.Where(c => c.EndDate >= DateTime.Now).Any() && i.Contracts.OrderByDescending(c => c.Id).First().PayMethod == payMethod));
        }
    }
}

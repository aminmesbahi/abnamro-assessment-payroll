using Ardalis.Specification;
using Assessment.ApplicationCore.Entities;

namespace Assessment.ApplicationCore.Specifications
{
    public class EmployeeFilterPaginatedSpecification : Specification<Employee>
    {
        public EmployeeFilterPaginatedSpecification(int skip, int take, PayMethod? payMethod)
            : base()
        {
            Query.Where(i => !payMethod.HasValue || (i.Contracts.Where(c => c.EndDate >= DateTime.Now).Any() && i.Contracts.OrderByDescending(c=>c.Id).First().PayMethod == payMethod))
               .Skip(skip).Take(take);
        }
    }
}

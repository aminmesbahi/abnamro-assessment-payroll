using Ardalis.Specification;
using Assessment.ApplicationCore.Entities;

namespace Assessment.ApplicationCore.Interfaces
{
    public interface IAsyncRepository<T> where T : BaseEntity, IAggregateRoot
    {
        Task<T> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<T>> ListAllAsync(CancellationToken cancellationToken = default);
        Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec, CancellationToken cancellationToken = default);
        Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
        Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
        Task DeleteAsync(T entity, CancellationToken cancellationToken = default);
        Task<int> CountAsync(ISpecification<T> spec, CancellationToken cancellationToken = default);
        Task<T> FirstAsync(ISpecification<T> spec, CancellationToken cancellationToken = default);
        Task<T> FirstOrDefaultAsync(ISpecification<T> spec, CancellationToken cancellationToken = default);
        Task<SalaryReport> GetSalaryReportByPaymentHistoryIdAsync(int paymentHistoryId, CancellationToken cancellationToken = default);
        Task<Employee> GetEmployeeHistory(int employeeId, CancellationToken cancellationToken = default);
        Task<IQueryable<SalaryReport>> GetEmployeePaymentsHistory(int employeeId, CancellationToken cancellationToken = default);
    }
}

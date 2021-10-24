using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Assessment.ApplicationCore.Entities;
using Assessment.ApplicationCore.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Assessment.Infrastructure.Data
{
    public class EfRepository<T> : IAsyncRepository<T> where T : BaseEntity, IAggregateRoot
    {
        protected readonly PayrollContext _dbContext;

        public EfRepository(PayrollContext dbContext)
        {
            _dbContext = dbContext;
        }

        public virtual async Task<T> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var keyValues = new object[] { id };
            return await _dbContext.Set<T>().FindAsync(keyValues, cancellationToken);
        }

        public async Task<IReadOnlyList<T>> ListAllAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<T>().ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec, CancellationToken cancellationToken = default)
        {
            var specificationResult = ApplySpecification(spec);
            return await specificationResult.ToListAsync(cancellationToken);
        }

        public async Task<int> CountAsync(ISpecification<T> spec, CancellationToken cancellationToken = default)
        {
            var specificationResult = ApplySpecification(spec);
            return await specificationResult.CountAsync(cancellationToken);
        }

        public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return entity;
        }

        public async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
        {
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<T> FirstAsync(ISpecification<T> spec, CancellationToken cancellationToken = default)
        {
            var specificationResult = ApplySpecification(spec);
            return await specificationResult.FirstAsync(cancellationToken);
        }

        public async Task<T> FirstOrDefaultAsync(ISpecification<T> spec, CancellationToken cancellationToken = default)
        {
            var specificationResult = ApplySpecification(spec);
            return await specificationResult.FirstOrDefaultAsync(cancellationToken);
        }

        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            return SpecificationEvaluator.Default.GetQuery(_dbContext.Set<T>().AsQueryable(), spec);
        }
        public async Task<SalaryReport> GetSalaryReportByPaymentHistoryIdAsync(int paymentHistoryId, CancellationToken cancellationToken = default)
        {
            var paymentQuery =
                from ph in _dbContext.PaymentHistories
                join ts in _dbContext.Timesheets on ph.TimesheetId equals ts.Id
                join c in _dbContext.Contracts on ts.ContractId equals c.Id
                join e in _dbContext.Employees on c.EmployeeId equals e.Id
                where ph.Id == paymentHistoryId
                select new SalaryReport
                {
                    EmployeeId = e.Id,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    BirthDate = e.BirthDate,
                    Age = (DateTime.Now.Subtract(e.BirthDate).Days / 365),
                    ContractStartDate = c.StartDate,
                    ContractEndDate = c.EndDate,
                    PayMethod = c.PayMethod,
                    Wage = c.Wage,
                    WorkingWeekHours = c.WorkingWeekHours,
                    MaxAllowedSickLeaveHours = c.MaxAllowedSickLeaveHours,
                    MaxAllowedVacationHours = c.MaxAllowedVacationHours,
                    TimesheetFromDate = ts.FromDate,
                    TimesheetToDate = ts.ToDate,
                    TimesheetWorkingHours = ts.WorkingHours,
                    SickLeaveHours = ts.SickLeaveHours,
                    VacationHours = ts.VacationHours,
                    Payed = ts.IsPayed ? "Yes" : "No",
                    CalculationTime = ph.CalculationTime,
                    PaymentTime = ph.PaymentTime,
                    BenefitsTotal = ph.PaymentFactors.Where(f => f.PaymentFactorType == PaymentFactorType.Benefit).Sum(f => f.Amount),
                    DeductionsTotal = ph.PaymentFactors.Where(f => f.PaymentFactorType == PaymentFactorType.Deduction).Sum(f => f.Amount),
                    GrossIncome = ph.GrossIncome,
                    Tax = ph.Tax,
                    NetIncome = ph.NetIncome
                };
            return paymentQuery.FirstOrDefault();
        }

        public async Task<Employee> GetEmployeeHistory(int employeeId, CancellationToken cancellationToken = default)
        {
            var historyQuery = _dbContext.Employees.Where(e => e.Id == employeeId).Include(e => e.Contracts).ThenInclude(c => c.Timesheets).ThenInclude(t => t.PaymentHistories).ThenInclude(p => p.PaymentFactors);
            return historyQuery.FirstOrDefault();
        }

        public async Task<IQueryable<SalaryReport>> GetEmployeePaymentsHistory(int employeeId, CancellationToken cancellationToken = default)
        {
            var paymentQuery =
                from ph in _dbContext.PaymentHistories
                join ts in _dbContext.Timesheets on ph.TimesheetId equals ts.Id
                join c in _dbContext.Contracts on ts.ContractId equals c.Id
                join e in _dbContext.Employees on c.EmployeeId equals e.Id
                where e.Id == employeeId
                select new SalaryReport
                {
                    EmployeeId = e.Id,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    BirthDate = e.BirthDate,
                    Age = (DateTime.Now.Subtract(e.BirthDate).Days / 365),
                    ContractStartDate = c.StartDate,
                    ContractEndDate = c.EndDate,
                    PayMethod = c.PayMethod,
                    Wage = c.Wage,
                    WorkingWeekHours = c.WorkingWeekHours,
                    MaxAllowedSickLeaveHours = c.MaxAllowedSickLeaveHours,
                    MaxAllowedVacationHours = c.MaxAllowedVacationHours,
                    TimesheetFromDate = ts.FromDate,
                    TimesheetToDate = ts.ToDate,
                    TimesheetWorkingHours = ts.WorkingHours,
                    SickLeaveHours = ts.SickLeaveHours,
                    VacationHours = ts.VacationHours,
                    Payed = ts.IsPayed ? "Yes" : "No",
                    CalculationTime = ph.CalculationTime,
                    PaymentTime = ph.PaymentTime,
                    BenefitsTotal = ph.PaymentFactors.Where(f => f.PaymentFactorType == PaymentFactorType.Benefit).Sum(f => f.Amount),
                    DeductionsTotal = ph.PaymentFactors.Where(f => f.PaymentFactorType == PaymentFactorType.Deduction).Sum(f => f.Amount),
                    GrossIncome = ph.GrossIncome,
                    Tax = ph.Tax,
                    NetIncome = ph.NetIncome
                };
            return paymentQuery;
        }
    }
}
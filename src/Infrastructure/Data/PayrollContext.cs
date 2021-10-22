using Assessment.ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Assessment.Infrastructure.Data
{
    public class PayrollContext : DbContext
    {
        public PayrollContext(DbContextOptions<PayrollContext> options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<Timesheet> Timesheets { get; set; }
        public DbSet<PaymentHistory> PaymentHistories { get; set; }
        public DbSet<PaymentFactor> PaymentFactors { get; set; }
        public DbSet<TaxReference> TaxReferences { get; set; }

        public DbSet<TaxBracket> TaxBrackets { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}

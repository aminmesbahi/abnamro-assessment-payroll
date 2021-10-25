using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests
{
    public class SalaryCalculationTests : TestBase
    {
        [Fact]
        public async Task database_seed_should_have_correct_records()
        {
            // Arrange
            using var dbContext = GetDbContext();
            await PayrollContextSeed.SeedAsync(dbContext);


            // Act
            var employees = dbContext.Employees.OrderBy(e => e.Id).ToList();
            var contracts = dbContext.Contracts.OrderBy(e => e.Id).ToList();
            var employeeId = contracts.First().EmployeeId;

            // Assert
            Assert.NotNull(employees);
            Assert.Equal(4, employees.Count);
            Assert.Equal("Gary", employees[0].FirstName);
            Assert.Equal(1, employeeId);
        }


        [Theory]
        [InlineData(90000)]
        public async Task calculating_salary_test(decimal grossIncome)
        {
            // Arrange
            var calc = new Calculator();

            // Assert
            Assert.Equal(31751.9470M, calc.CalculateTax(grossIncome, GetTaxtBrackets()));

        }

    }
}
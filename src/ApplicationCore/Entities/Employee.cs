using Ardalis.GuardClauses;
using Assessment.ApplicationCore.Interfaces;

namespace Assessment.ApplicationCore.Entities
{
    public class Employee : BaseEntity, IAggregateRoot
    {
        public Employee(string firstName, string lastName, DateTime birthDate)
        {
            FirstName = firstName;
            LastName = lastName;
            BirthDate = birthDate;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }

        public IList<Contract> Contracts { get; set; }
        public void UpdateDetails(string firstName, string lastName, DateTime birthDate)
        {
            Guard.Against.NullOrEmpty(firstName, nameof(firstName));
            Guard.Against.NullOrEmpty(lastName, nameof(lastName));
            Guard.Against.OutOfSQLDateRange(birthDate, nameof(birthDate));

            FirstName = firstName;
            LastName = lastName;
            BirthDate = birthDate;
        }
    }

}
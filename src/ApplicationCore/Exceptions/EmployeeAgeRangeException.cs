namespace Assessment.ApplicationCore.Exceptions
{
    public class EmployeeAgeRangeException : Exception
    {
        public EmployeeAgeRangeException(int employeeAge) : base($"This employee age is {employeeAge}, and we are not allowed to hire people younger than 18 and older than 60.")
        {
            EmployeeAge = employeeAge;
        }
        protected EmployeeAgeRangeException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
        {
        }

        public EmployeeAgeRangeException(string message) : base(message)
        {
        }

        public EmployeeAgeRangeException(string message, Exception innerException) : base(message, innerException)
        {
        }
        public int EmployeeAge { get; }
    }
}

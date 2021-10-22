using System.ComponentModel.DataAnnotations;

namespace Assessment.Api.EmployeeEndpoints
{
    public class UpdateEmployeeRequest : BaseRequest
    {
        public int Id { get; set; }
        
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

    }
}

using Assessment.Api;

namespace Assessment.Api.EmployeeEndpoints
{
    public class CreateEmployeeResponse : BaseResponse
    {
        public CreateEmployeeResponse(Guid correlationId) : base(correlationId)
        {
        }

        public CreateEmployeeResponse()
        {
        }

        public EmployeeDto CatalogItem { get; set; }
    }
}

using AutoMapper;
using Assessment.ApplicationCore.Entities;
using Assessment.Api.EmployeeEndpoints;


namespace Microsoft.eShopWeb.PublicApi
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Employee, EmployeeDto>();

        }
    }
}

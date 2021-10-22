using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Assessment.ApplicationCore.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Assessment.ApplicationCore.Entities;
using Assessment.Web.Interfaces;

namespace Microsoft.eShopWeb.Web.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        IEmployeeViewModelService _employeeViewModelService;
        public EmployeeController(IEmployeeViewModelService employeeViewModelService)
        {
            _employeeViewModelService = employeeViewModelService;
        }
    }
}
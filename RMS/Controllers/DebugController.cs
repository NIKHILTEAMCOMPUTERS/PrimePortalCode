using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RMS.BackgroundServices;
using System.Net.NetworkInformation;
using System.Net;

namespace RMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DebugController : ControllerBase
    {
        private readonly EmployeeDataUpdateService _employeeDataService;

        public DebugController(EmployeeDataUpdateService employeeDataService)
        {
            _employeeDataService = employeeDataService;
        }

        [HttpGet("triggerEmployeeUpdate")]
        public IActionResult TriggerEmployeeUpdate()
        {
            _employeeDataService.ManualTrigger();
            return Ok("Employee update triggered manually.");
        }

        [HttpGet("triggerCRMdataUpdate")]
        public IActionResult ManualTriggerforCRM()
        {
            _employeeDataService.ManualTriggerforCRM();
            return Ok("CRM Data triggered manually.");
        }

        
    }
}


// Using a tool like Postman or even just your browser, navigate to the endpoint to trigger the service manually.
// For example, if you're running locally on port 5001, you would visit:
//  https://localhost:5001/api/debug/triggerEmployeeUpdate

// Note:
//Remember that this is for debugging purposes. Before deploying to production:we sould do followings 
//Removing or securing the manual trigger endpoint to ensure it's not exposed to the public.
//Restoring the EmployeeDataUpdateService registration to AddHostedService in Startup.cs if changed it to AddSingleton.
//Using this approach will allow us to inspect the behavior of our background service in a controlled manner.
//    }



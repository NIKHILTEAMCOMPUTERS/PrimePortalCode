using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RMS.Data.Models;
using RMS.Entity.DTO;
using RMS.Extensions;
using RMS.Service;
using RMS.Service.Interfaces;
using System.Security.Claims;

namespace RMS.Controllers.Timesheet
{

    public class TimesheetController : BaseApiController
    {
        private readonly IUnitOfWork _uow;
        public TimesheetController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpPost("CreateHeader/{monthyear}"), Authorize]
        public async Task<IActionResult> PostHeaderAsync(string monthyear)
        {
            //JwtLoginDetailDto Userdetails = new JwtLoginDetailDto();
            //var identity = HttpContext.User.Identity as ClaimsIdentity;


            if (!UtilityMethods.IsValidMonthYearFormat(monthyear))
            {
                return StatusCode(StatusCodes.Status400BadRequest, "The month year format is incorrect");
            }

            if (!(HttpContext.User.Identity is ClaimsIdentity identity))
            {
                return Unauthorized("Authentication Failed");
            }
            var loginDetail = AuthenticUserDetails.GetCurrentUserDetails(identity);


            var result = await _uow.TimesheetRepository.AddHeaderAsync(monthyear, loginDetail);

            return Ok(result);
        }
        [HttpPost("PostDetailItems")]
        public async Task<IActionResult> PostHeaderDetailItemsAsync([FromBody] TimeSheetItemDto value)
        {
            if (!(HttpContext.User.Identity is ClaimsIdentity identity))
            {
                return Unauthorized("Authentication Failed");
            }
            var loginDetail = AuthenticUserDetails.GetCurrentUserDetails(identity);
            var result = await _uow.TimesheetRepository.UpsertHeaderDetailItemsAsync(value, loginDetail);
            return Ok(result);
        }



        [HttpGet("GetHeaderItems")]
        public async Task<IActionResult> GetHeaderItemAsync()
        {
            if (!(HttpContext.User.Identity is ClaimsIdentity identity))
            {
                return Unauthorized("Authentication Failed");
            }
            var loginDetail = AuthenticUserDetails.GetCurrentUserDetails(identity);
            var result = await _uow.TimesheetRepository.GetHeaderAsync(loginDetail);
            return Ok(result);
        }
        [HttpGet("GetDetailListByHeaderId/{id:int}")]
        public async Task<IActionResult> GetListofTimesheetDetailByHeaderid(int id)
        {
            var result = await _uow.TimesheetRepository.GetListofTimesheetDetailByHeaderid(id);
            return Ok(result);
        }

        // public Task<GenericResponse<string>> SubmitTimeSheetDetailItemsAsync(List<int> timesheetDetailIds, JwtLoginDetailDto userdetails);
        //public Task<GenericResponse<string>> DeleteTimeSheetDetailItemsAsync(int timesheetDetailId, JwtLoginDetailDto userdetails);
        [HttpPost("submitTimeSheetDetailItemAsync")]
        public async Task<IActionResult> SubmitListOfTimeSheetDetailByIds([FromBody] List<int> ids)
        {
            if (!(HttpContext.User.Identity is ClaimsIdentity identity))
            {
                return Unauthorized("Authentication Failed");
            }
            var loginDetail = AuthenticUserDetails.GetCurrentUserDetails(identity);
            var result = await _uow.TimesheetRepository.SubmitTimeSheetDetailItemsAsync(ids, loginDetail);
            return Ok(result);
        }

        [HttpPost("DeleteTimeSheetDetailItemAsync/{id:int}")]
        public async Task<IActionResult> DeleteTimeSheetDetailById(int id)
        {
            if (!(HttpContext.User.Identity is ClaimsIdentity identity))
            {
                return Unauthorized("Authentication Failed");
            }
            var loginDetail = AuthenticUserDetails.GetCurrentUserDetails(identity);
            var result = await _uow.TimesheetRepository.DeleteTimeSheetDetailItemsAsync(id, loginDetail);
            return Ok(result);
        }

        [HttpGet("GetTimesheetsByManagerId/{monthYear}/{projectId:int}/{employeeId:int}")]
        public async Task<IActionResult> GetTimesheetByManagerId(string monthYear, int projectId, int employeeId)
        {
            if (!(HttpContext.User.Identity is ClaimsIdentity identity))
            {
                return Unauthorized("Authentication Failed");
            }
            var loginDetail = AuthenticUserDetails.GetCurrentUserDetails(identity);
            var result = await _uow.TimesheetRepository.GetTimesheetByManagerId(monthYear, projectId, employeeId, loginDetail);
            return Ok(result);
        }



        [HttpGet("GetProjectForTimesheet/{Employeeid:int}")]
        public async Task<IEnumerable<Project>> GetProjectForTimesheet(int Employeeid)
        {
            return await _uow.TimesheetRepository.GetProjectsWithDetailsByTimesheet(Employeeid);
        }
        [HttpPost("Manageraction")]
        public async Task<IActionResult> Manageraction(List<TimeSheetItemDto>  obj)
        {
            if (!(HttpContext.User.Identity is ClaimsIdentity identity))
            {
                return Unauthorized("Authentication Failed");
            }
            var loginDetail = AuthenticUserDetails.GetCurrentUserDetails(identity);
            var result = await _uow.TimesheetRepository.Timesheetdecline(obj, loginDetail);
            return Ok(result);
        }
        [HttpGet("categoryofactivity")]
        public async Task<IActionResult> Getcategoryofactivity()
        {
            var result = await _uow.TimesheetRepository.Getcategoryofactivity();
                 return Ok(result);
        }
        [HttpGet("GetTdeTimesheetData/{monthYear}")]
        public async Task<IActionResult> GetTdeTimesheetData(string monthYear)
        {
            var result = await _uow.TimesheetRepository.GetTdeTimesheetCount(monthYear);
            return Ok(result);

        }
    }
}


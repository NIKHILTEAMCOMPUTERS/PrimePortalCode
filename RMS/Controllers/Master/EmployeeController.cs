using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using RMS.Data.Models;
using RMS.Entity.DTO;
using RMS.Extensions;
using RMS.Service.Interfaces;
using System.Security.Claims;
using System.Collections;
using System.Security.Principal;

namespace RMS.Controllers.Master
{
    public class EmployeeController : BaseApiController
    {
        private readonly IUnitOfWork _uow;
        public EmployeeController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpGet]
        public async Task<IEnumerable<employeeResponseDto>> Get()
        {
            //return await _uow.EmployeeRepository.GetAsync();
            return await _uow.EmployeeRepository.GetEmployeesWithDetails();
        }
        //[HttpGet("EmployeeDetailsView/{id?:int}")]
        [HttpGet("EmployeeDetailsView/{id?}")]
        public async Task<IEnumerable<DataForEmployeeReport>> EmployeeDetailsView(int ?id)
        {
            //return await _uow.EmployeeRepository.GetAsync();
            return await _uow.EmployeeRepository.GetEmployeeDetailsView(id);
        }
        [HttpGet("DaList")]
        public async Task<IEnumerable<DaListDto>> DaNameList()
        {
            //return await _uow.EmployeeRepository.GetAsync();
            return await _uow.EmployeeRepository.DaList();
        }


        [HttpGet("GetEmployeesForCount")]
        public async Task<IEnumerable<employeeResponseDto>> GetEmployeesForCount()
        {
            //return await _uow.EmployeeRepository.GetAsync();
            return await _uow.EmployeeRepository.GetEmployeesForCount();
        }

        [HttpGet("EmployeeByDeliveryAnchor/{Id:int}")]
        public async Task<IEnumerable<employeeResponseDto>> GetEmployeeByDeliveryAnchorId(int id)
        {
            var result = await _uow.EmployeeRepository.GetEmployeesOfDA(id); ;
            return result;
        }

        [HttpGet("{id}")]
        public async Task<employeeResponseDto> Get(int id)
        {
            return await _uow.EmployeeRepository.GetEmployeesByIdWithDetails(id);

        }

        //[HttpPost]
        //public async Task<bool> Post([FromBody] Rmsemployee value)
        //{
        //    _uow.CountryRepository.Add(value);
        //    return await _uow.Complete();
        //}

        [HttpPut("{id}"), Authorize]
        public async Task<bool> Put([FromBody] Rmsemployee value)
        {
            _uow.EmployeeRepository.Update(value);
            return await _uow.Complete();
        }

        //[HttpDelete("{id}")]
        //public async Task<bool> Delete(int id)
        //{
        //    _uow.CountryRepository.Delete(id);
        //    return await _uow.Complete();
        //}
        [HttpGet("GetEmployeeByProcedure")]
        public async Task<IActionResult> GetEmployeeByProcedure()
        {
            //return await _uow.EmployeeRepository.GetAsync();
            var Results = await _uow.EmployeeRepository.GetEmployeelistStoredProcedure();
            return Ok(Results);
        }
        [HttpPost("ResignationUpdateAPI"), Authorize]
        public async Task<IActionResult> ResignationUpdateAPI([FromForm] JsonWithFileDto rmsemployee)
        {
            var obj = JsonConvert.DeserializeObject<Rmsemployee>(rmsemployee.JasonData);

            var empId = obj.Employeeid;
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity == null)
                return BadRequest("Authentication Fails");

            var LoginDetails = AuthenticUserDetails.GetCurrentUserDetails(identity);

            if (rmsemployee == null || empId <= 0)
            {
                return BadRequest("Employee Id neither be less than zero or equal to zero/Null");
            }

            var result = await _uow.EmployeeRepository.EmployeeResignedStatus(empId, obj, LoginDetails);
            if (result.responseCode != 200)
            {
                return BadRequest(result);
            }
            else
            {
                return Ok(result);
            }
        }
        [HttpPost("AdeUpdation"), Authorize]
        public async Task<IActionResult> AdeUpdation([FromBody] adeUpdationDto Value)
        {

            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity == null)
                return BadRequest("Authentication Fails");

            var LoginDetails = AuthenticUserDetails.GetCurrentUserDetails(identity);

            if (Value.employeeid == null || Value.employeeid <= 0)
            {
                return BadRequest("Employee Id neither be less than zero or equal to zero/Null");
            }

            var result = await _uow.EmployeeRepository.AdeUpdation(Value, LoginDetails);
            return Ok(result);

        }
        [HttpPost("UpdateEmployeeSubpractice"), Authorize]
        public async Task<IActionResult> UpdateEmployeeSubpractice([FromBody] RequestSubpracticeUpdationDto Value)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity == null)
                return BadRequest("Authentication Fails");

            var LoginDetails = AuthenticUserDetails.GetCurrentUserDetails(identity);

            var result = await _uow.EmployeeRepository.UpdateEmployeeSubpractice(Value, LoginDetails);
            return Ok(result);


        }
        //nick
        [HttpGet("Employees")]
        public async Task<IEnumerable<employeeResponseDto>> GetEmployees()
        {
            return await _uow.EmployeeRepository.GetEmployees();
        }

        [HttpGet("GetAllBillingDataReourceWise")]
        public async Task<IActionResult> GetAllBillingDataReourceWiseNew()
        {
            var result = await _uow.EmployeeRepository.GetAllBillingDataReourceWiseNew();
            return Ok(result);
        }
        [HttpGet("GetProfile"), Authorize]
        public async Task<IActionResult> GetProfile()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity == null)
                return BadRequest("Authentication Fails");

            var LoginDetails = AuthenticUserDetails.GetCurrentUserDetails(identity);
            var result = await _uow.EmployeeRepository.GetProfile(LoginDetails);
            return Ok(result);

        }
        
        [HttpGet("GetEmployeebyskill/{skillid?}")]
        public async Task<IActionResult> GetEmployeebyskill(int skillid)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity == null)
                return BadRequest("Authentication Fails");

            var LoginDetails = AuthenticUserDetails.GetCurrentUserDetails(identity);
            var result = await _uow.EmployeeRepository.GetEmployeesBySkill(skillid);
            return Ok(result);

        }
    }
}
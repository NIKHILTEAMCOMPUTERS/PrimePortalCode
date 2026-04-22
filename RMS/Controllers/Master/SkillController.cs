using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using RMS.Data.Models;
using RMS.Entity.DTO;
using RMS.Extensions;
using RMS.Service.Interfaces;
using System.Security.Claims;

namespace RMS.Controllers.Master
{
    public class SkillController : BaseApiController
    {
        private readonly IUnitOfWork _uow;
        public SkillController(IUnitOfWork uow)
        {
            _uow = uow;
        }
        [HttpGet]
        public async Task<IEnumerable<Skill>> Get()
        {
            return await _uow.SkillRepository.GetSkills();
        }
        [HttpGet("{id}")]
        public async Task<Skill> Get(int id)
        {
            return await _uow.SkillRepository.GetSkillById(id);
        }
        [HttpPost]
        public async Task<bool> Post([FromBody] Skill value)
        {
            _uow.SkillRepository.Add(value);
            return await _uow.Complete();
        }
        [HttpPut("{id}")]
        public async Task<bool> Put([FromBody] Skill value)
        {
            _uow.SkillRepository.Update(value);
            return await _uow.Complete();
        }
        [HttpDelete("{id}")]
        public async Task<bool> Delete(int id)
        {
            _uow.SkillRepository.Delete(id);
            return await _uow.Complete();
        }
        [HttpPost,Route("AddSkillsByProcedure")]
        public async Task<IActionResult> AddSkills(SkillRequestDto Object)
        {
            var result = await _uow.SkillRepository.AddSkill(Object);
            return Ok(result);
        }
        [HttpPost("AddEmployeeSkills"),Authorize]
        public async Task<Response> AssignEmployeeSkill(List<EmployeeskillDto> Value)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (Value == null)
            {
                throw new ArgumentNullException("Value cannot be null.");
            }            
            if (identity == null)
            {
                throw new Exception("Authentication failed.");
            }
            var LoginDetails = AuthenticUserDetails.GetCurrentUserDetails(identity);
            var result = await _uow.SkillRepository.UpsertAssignEmployeeSkill(Value,LoginDetails);
            return result;
        }
        [HttpPut("UpdateEmployeeSkills"), Authorize]
        public async Task<Response> UpdateEmployeeSkill(List<EmployeeskillDto> Value)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (Value == null)
            {
                throw new ArgumentNullException("Value cannot be null.");
            }
            if (identity == null)
            {
                throw new Exception("Authentication failed.");
            }
            var LoginDetails = AuthenticUserDetails.GetCurrentUserDetails(identity);
            var result = await _uow.SkillRepository.UpsertAssignEmployeeSkill(Value, LoginDetails);
            return result;
        }
        [HttpDelete("DeleteEmployeeSkill/{id:int}"),Authorize]
        public async Task<IActionResult> DeleteEmployeeSkill(int id)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            
            if (identity == null)
            {
                throw new Exception("Authentication failed.");
            }
            var LoginDetails = AuthenticUserDetails.GetCurrentUserDetails(identity);
            if (id <=0)
            {
                throw new Exception("Employee Skill can not be zore or less than zero");  

            }
            var result= await _uow.SkillRepository.DeleteEmployeeSkill(id, LoginDetails);
            return Ok(result);  

        }
    }
}
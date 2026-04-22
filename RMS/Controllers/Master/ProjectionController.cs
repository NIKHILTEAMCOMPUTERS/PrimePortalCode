using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RMS.Data.Models;
using RMS.Entity.DTO;
using RMS.Extensions;
using RMS.Service.Interfaces;
using RMS.Service.Repositories.Master;
using System.Security.Claims;
using System.Security.Cryptography.Xml;

namespace RMS.Controllers.Master
{
    public class ProjectionController : BaseApiController
    {
        private readonly IUnitOfWork _uow;
        public ProjectionController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _uow.ProjectionRepository.GetProjectionListAsync();
            return Ok(result);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {           
          var result= await _uow.ProjectionRepository.GetProjectionByIdAsync(id);
            return Ok(result);  
           
        }

        [HttpPost,Authorize]
        public async Task<IActionResult> Post([FromBody] ProjectionRequestResponseDto DtoObject)
        {
            JwtLoginDetailDto Userdetails = new JwtLoginDetailDto();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity == null)
                return BadRequest("Authentication Fails");
            else
            {
                var LoginDetails = AuthenticUserDetails.GetCurrentUserDetails(identity);                
                var result = await _uow.ProjectionRepository.UpsertProjectionAsync(DtoObject, LoginDetails);
                return Ok(result);
               
            }
        }
        [HttpGet("GetEmpBySkillExe")]
        public async Task<IActionResult> GetEmployeeBy_SkillId_Exep([FromBody]Employee_Skill_Exe_Dto Value)
        {
            var result = await _uow.ProjectionRepository.GetEmployeeBy_SkillId_Exep(Value);
            return Ok(result);
        }
        [HttpGet("GetSkillNameExe")]
        public async Task<IActionResult> GetSkillNameExe_Collection()
        {
            var result = await _uow.ProjectionRepository.GetSkillNameExe_Collection();
            return Ok(result);
        }

        [HttpGet("GetEmployeeProjectDetails/{EmployeeId:int}")]
        public async Task<IActionResult> GetEmployeeProjectDetails_For_Projrction(int EmployeeId)
        {
            var result = await _uow.ProjectionRepository.GetEmployeeProjectDetails_For_Projrction(EmployeeId);
            return Ok(result);
        }



        [HttpDelete("{id}")]
        public async Task<bool> Delete(int id)
        {
            _uow.ProjectionRepository.Delete(id);
           
            return await _uow.Complete();
        }

        [HttpGet("ProjectionRequestList/{EmployeeId:int}")]
        public async Task<IActionResult> ProjectionRequestList(int EmployeeId)
        {
            JwtLoginDetailDto Userdetails = new JwtLoginDetailDto();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity == null)
                return BadRequest("Authentication Fails");
            else
            {
                var LoginDetails = AuthenticUserDetails.GetCurrentUserDetails(identity);
                var result = await _uow.ProjectionRepository.GetProjectionRequestList(EmployeeId);
                return Ok(result);

            }
        }
    }
}
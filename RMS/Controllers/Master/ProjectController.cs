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
    public class ProjectController : BaseApiController
    {
        private readonly IUnitOfWork _uow;
        public ProjectController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpGet]
        public async Task<IEnumerable<ResponseForProjectsWithDetailsDto>> Get()
        {
           //return await _uow.ProjectRepository.GetAsync();
            return await _uow.ProjectRepository.GetProjectsWithDetails();
        }


        [HttpGet("{id}")]
        public async Task<ResponseForProjectsWithDetailsDto> Get(int id)
        {
            //return await _uow.ProjectRepository.GetByIdAsync(id);
            return await _uow.ProjectRepository.GetProjectsWithDetailsById(id);
        }

        [HttpGet("ProjectListByDeliveryAnchor/{DeliveryAnchorId:int}")]
        public async Task<IEnumerable<ResponseForProjectsWithDetailsDto>> ProjectListByEmployeeId(int DeliveryAnchorId)
        {
            return await _uow.ProjectRepository.GetProjectsWithDetailsByDA(DeliveryAnchorId);
        }

        [HttpPost("SaveAsDraft"),Authorize]
        public async Task<IActionResult> Post([FromBody] ProjectSaveAsDraftRequestDto DtoObject)
        {
            JwtLoginDetailDto Userdetails = new JwtLoginDetailDto();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity == null)
                return BadRequest("Authentication Fails");
            else
            {
                var LoginDetails = AuthenticUserDetails.GetCurrentUserDetails(identity);                
                var result = await _uow.ProjectRepository.SaveProjectsAsDraftAsync(DtoObject, LoginDetails);
                return Ok(result);
               
            }
        }
        [HttpPost("UpdateProjectDraft/{ProjectId:int}"), Authorize]
        public async Task<IActionResult> UpdateProjectDraft(int ProjectId ,[FromBody] ProjectSaveAsDraftRequestDto value)
        {
            JwtLoginDetailDto Userdetails = new JwtLoginDetailDto();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity == null)
                return BadRequest("Authentication Fails");
            else
            {
                var LoginDetails = AuthenticUserDetails.GetCurrentUserDetails(identity);
                var result = await _uow.ProjectRepository.UpdateProjectAsDeaft(ProjectId, value, LoginDetails);
                return Ok(result);

            }
        }     


        [HttpDelete("{id}")]
        public async Task<bool> Delete(int id)
        {
            _uow.ProjectRepository.Delete(id);
           
            return await _uow.Complete();
        }
        [HttpGet("ProjectListByCustomerId/{CustomerId:int}")]
        public async Task<IActionResult> ProjectListByCustomerId(int CustomerId)
        {
            //return await _uow.ProjectRepository.GetByIdAsync(id);
            var result= await _uow.ProjectRepository.GetListProjectsByCustomerId(CustomerId);

            return Ok(result);  
        }
        [HttpPost("CheckProjectno")]
        public async Task<IActionResult> CheckProjectno([FromBody] ProjectCheckDto DtoObject)
        {

            var resut = await _uow.ProjectRepository.CheckProjectNo(DtoObject.Projectid, DtoObject.Projectno);
            return Ok(resut);
        }
        [HttpGet("GetProjetsByEmployeeId/{id:int}")]
        public async Task<IActionResult> GetProjetsByEmployeeId(int id)
        {
            var result =await _uow.ProjectRepository.GetProjetsByEmployeeId(id);
            return Ok(result);  

        }
        [HttpGet("GetProjetsEmployeesByHeadId/{id}")]
        public async Task<IActionResult> GetProjetsEmployeesByHeadId(string id)
        {
            var result = await _uow.ProjectRepository.GetProjetsEmployeesByHeadId(id);
            return Ok(result);

        }
        [HttpPost("CheckEmployeeActiveProjects")]
        public async Task<IActionResult> CheckEmployeeActiveProjects([FromBody] ActiveProjectRequest request)
        {
            var resut = await _uow.ProjectRepository.CheckEmployeeActiveProjects(request.EmployeeId, request.StartDate, request.EndDate);
            return Ok(resut);
        }

        [HttpPost("EmployeeReleaseRequest"), Authorize]
        public async Task<IActionResult> EmployeeReleaseRequest([FromBody] EmployeeReleaseRequest request)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity == null)
            {
                throw new Exception("Authentication Fails");
            }
            var LoginDetails = AuthenticUserDetails.GetCurrentUserDetails(identity);
            var result = await _uow.ProjectRepository.UpsertEmployeeRequest(request, LoginDetails);
            return Ok(result);
        }
    }
}
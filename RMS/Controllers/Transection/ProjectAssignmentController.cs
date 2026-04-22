using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.Data.Models;
using RMS.Entity.DTO;
using RMS.Extensions;
using RMS.Service.Interfaces;
using System.Security.Claims;

namespace RMS.Controllers.Transection
{
    public class ProjectAssignmentController : BaseApiController
    {
        private readonly IUnitOfWork _uow;
        public ProjectAssignmentController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpGet]
        public async Task<IEnumerable<Projectemployeeassignment>> Get()
        {
            return await _uow.ProjectAssignmentRepository.GetAsync();
        }

        [HttpGet("{id}")]
        public async Task<Projectemployeeassignment> Get(int id)
        {
            return await _uow.ProjectAssignmentRepository.GetByIdAsync(id);
        }

        [HttpPost,Authorize] //Authorize
        public async Task<Response> Post([FromBody] ProjectemployeeassignmentRequestDto value)
        {
            // Validate request body
            if (value == null)
                return new Response { responseCode=400,responseMessage= "Invalid request body." };

            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity == null)
                return new Response { responseCode = 400, responseMessage = "Authorization is fiiled" };

            var LoginDetails = AuthenticUserDetails.GetCurrentUserDetails(identity);

            return await _uow.ProjectAssignmentRepository.AssignEmployeesToProjectAsync(value, LoginDetails);
        }

        [HttpPost("AssignEmployeeProject"),Authorize] //Authorize
        public async Task<Response> AssignEmployeeProject([FromBody] EmployeeAssignDto value)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity == null)
                return new Response { responseCode = 400, responseMessage = "Authorization is failed" };

            var LoginDetails = AuthenticUserDetails.GetCurrentUserDetails(identity);

            return await _uow.ProjectAssignmentRepository.AssignEmployee(value, LoginDetails);
        }

        [HttpPut("{id}")]
        public async Task<bool> Put([FromBody] Projectemployeeassignment value)
        {
            _uow.ProjectAssignmentRepository.Update(value);
            return await _uow.Complete();
        }

        [HttpDelete("{id}")]
        public async Task<bool> Delete(int id)
        {
            _uow.CategoryStatusRepository.Delete(id);
            return await _uow.Complete();
        } 

        [HttpPost("DeleteEmployeeProject"),Authorize] //Authorize
        public async Task<Response> DeleteEmployeeProject([FromBody] ProjectemployeeassignmentRequestDto value)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity == null)
                return new Response { responseCode = 400, responseMessage = "Authorization is failed" };

            var LoginDetails = AuthenticUserDetails.GetCurrentUserDetails(identity);

            return await _uow.ProjectAssignmentRepository.DeleteProjectEmployee(value, LoginDetails);
        }

        [HttpPost("Changeemployeeprojectstatus"), Authorize] //Authorize
        public async Task<Response> Changeemployeeprojectstatus([FromBody] ProjectemployeeassignmentRequestDto value)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity == null)
                return new Response { responseCode = 400, responseMessage = "Authorization is failed" };

            var LoginDetails = AuthenticUserDetails.GetCurrentUserDetails(identity);

            return await _uow.ProjectAssignmentRepository.Changeemployeeprojectstatus(value, LoginDetails);
        }
    }
}
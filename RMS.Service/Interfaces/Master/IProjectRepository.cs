using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RMS.Data.Models;
using RMS.Entity.DTO;
using System.Collections;

namespace RMS.Service.Interfaces.Master
{
    public interface IProjectRepository : IGenericRepository<Project>
    {
        //new chnages 
        public Task<Project> GetByName(string name);
        public  Task<Project> SaveProjectsAsDraftAsync(ProjectSaveAsDraftRequestDto DtoObject, JwtLoginDetailDto LoginDetails);

        public Task<List<ResponseForProjectsWithDetailsDto>> GetProjectsWithDetails();
        public Task<ResponseForProjectsWithDetailsDto> GetProjectsWithDetailsById(int ProjectId);
        public Task<List<ResponseForProjectsWithDetailsDto>> GetProjectsWithDetailsByDA(int EmployeeId);
        public Task<Project> UpdateProjectAsDeaft(int projectId, ProjectSaveAsDraftRequestDto DtoObject, JwtLoginDetailDto LoginDetails);
        public Task<List<ProjectListResponseDto>> GetListProjectsByCustomerId(int CustomerId);
        public Task<bool> CheckProjectNo(int? Projectid, string Projectno);
        public Task<IEnumerable> GetProjetsByEmployeeId(int id);
        public Task<IEnumerable> GetProjetsEmployeesByHeadId(string id);
        public Task<IEnumerable<object>> CheckEmployeeActiveProjects(int employeeid, DateTime startdate, DateTime enddate);
        public Task<GenericResponse<string>> UpsertEmployeeRequest(EmployeeReleaseRequest value, JwtLoginDetailDto logindetails);


    }
}
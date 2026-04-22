using Microsoft.AspNetCore.Http;
using RMS.Data.Models;
using RMS.Entity.DTO;
using System.Collections;

namespace RMS.Service.Interfaces.Master
{
    public interface IProjectionRepository : IGenericRepository<Projection>
    {
        Task<ApiResponse<Projection>> UpsertProjectionAsync(ProjectionRequestResponseDto projection, JwtLoginDetailDto logindetails);
        Task<ApiResponse<List<ProjectionRequestResponseDto>>> GetProjectionListAsync();
        Task<ApiResponse<ProjectionRequestResponseDto>> GetProjectionByIdAsync(int id);
        Task<ApiResponse<object>>GetEmployeeBy_SkillId_Exep(Employee_Skill_Exe_Dto Value);
        Task<ApiResponse<object>> GetSkillNameExe_Collection();
        Task<ApiResponse<object>> GetEmployeeProjectDetails_For_Projrction(int EmployeeId);
        public Task<IEnumerable<object>> GetProjectionRequestList(int EmployeeId);



    }

}

using Microsoft.AspNetCore.Http;
using RMS.Data.Models;
using RMS.Entity.DTO;

namespace RMS.Service.Interfaces.Master
{
    public interface IProjectAssignmentRepository : IGenericRepository<Projectemployeeassignment>
    {
        public Task<Response> AssignEmployeesToProjectAsync(ProjectemployeeassignmentRequestDto requestDto, JwtLoginDetailDto logiondetilas);
        
            public Task<Response> AssignEmployee(EmployeeAssignDto requestDto, JwtLoginDetailDto logiondetilas);
        
             public Task<Response> DeleteProjectEmployee(ProjectemployeeassignmentRequestDto requestDto, JwtLoginDetailDto logiondetilas);
        
             public Task<Response> Changeemployeeprojectstatus(ProjectemployeeassignmentRequestDto requestDto, JwtLoginDetailDto logiondetilas);
    }
}
using RMS.Data.Models;
using RMS.Entity.DTO;

namespace RMS.Service.Interfaces.Master
{
    public interface ISkillRepository : IGenericRepository<Skill>
    {
        public Task<Skill> GetByName(string name);
        public Task<Response> AddSkill(SkillRequestDto obj);
        public Task<Response> UpdateSkill(SkillRequestDto obj);
        public Task<Response> DeleteSkill(SkillRequestDto obj);
        public Task<Skill> GetSkillById(int id);
        public Task<List<Skill>> GetSkills();
        public Task<Response> UpsertAssignEmployeeSkill(List<EmployeeskillDto> Value, JwtLoginDetailDto logindetails);
        public Task<Response> DeleteEmployeeSkill(int id, JwtLoginDetailDto logindetails);

    }
    
}
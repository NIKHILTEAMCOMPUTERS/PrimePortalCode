using RMS.Data.Models;

namespace RMS.Service.Interfaces.RBAC
{
    public interface IRoleRepository : IGenericRepository<Role>
    {
        public Task<Role> GetByName(string name);
        public Task<List<Role>> GetAll();
        public Task<Role> GetById(int roleId);
    }
}

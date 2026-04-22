using Microsoft.EntityFrameworkCore;
using RMS.Data.Models;
using RMS.Service.Interfaces.RBAC;

namespace RMS.Service.Repositories.RBAC
{
    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        private readonly RmsDevContext _context;
        public RoleRepository(RmsDevContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Role>> GetAll()
        {
            return await _context.Roles.AsNoTracking().Include(c=>c.Rolepages).ToListAsync();
        }

        public async Task<Role> GetById(int roleId)
        {
            return await _context.Roles.AsNoTracking().Include(c => c.Rolepages).Where(c=>c.Roleid == roleId).FirstOrDefaultAsync();
        }

        public async Task<Role> GetByName(string name)
        {
            return await _context.Roles.AsNoTracking().Where(c => c.Rolename == name).FirstOrDefaultAsync();
        }
    }
}

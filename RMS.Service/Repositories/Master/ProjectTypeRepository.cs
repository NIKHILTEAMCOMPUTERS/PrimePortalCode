using Microsoft.EntityFrameworkCore;
using RMS.Data.Models;
using RMS.Service.Interfaces.Master;

namespace RMS.Service.Repositories.Master
{
    public class ProjectTypeRepository: GenericRepository<Projecttype>, IProjectTypeRepository
    {
        private readonly RmsDevContext _context;
        public ProjectTypeRepository(RmsDevContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Projecttype> GetByName(string name)
        {
            return await _context.Projecttypes.AsNoTracking().Where(c=>c.Projecttypename == name).FirstOrDefaultAsync();
        }
      
    }
}

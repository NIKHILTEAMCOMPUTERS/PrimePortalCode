using Microsoft.EntityFrameworkCore;
using RMS.Data.Models;
using RMS.Service.Interfaces.Master;

namespace RMS.Service.Repositories.Master
{
    public class ProjectModelRepository : GenericRepository<Projectmodel>, IProjectModelRepository
    {
        private readonly RmsDevContext _context;
        public ProjectModelRepository(RmsDevContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Projectmodel> GetByName(string name)
        {
            return await _context.Projectmodels.AsNoTracking().Where(c=>c.Projectmodelname == name).FirstOrDefaultAsync();
        }
       
      
    }
}

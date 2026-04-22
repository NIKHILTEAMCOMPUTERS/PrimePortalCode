using Microsoft.EntityFrameworkCore;
using RMS.Data.Models;
using RMS.Service.Interfaces.Master;

namespace RMS.Service.Repositories.Master
{
    public class CategoryStatusRepository : GenericRepository<Categorystatus>, ICategoryStatusRepository
    {
        private readonly RmsDevContext _context;
        public CategoryStatusRepository(RmsDevContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Categorystatus> GetByName(string name)
        {
            return await _context.Categorystatuses.AsNoTracking().Where(c=>c.Categorystatusname == name).FirstOrDefaultAsync();
        }
        
    }
}
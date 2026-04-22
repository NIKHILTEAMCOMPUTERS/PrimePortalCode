using Microsoft.EntityFrameworkCore;
using RMS.Data.Models;
using RMS.Service.Interfaces.Master;

namespace RMS.Service.Repositories.Master
{
    public class CategorySubStatusRepository : GenericRepository<Categorysubstatus>, ICategorySubStatusRepository
    {
        private readonly RmsDevContext _context;
        public CategorySubStatusRepository(RmsDevContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Categorysubstatus> GetByName(string name)
        {
            return await _context.Categorysubstatuses.AsNoTracking().Where(c=>c.Categorysubstatusname == name).FirstOrDefaultAsync();
        }
        public async Task<List<Categorysubstatus>> GetSubCategoryByCategoryId(int CategoryId)
        {
            return await _context.Categorysubstatuses.AsNoTracking().Where(c => c.Categorystatusid == CategoryId).ToListAsync();
        }
    }
}
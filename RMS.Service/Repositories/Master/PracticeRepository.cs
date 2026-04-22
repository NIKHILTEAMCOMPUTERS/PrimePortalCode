using Microsoft.EntityFrameworkCore;
using RMS.Data.Models;
using RMS.Service.Interfaces.Master;

namespace RMS.Service.Repositories.Master
{
    public class PracticeRepository : GenericRepository<Practice>, IPracticeRepository
    {
        private readonly RmsDevContext _context;
        public PracticeRepository(RmsDevContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Practice> GetByName(string name)
        {
            return await _context.Practices.AsNoTracking().Where(c=>c.Practicename == name).FirstOrDefaultAsync();
        }
      
    }
}

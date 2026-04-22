using Microsoft.EntityFrameworkCore;
using RMS.Data.Models;
using RMS.Service.Interfaces.Master;

namespace RMS.Service.Repositories.Master
{
    public class SubPracticeRepository : GenericRepository<Subpractice>, ISubpracticeRepository
    {
        private readonly RmsDevContext _context;
        public SubPracticeRepository(RmsDevContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Subpractice> GetByName(string name)
        {
            return await _context.Subpractices.Where(c=>c.Subpracticename == name).FirstOrDefaultAsync();
            
        }
        public async Task<List<Subpractice>> GetSubpracticeByPracticeId(int PracticeId)
        {
            return await _context.Subpractices.Where(s=>s.Practiceid== PracticeId).ToListAsync();
            
        }
    }
}

using Microsoft.EntityFrameworkCore;
using RMS.Data.Models;
using RMS.Service.Interfaces.Master;

namespace RMS.Service.Repositories.Master
{
    public class PreSalesQuestionRepository : GenericRepository<Presalesquestionmaster>, IPreSalesQuestionRepository
    {
        private readonly RmsDevContext _context;
        public PreSalesQuestionRepository(RmsDevContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Presalesquestionmaster> GetByName(string name)
        {
            return await _context.Presalesquestionmasters.AsNoTracking().Where(c=>c.Question == name).FirstOrDefaultAsync();
        }
        public async Task<List<Presalesquestionmaster>> GetQuestionsByPractice(int PracticeId)
        {
            return await _context.Presalesquestionmasters.AsNoTracking().Where(s=>s.Practiceid==PracticeId).ToListAsync();
        }

        
    }
}
using Microsoft.EntityFrameworkCore;
using RMS.Data.Models;
using RMS.Service.Interfaces.Master;

namespace RMS.Service.Repositories.Master
{
    public class StateRepository : GenericRepository<State>, IStateRepository
    {
        private readonly RmsDevContext _context;
        public StateRepository(RmsDevContext context) : base(context)
        {
            _context = context; 
        }

        public Task<State> GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public async Task<List<State>> GetStateByCountryId(int countryId)
        {
            return await _context.States.AsNoTracking().Where(c=>c.Countryid==countryId).ToListAsync();    
                 
        }
    }
}

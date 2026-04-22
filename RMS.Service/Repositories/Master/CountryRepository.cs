using Microsoft.EntityFrameworkCore;
using RMS.Data.Models;
using RMS.Service.Interfaces.Master;

namespace RMS.Service.Repositories.Master
{
    public class CountryRepository : GenericRepository<Country>, ICountryRepository
    {
        private readonly RmsDevContext _context;
        public CountryRepository(RmsDevContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Country> GetByName(string name)
        {
            return await _context.Countries.AsNoTracking().Where(c=>c.Countryname == name).FirstOrDefaultAsync();
        }
    }
}

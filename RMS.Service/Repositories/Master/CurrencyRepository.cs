using Microsoft.EntityFrameworkCore;
using RMS.Data.Models;
using RMS.Service.Interfaces.Master;

namespace RMS.Service.Repositories.Master
{
    public class CurrencyRepository : GenericRepository<Currency>, ICurrencyRepository
    {
        private readonly RmsDevContext _context;
        public CurrencyRepository(RmsDevContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Currency> GetByName(string name)
        {
            return await _context.Currencies.AsNoTracking().Where(c=>c.Currencyname == name).FirstOrDefaultAsync();
        }
    }
}

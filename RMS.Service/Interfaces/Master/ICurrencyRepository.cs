using RMS.Data.Models;

namespace RMS.Service.Interfaces.Master
{
    public interface ICurrencyRepository : IGenericRepository<Currency>
    {
        public Task<Currency> GetByName(string name);
    }
}
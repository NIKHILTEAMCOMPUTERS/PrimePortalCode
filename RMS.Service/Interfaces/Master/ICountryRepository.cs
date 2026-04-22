using RMS.Data.Models;

namespace RMS.Service.Interfaces.Master
{
    public interface ICountryRepository : IGenericRepository<Country>
    {
        public Task<Country> GetByName(string name);
    }
}
using RMS.Data.Models;

namespace RMS.Service.Interfaces.Master
{
    public interface ICityRepository : IGenericRepository<City>
    {
        public Task<City> GetByName(string name);
        public Task<List<City>> GetCitiesByStateId(int StateId);

        public Task<List<CrmAggregatedDataFromAllTable>> GetCRMdataAsyncAsync();
    }
}
using RMS.Data.Models;

namespace RMS.Service.Interfaces.Master
{
    public interface IStateRepository : IGenericRepository<State>
    {
        public Task<State> GetByName(string name);
        public Task<List<State>> GetStateByCountryId(int countryId);
    }
}
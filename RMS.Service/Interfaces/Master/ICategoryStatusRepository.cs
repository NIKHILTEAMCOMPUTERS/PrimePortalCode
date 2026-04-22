using RMS.Data.Models;

namespace RMS.Service.Interfaces.Master
{
    public interface ICategoryStatusRepository : IGenericRepository<Categorystatus>
    {
        public Task<Categorystatus> GetByName(string name);
        
    }
}
using RMS.Data.Models;

namespace RMS.Service.Interfaces.Master
{
    public interface IProjectModelRepository : IGenericRepository<Projectmodel>
    {
        public Task<Projectmodel> GetByName(string name);
        
    }
}
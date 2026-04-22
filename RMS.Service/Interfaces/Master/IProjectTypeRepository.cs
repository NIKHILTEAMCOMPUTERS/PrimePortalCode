using RMS.Data.Models;

namespace RMS.Service.Interfaces.Master
{
    public interface IProjectTypeRepository : IGenericRepository<Projecttype>
    {
        public Task<Projecttype> GetByName(string name);
       
    }
}
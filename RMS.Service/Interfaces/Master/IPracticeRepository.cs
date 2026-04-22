using RMS.Data.Models;

namespace RMS.Service.Interfaces.Master
{
    public interface IPracticeRepository : IGenericRepository<Practice>
    {
        public Task<Practice> GetByName(string name);
       
    }
}
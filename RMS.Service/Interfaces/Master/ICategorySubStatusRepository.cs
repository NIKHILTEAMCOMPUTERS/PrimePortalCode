using RMS.Data.Models;

namespace RMS.Service.Interfaces.Master
{
    public interface ICategorySubStatusRepository : IGenericRepository<Categorysubstatus>
    {
        public Task<Categorysubstatus> GetByName(string name);
        public Task<List<Categorysubstatus>> GetSubCategoryByCategoryId(int CategoryId);


    }
}
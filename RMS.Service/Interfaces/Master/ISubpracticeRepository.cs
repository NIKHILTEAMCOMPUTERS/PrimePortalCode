using RMS.Data.Models;

namespace RMS.Service.Interfaces.Master
{
    public interface ISubpracticeRepository : IGenericRepository<Subpractice>
    {
        public Task<Subpractice> GetByName(string name);
        public Task<List<Subpractice>> GetSubpracticeByPracticeId(int PracticeId);
    }
}
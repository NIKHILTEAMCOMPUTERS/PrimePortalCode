using RMS.Data.Models;

namespace RMS.Service.Interfaces.Master
{
    public interface IPreSalesQuestionRepository : IGenericRepository<Presalesquestionmaster>
    {
        public Task<Presalesquestionmaster> GetByName(string name);
        public Task<List<Presalesquestionmaster>> GetQuestionsByPractice(int StateId);
    }
}
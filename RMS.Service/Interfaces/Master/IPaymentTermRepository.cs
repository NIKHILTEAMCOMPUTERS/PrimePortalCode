using RMS.Data.Models;

namespace RMS.Service.Interfaces.Master
{
    public interface IPaymentTermRepository : IGenericRepository<Paymentterm>
    {
        public Task<Paymentterm> GetByName(string name);
    }
}
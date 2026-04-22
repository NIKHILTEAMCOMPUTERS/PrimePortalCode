using Microsoft.EntityFrameworkCore;
using RMS.Data.Models;
using RMS.Service.Interfaces.Master;

namespace RMS.Service.Repositories.Master
{
    public class PaymentTermRepository : GenericRepository<Paymentterm>, IPaymentTermRepository
    {
        private readonly RmsDevContext _context;
        public PaymentTermRepository(RmsDevContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Paymentterm> GetByName(string name)
        {
            return await _context.Paymentterms.AsNoTracking().Where(c=>c.Paymenttermname == name).FirstOrDefaultAsync();
        }
    }
}

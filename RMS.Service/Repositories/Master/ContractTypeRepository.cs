using Microsoft.EntityFrameworkCore;
using RMS.Data.Models;
using RMS.Service.Interfaces.Master;

namespace RMS.Service.Repositories.Master
{
    public class ContractTypeRepository : GenericRepository<Contracttype>, IContractTypeRepository
    {
        private readonly RmsDevContext _context;
        public ContractTypeRepository(RmsDevContext context) : base(context)
        {
            _context = context;
        }

     
    }
}
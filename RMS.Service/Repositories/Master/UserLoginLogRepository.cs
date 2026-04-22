using Microsoft.EntityFrameworkCore;
using RMS.Data.Models;
using RMS.Entity.Account;
using RMS.Service.Interfaces.Master;

namespace RMS.Service.Repositories.Master
{
    public class UserLoginLogRepository : GenericRepository<Userloginlog>, IUserLoginLogRepository  
    {
        private readonly RmsDevContext _context;
        public UserLoginLogRepository(RmsDevContext context) : base(context)
        {
            _context = context;
        }       
    }
}

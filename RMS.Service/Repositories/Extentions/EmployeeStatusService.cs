using Microsoft.EntityFrameworkCore;
using RMS.Data.Models;
using RMS.Service.Interfaces.Extentions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Service.Repositories.Extentions
{
    public class EmployeeStatusService : IEmployeeStatusService
    {
        private readonly RmsDevContext _context;

        public EmployeeStatusService(RmsDevContext context)
        {
            _context = context;
        }
        public async Task<string> GetEmployeeStatusAsync(int contractEmployeeId)
        {

            var employeeStatusName = await _context.Contractemployees.AsNoTracking()
                                             .Where(x => x.Contractemployeeid == contractEmployeeId)
                                             .Include(x => x.Categorysubstatus)
                                             .ThenInclude(x => x.Categorystatus)
                                             .Select(x => x.Categorysubstatus.Categorysubstatusname)
                                             .FirstOrDefaultAsync();

            return employeeStatusName;
        }
    }
}

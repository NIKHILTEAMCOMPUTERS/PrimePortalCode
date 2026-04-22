using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Service.Interfaces.Extentions
{
    public interface IEmployeeStatusService
    {
        Task<string> GetEmployeeStatusAsync(int contractEmployeeId);
    }
}

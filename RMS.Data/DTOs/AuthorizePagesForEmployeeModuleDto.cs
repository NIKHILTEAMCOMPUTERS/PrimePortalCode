using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Data.DTOs
{
    public class AuthorizePagesForEmployeeModuleDto
    {
        public int? EmployeeId { get; set; }
        public string? UserId { get; set; }
        public string? EmployeeName { get; set; }
        public string? RoleName { get; set; }
        public bool ?IsRowLevel { get; set; }
        public string? ModuleName { get; set; }
        public string? ModuleIcon { get; set; }
        public string? PageName { get; set; }
        public string ?ControllerName { get; set; }
        public string ?ActionName { get; set; }
        public string ?PageIcon { get; set; }
        public bool ?IsReadPermit { get; set; }
        public bool ?IsWritePermit { get; set; }
        public bool ?IsDeletePermit { get; set; }
        public bool ?IsBillingPermit { get; set; }
        public int ?PageSequence { get; set; }
    }
}

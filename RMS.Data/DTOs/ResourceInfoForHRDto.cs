using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Data.DTOs
{
    public class ResourceInfoForHRDto
    {
        public string? TMC { get; set; }
        public string? Practice { get; set; }
        public string? SubPractice { get; set; }
        public string? Region { get; set; }
        public string? Function { get; set; }
        public string? ResourceName { get; set; }
        public int? EmployeeId { get; set; }
        public string? Flag { get; set; }
        public string? BillableNonbillable { get; set; }
        public string? ProjectNames { get; set; }
        public string? ProjectTypes { get; set; }
        public string? CustomerNames { get; set; }

        // ✅ NEW FIELD (matches function output)
        public string? CustomerCodes { get; set; }
        public string? CustomerCompanyNames { get; set; }

        public string? DA { get; set; }
    }

}

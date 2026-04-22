using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Entity.DTO
{
    public class EmployeeDetailsViewDto
    {


    
            public string Tmc { get; set; }
            public string Practice { get; set; }
            public string SubPractice { get; set; }
            public string Region { get; set; }
            public string Function { get; set; }
            public string ResourceName { get; set; }
            public int EmployeeId { get; set; }
            public string Flag { get; set; }
            public string BillableNonBillable { get; set; }
            public string ProjectNames { get; set; }
            public string ProjectTypes { get; set; }
            public string CustomerNames { get; set; }
            public string UserId { get; set; }
            public decimal CurrentMonthBilling { get; set; }
            public bool Ade { get; set; }
            public string DateOfJoining { get; set; } 
            public int WorkExeDays { get; set; }
            public string BillingMonthYear { get; set; }
            public string Exe { get; set; }

    }
}

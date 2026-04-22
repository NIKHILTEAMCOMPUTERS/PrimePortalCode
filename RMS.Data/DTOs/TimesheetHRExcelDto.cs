using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Data.DTOs
{
    public class TimesheetHRExcelDto
    {
        public string? MonthYear { get; set; }
        public string? ReportingHead { get; set; }
        public string? EmployeeName { get; set; }
        public string? EmployeeUserId { get; set; }
        public int DetailCount { get; set; }
    }
}

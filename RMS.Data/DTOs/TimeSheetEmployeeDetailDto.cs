using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Data.DTOs
{
    public class TimeSheetEmployeeDetailDto
    {
        public string? UserId { get; set; }
        public int? EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
        public string? ReportingHead { get; set; }
        public string? ContractNo { get; set; }
        public string? ProjectName { get; set; }
        public string? ProjectTypeName { get; set; }
        public int? ProjectId { get; set; }
        public int? ContractId { get; set; }
        public DateTime? ContractStartDate { get; set; }
        public DateTime? ContractEndDate { get; set; }
        public string? Flag { get; set; }
        public string? ContractStatus { get; set; }
        public string? EmployeeStatus { get; set; }
        public string? CompanyName { get; set; }
        public string? PracticeName { get; set; }
        public string? SubPracticeName { get; set; }
        public string? DeliveryAnchorName { get; set; }
        public string? CategorySubStatusName { get; set; }
        public string? CategoryStatusName { get; set; }
    }
}

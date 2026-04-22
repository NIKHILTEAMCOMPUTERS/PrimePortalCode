using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Data.DTOs
{
    public class EmployeeDetailDto
    {

        public int? EmployeeId { get; set; }
        public string? TmcId { get; set; }
        public string? EmployeeName { get; set; }
        public string? CompanyEmail { get; set; }
        public string? ContactNo { get; set; }
        public string? DepartmentName { get; set; }
        public string? BranchName { get; set; }
        public string? DateOfBirth { get; set; }
        public string? DateOfJoining { get; set; }
        public string? EmpPractice { get; set; }
        public string? EmpSubPractice { get; set; }
        public string? EmpCategorySubStatus { get; set; }
        public string? EmpCategoryStatus { get; set; }
        public string? ReportingHead { get; set; }
        public int? ProjectId { get; set; }
        public string? ProjectName { get; set; }
        public string? ProjectSubPractice { get; set; }
        public string? ProjectPractice { get; set; }
        public string? CompanyName { get; set; }
        public int? ContractId { get; set; }
        public string? ContractNo { get; set; }
        public string? PoNumber { get; set; }
        public DateTime? ContractStartDate { get; set; }
        public DateTime? ContractEndDate { get; set; }
        public decimal? Amount { get; set; }
        public decimal? CurrentMonthBilling { get; set; }
    }
}

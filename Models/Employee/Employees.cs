using Newtonsoft.Json.Linq;
using RMS.Client.Models.Employee;

namespace RMS.Client.Models.Employee
{
    public class Employees
    {
        //employee details 
        public int Employeeid { get; set; }
        public string Experience { get; set; }
        public string TmcId { get; set; }
        public string Employeename { get; set; }
        public string costcenter { get; set; }
        public string email { get; set; }
        public string Contactno { get; set; }


        //public int Designationid { get; set; }
        //public string Designation { get; set; }
        //public string? Reportheadid { get; set; }
        public string Reporthead { get; set; }
        //public int Departmentid { get; set; }
        public string Department { get; set; }
        public int? Branchid { get; set; }
        public string Branch { get; set; }
        public string Dateofbirth { get; set; }
        public string Dateofjoining { get; set; }
        public string PracticeName { get; set; }
        public string SubPracticeName { get; set; }

        public string CategorySubStatusName { get; set; }
        public string CategoryStatusName { get; set; }
        public string companyemail { get; set; }
        public int? userid { get; set; }
        public string? VendorName { get; set; }
        public int? VendorId { get; set; }
        public string? Udf3 { get; set; }    // this is used for employee resign status
        public bool isAde { get; set; }   // for mark trainees
        public bool Ade { get; set; }
        public List<employeeProject> employeeProjects { get; set; }
        public EmployeeSkill PrimarySkill { get; set; }
        public List<EmployeeSkill> SecondarySkills { get; set; }
    }

    public class employeeProject
    {

        //Projectemployeeassignment
        // public int Projectemployeeassignmentid { get; set; }
        public int Projectid { get; set; }
        public int Categorysubstatusid { get; set; }
        //Categorysubstatus    
        // public int Categorystatusid { get; set; }
        public string Categorysubstatusname { get; set; }
        //Categorystatus   
        public string Categorystatusname { get; set; }
        //public string? Categorystatusdescription { get; set; }
        //Project
        public string Projectname { get; set; }
        //public string? Projectdescription { get; set; }
        // public int? Customerid { get; set; }
        //public int? Projecttypeid { get; set; }
        // public int? Subpracticeid { get; set; }
        //Customer
        public string CustomerCompanyname { get; set; }
        // public string? CustomerLastname { get; set; }
        //  public string? CustomerFirstname { get; set; }
        // public string? CustomerCompanylogourl { get; set; }
        //Projecttype
        public string Projecttypename { get; set; }
        //Subpractice       
        public int contractid { get; set; }
        public string Contractno { get; set; }
        public string Subpracticename { get; set; }
        //Practice
        public string Practicename { get; set; } = null!;
        public decimal BillingCosting { get; set; } = 0;
        public string Billingmonthyear { get; set; } = DateTime.Now.ToString("MMM-yy");
        public string DeliveryAnchorName { get; set; } = "";
        public List<Contract> contracts { get; set; }

        public List<ContractBilling> contractbillings { get; set; }
        public DateTime? EffectiveStartDate { get; set; }
        public DateTime? EffectiveEndDate { get; set; }

    }


    public class Contract
    {
        public int? ContractId { get; set; }
        public string? ContractNo { get; set; }
        public string? DeliveryAnchorName { get;set; }
        public int? Statusid { get; set; }
        public DateTime? Contractenddate { get; set; }
        public DateTime? Contractstartdate {  get; set; }
        public DateTime? Lastupdatedate { get; set; }


    }

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


    public class ContractBilling
    {
        public int ContractBillingId { get; set; }
        public string BillingMonthYear { get; set; }
        public decimal Costing { get; set; }
    }
    public class RequestSubpracticeUpdationDto
    {
        public int EmployeeId { get; set; }
        public int SubpracticeId { get; set; }
    }
    public class EmployeeSkill
    {
        public int? EmployeeSkillId { get; set; }
        public int? SkillId { get; set; }
        public string SkillName { get; set; }
        public decimal experinceinmonths { get; set; } = 0;
        public bool IsPrimary { get; set; } = false;
        public decimal? ManagerRating { get; set; }
        public decimal? SelfReting { get; set; }
    }

    public class DaListDto
    {
        public int DaId { get; set; }
        public string DaName { get; set; }
    }
}






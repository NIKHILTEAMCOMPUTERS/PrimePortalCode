using RMS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Entity.DTO
{
   public class employeeResponseDto
    {
        //employee details 
        public int? Employeeid { get; set; }
        public string? Experience { get; set; }
        public string? TmcId { get; set; }
        public string? Employeename { get; set; }
        public string? email { get; set; }
        public string? Contactno { get; set; }

        public string PracticeName { get; set; }
        public string SubPracticeName { get; set; }
        public int? subpracticeid { get; set; }
  
        public string CategorySubStatusName { get; set; }
        public string CategoryStatusName { get; set; }
        public int Designationid { get; set; }


        //public int Designationid { get; set; }
        //public string Designation { get; set; }
        public string? Reportheadid { get; set; }
        public string? Reporthead { get; set; }
        public int Departmentid { get; set; }
        public string Department { get; set; }
        public int? Branchid { get; set; }
        public string Branch { get; set; }
        public string? Dateofbirth { get; set; }
        public string? Dateofjoining { get; set; }
        public string costcenter { get; set; }
        public int? Udf2 { get; set; }
        public string? Udf3 { get; set; }
        public List<employeeProject> employeeProjects { get; set; }
        public List<employeeRole> Role { get; set; }
        public Subpractice? Subpractice { get; set; }
        public Vendor? Vendor { get; set; }
        public string? currentbill { get; set; }
        public  bool Ade { get; set; }
        public Skill_Employee_Info? PrimarySkill{ get; set; }
        public List<Skill_Employee_Info>? SecondarySkills { get; set; }

    }
    public class Skill_Employee_Info
    {
        public int Employeeskillid { get; set; }
        public int Skillid { get; set; }
        public string Skillname { get; set; }
        public decimal? Experinceinmonths { get; set; }
        public bool? IsPrimary { get; set; } 
        public decimal? Managerrating { get; set; }
        public decimal? Selfreting { get; set; }
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
        public string? Projectname { get; set; }
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
        // public int? Practiceid { get; set; }
        public string Subpracticename { get; set; }
        //Practice
        public string Practicename { get; set; } = null!;
        public DateTime ProjectStartDate { get; set; }

        //added for billing purpose 
        public decimal BillingCosting { get; set; } = 0;
        public List<ContractbillingInfo> Contractbillings { get; set; } = new List<ContractbillingInfo>();
        public string Billingmonthyear { get; set; } = DateTime.Now.ToString("MMM-yy");
        public string DeliveryAnchorName { get; set; } = "";
        public List<Contractinfo> contracts { get; set; } = new List<Contractinfo>();

        public DateTime? EffectiveStartDate { get; set; }
        public DateTime? EffectiveEndDate { get; set; }

    }
    public class Contractinfo
    {
        public int Contractid { get; set; }
        public string ContractNo { get; set; }
        public string? DeliveryAnchorName { get; set; }       
        public string? Ponumber { get; set; }
        public DateTime? Contractstartdate { get; set; }
        public DateTime? Contractenddate { get; set; }
        public decimal? Amount { get; set; }
        public string? Contactpersonname { get; set; }
        public int ?Deliveryanchorid { get; set; }
        public decimal? Povalue { get; set; }
        public int? Statusid { get; set; }
        public List<ContractemployeeInfo> Contractemployees { get; set; } = new List<ContractemployeeInfo>();
        public DateTime? Lastupdatedate { get; set; }
    }
    public partial class ContractemployeeInfo
    {
        public int Contractemployeeid { get; set; }
        public int? Categorysubstatusid { get; set; }
        public List<ContractbillingInfo> Contractbillings { get; set; } = new List<ContractbillingInfo>();

    }
    public partial class ContractbillingInfo
    {
        public int Contractbillingid { get; set; }

        public string? Billingmonthyear { get; set; }

        public decimal? Costing { get; set; }


    }

    public class employeeRole
    {
        public int RoleId { get; set; }
        public string Role { get; set; }
    }

    public class adeUpdationDto
    {
        public int? employeeid { get; set; }
        public bool? IsAde { get; set; }
    }
    public class RequestSubpracticeUpdationDto
    {        
        public int EmployeeId { get; set; }
        public int SubpracticeId { get; set; }
    }

}

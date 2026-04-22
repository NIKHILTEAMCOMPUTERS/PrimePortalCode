using Microsoft.AspNetCore.Http;
using RMS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Entity.DTO
{
    public class OafDto
    {
        public int? Oafid { get; set; }
        public bool? Isextended { get; set; }
        public string? Ponumber { get; set; }
        public string? Povalue { get; set; }
        public string? Orderdescription { get; set; }
        public string? Potermscondition { get; set; }
        public int Customerid { get; set; }
        //added for response
        public CustomerInfo Customer { get; set; }
        public string? Projectname { get; set; }
        public string? Projectmodel { get; set; }
        public int? Projecttypeid { get; set; }
        //added for response
        public string? Projecttype { get; set; }
        public int? Subpracticeid { get; set; }
        //added for response
        public string? Subpractice { get; set; }

        public int? Practiceid { get; set; }
        //added for response
        public string? Practice { get; set; }

        public string? Projectdescription { get; set; }
        public string? Contractno { get; set; }
        public DateTime? Contractstartdate { get; set; }
        public DateTime? Contractenddate { get; set; }
        public string? Clientcoordinator { get; set; }
        public string? Milestones { get; set; }
        public int? Costsheetid { get; set; }
        //added for response
        public string? CostsheetName { get; set; }
        public string? Emailattachment { get; set; }
        public string? Proposalattachment { get; set; }
        public string? Poattachment { get; set; }
        public string? Costattachment { get; set; }
        public int? Statusid { get; set; }
        //added for response
        public string? Status { get; set; }
        public string? Remarks { get; set; }
        public int? Deliveryanchorid { get; set; }
        public decimal? Xvalue { get; set; }
        //added for response
        public string? CreatedBy { get; set; }
        //added for response
        public DateTime? CreatedDate { get; set; }
        public int? Accountmanagerid { get; set; }
        public string? Accountmanager { get; set; }
        public decimal? Advanceamount { get; set; }
        public decimal? Advancepercent { get; set; }    

        public int? CommittedClientBillingDate { get; set; }

        public int? ContractId { get; set; }
        public bool IsResourceDeployed { get; set; }

        public List<OaflineInfo> ?Oaflines { get; set; }
        public List<OafchecklistInfo>? Oafchecklists { get; set; }
        public List<MilestonInfoDto>? Milestonlist { get; set; }
        public ActiveContractInfo ActiveContract { get; set; }

        //OAF extended istory 
        public int? HistoryId { get; set; }       
    }
    public class OaflineInfo
    {
        public int? Oaflineid { get; set; }
        public string? Lineno { get; set; }
        public string? Linedescription1 { get; set; }
        public string? Linedescription2 { get; set; }
        public decimal? Lineamount { get; set; }
    }

    public class OafchecklistInfo
    {
        public int? Oafchecklistid { get; set; }
        public string? Question { get; set; }
        public string? Clientresponse { get; set; }
        public bool? Isextra { get; set; }
        public int? Statusid { get; set; }
        public  string? Status { get; set; }
        public string? Remarks { get; set; }
    }
    public class JsonWithFilesAofDto
    {
        public string JasonData { get; set; }
        public IFormFile Emailattachment { get; set; }
        public IFormFile Proposalattachment { get; set; }
        public IFormFile Poattachment { get; set; }
        public IFormFile Costattachment { get; set; }
    }
    public  class MilestonInfoDto
    {
        public int? Id { get; set; }

        public int? Oafid { get; set; }

        public string Milestonedesc { get; set; } = null!;

        public decimal? Percentage { get; set; }

        public decimal? Amount { get; set; }
        public int? Days { get; set; }
    }
    public partial class OafResponseDto
    {
        public int Oafid { get; set; }

        public string? Ponumber { get; set; }

        public string? Povalue { get; set; }

        public string? Orderdescription { get; set; }

        public string? Potermscondition { get; set; }

        public int Customerid { get; set; }

        public string? Projectname { get; set; }

        public string? Projectmodel { get; set; }

        public int? Projecttypeid { get; set; }

        public int? Subpracticeid { get; set; }

        public string? Projectdescription { get; set; }

        public string? Contractno { get; set; }

        public DateTime? Contractstartdate { get; set; }

        public DateTime? Contractenddate { get; set; }

        public string? Clientcoordinator { get; set; }

        public string? Milestones { get; set; }

        public int? Costsheetid { get; set; }

        public string? Emailattachment { get; set; }

        public string? Proposalattachment { get; set; }

        public string? Poattachment { get; set; }

        public int? Statusid { get; set; }

        public string? Remarks { get; set; }

        public int? Deliveryanchorid { get; set; }

        public DateTime Createddate { get; set; }

        public int Createdby { get; set; }

        public decimal? Xvalue { get; set; }

        public string? Costattachment { get; set; }

        public virtual CostsheetInfo? Costsheet { get; set; }

        public virtual CustomerInfo Customer { get; set; } = null!;

        public virtual Rmsemployee? Deliveryanchor { get; set; }

        public virtual ICollection<Oafchecklist> Oafchecklists { get; set; } = new List<Oafchecklist>();

        public virtual ICollection<Oaflineinfo> Oaflines { get; set; } = new List<Oaflineinfo>();

        public virtual ProjecttypeInfo? Projecttype { get; set; }

        public virtual StatusInfo? Status { get; set; }

        public virtual Subpractice? Subpractice { get; set; }
        public virtual ActiveContractInfo? ActiveContract { get; set; }
    }
    public partial class CostsheetInfo
    {
        public int Costsheetid { get; set; }
        public string Costsheetname { get; set; }
        public virtual ICollection<CostsheetdetailInfo> Costsheetdetails { get; set; } = new List<CostsheetdetailInfo>();


    }
    public partial class CostsheetdetailInfo
    {
        public int Costsheetdetailid { get; set; }

        public int Costsheetid { get; set; }

        public int Skillid { get; set; }

        public string? Skillexperince { get; set; }

        public int? Requiredresource { get; set; }

        public decimal? Skillcost { get; set; }

        public virtual SkillInfo Skill { get; set; } = null!;
    }
    public partial class SkillInfo
    {
        public int Skillid { get; set; }

        public string Skillname { get; set; } = null!;

        public virtual ICollection<SkillcostingInfo> Skillcostings { get; set; } = new List<SkillcostingInfo>();

        public virtual ICollection<SkilltagInfo> Skilltags { get; set; } = new List<SkilltagInfo>();
    }
    public partial class SkillcostingInfo
    {
        public int Skillcostingid { get; set; }

        public int? Skillid { get; set; }

        public string? Expname { get; set; }

        public int? Fromexpmonth { get; set; }

        public int? Toexpmonth { get; set; }

        public decimal? Amount { get; set; }
    }
    public partial class SkilltagInfo
    {
        public int Tagid { get; set; }
        public string Tagname { get; set; } = null!;
        public int? Skillid { get; set; }
    }
    public partial class CustomerInfo
    {
        public string Companyname { get; set; }
        public string? Companylogourl { get; set; }
        public string? Companyemail { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
    }
  
    public partial class Oaflineinfo
    {
        public int Oaflineid { get; set; }

        public int Oafid { get; set; }

        public string? Lineno { get; set; }

        public string? Linedescription1 { get; set; }

        public string? Linedescription2 { get; set; }

        public decimal? Lineamount { get; set; }


    }
    public partial class ProjecttypeInfo
    {
        public int Projecttypeid { get; set; }

        public string Projecttypename { get; set; } = null!;


    }
    public partial class StatusInfo
    {
        public int Statusid { get; set; }

        public string? Statusname { get; set; }

        public string? Statusdiscription { get; set; }

        public int? Statuscode { get; set; }

    }
    public partial class ActiveContractInfo
    {
        public string Contrantno { get; set; }
        public int? Contractid { get; set; }
        public DateTime? Contractstartdate { get; set; }
        public DateTime? Contractenddate { get; set; }
        public int? StatusId { get; set; }
        public string? Status { get; set; }
        
        public List<ResourceInfo> Resources { get; set; }   
    }
    public class ResourceInfo
    {
        public int Employeeid { get; set; }
        public string Tmc { get; set; }
        public string Employeename { get; set; }
    }

}
       
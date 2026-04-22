using RMS.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Entity.DTO
{

    public partial class ContractRequestDto
    {
        public int? Contractid { get; set; }

        public string? Contractno { get; set; }

        public string? Ponumber { get; set; }

        public int? Projectid { get; set; }

        public DateTime? Contractstartdate { get; set; }

        public DateTime? Contractenddate { get; set; }

        public decimal? Amount { get; set; }
      

        public string? Contactpersonname { get; set; }

        public int? Statusid { get; set; }

        public int? Costsheetid { get; set; }

        public string? Contactnumber { get; set; }

        public string? Remarks { get; set; }

        public int? Deliveryanchorid { get; set; }
        public  string? DeliveryAnchorName{ get; set; }

        public string? Attachment { get; set; }
        

        public string? Invoiceperiod { get; set; }

        public string? Ctype { get; set; }

        public int? Contracttypeid { get; set; }
        public string?CustomerCompanyName { get; set; }

        public List<Extendedcontract>? extendedcontracts { get; set; }
        public List<ContractlineInfo>? Contractlines { get; set; }

        public List<ContractQuestionsInfo>? Questions { get; set; }
        public List<Oaf>? Oaf { get; set; }



        //Extra
        public string? AttachmentURL { get; set; }
        public string StatusName { get; set; }
        //public DeliveryanchorInfo? DeliveryanchorInfo { get; set; }
        public ProjectInfo? ProjectInfo { get; set; }
        public List<ContractEmployee>? ContractEmployees { get; set; }
        public bool isExtended { get; set; } = false;
        public int? OldContractId { get; set; }
        public bool isProjection { get; set; } = false;
        public bool? Isprojectestimationdone { get; set; }

    }

    public partial class ContractlineInfo
    {
        public int Contractlineid { get; set; }

        public int? Contractid { get; set; }

        public string? Lineno { get; set; }

        public string? Linedescription1 { get; set; }

        public string? Linedescription2 { get; set; }

        public decimal? Lineamount { get; set; }
    }
    public partial class ContractQuestionsInfo
    {
        public int Responseid { get; set; }

        public int Contractid { get; set; }

        public string? response { get; set; }

        public int? Statusid { get; set; }
        public string StatusName { get; set; }

        public string? Question { get; set; }

        public string? Refresponse { get; set; }

        public string? Remarks { get; set; }
        public bool? IsExtra { get; set; }

    }


    public class DeliveryanchorInfo
    {
        public string DeliveryAnchorName { get; set;  }
    }

    public class ProjectInfo
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string SubPracticeName { get; set; }
        public string PracticeName { get; set; }
        public string ProjectType { get; set; }
        public string CustomerName { get; set; }
        public string Email { get; set; }
        public string ContactNo { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string CustomerLogo { get; set; }
        public int CustomerId { get; set; }
        public int? CommittedClientBillingDate { get; set; }
    }

    public class ContractEmployee
    {
        public int? EmployeeId { get; set; }
        public int Contractemployeeid { get; set; }
        public string EmployeeName { get; set; }
        public string TmcCode { get; set; }
        public string ?PracticeName { get; set; }
        public  string? EmployeeSataus { get; set; }
        public List<InitialBillinginfo>? InitialBillings { get; set; }
        public List<ContractBillinginfo>? ContractBillings { get; set; }
        public List<ContractBillingProvisionalInfo>? ContractBillingsProvisional { get; set; }
    }

    public class ContractBillinginfo
    {
        public int ContractBillingId { get; set; }
        public int? Contractemployeeid { get; set; }
        public string BillingMonthYear { get; set; }
        public decimal? Costing { get; set; }
        public DateTime? Expecteddate { get; set; }
        public string? BillingStatus { get; set; }
    }
    public class InitialBillinginfo
    {
        public int Contractbillingid { get; set; }

        public int? Contractemployeeid { get; set; }

        public string? BillingMonthYear { get; set; }

        public decimal? Costing { get; set; }
       
    }


    public class ContractBillingProvisionalInfo
    {
        public int Contractbillingprovesionid { get; set; }
        public int Contractemployeeid { get; set; }
        public string? Billingmonthyear { get; set; }
        public decimal? Costing { get; set; }
        public int? StatusId { get; set; }
        public  string? Status { get; set; }
        public DateTime? Expecteddate { get; set; }
        public string? BillingStatus { get; set; }
    }

    

    public class ContractEmpBillingDto
    {
        
        public int Contractemployeeid { get; set; }        
        public string BillingMonthYear { get; set; }     
        public decimal Costing { get; set; }
        public DateTime? Exptectedbillingdate { get; set; } // 01/12/2
        public bool? isRevised { get; set; }
        

    }
    public class ApproverActionDto
    {

        public int ?Contractbillingprovesionid { get; set; }
        public int StatusId { get; set; }
        public string? Remark { get; set; }


    }

    public class RequestforBillingDetailsDto
    {
        public string Mode { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }

    public class ResponseForBillingDetailsDto
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string CustomerName { get; set; }
        public string ProjectName { get; set; }
        public string PracticeName { get; set; }
        public string? PoNumber { get; set; }
        public string SubPracticeName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string MonthYear { get; set; }
        public decimal? Cost { get; set; }
        public string? DeliveryAnchor { get; set; }
        public decimal? ProvisionCost { get; set; }
    }
    public class MonthYearWithBillingData
    {
        public string MonthYear { get; set; }
    }


    public class ContractCheckDto
    {
        public int Contractid { get; set; }
        public string? Contractno { get; set; }
        public DateTime?Inputdate { get; set; }
    }
    public class PoCheckDto
    {
        public int Contractid { get; set; }
        public string? PoNo { get; set; }
    }
    public class BillingStatusUpdateRequestDto
    {
        public int contractbillingid { get; set; }
        public bool? isBilled { get; set; }
        public bool? isTobeBilled { get; set; }
        public DateTime? Exptectedbillingdate { get; set; }
    }
    public class ForeclosureInputDTO
    {
        public int ContractId { get; set; }
        public DateTime ForeClosureDate { get; set; }
    }


}





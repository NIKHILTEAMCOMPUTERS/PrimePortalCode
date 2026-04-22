using RMS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Entity.DTO
{
    public class BillingApprvalDto
    {
        public int? Contractbillingid { get; set; }
        public decimal? Costing { get; set; }
        public DateTime? Exptectedbillingdate { get; set; } // 01/12/2
        public bool? IsRevised { get; set; }
        public int ? StatusId { get; set;}
        public string? Remarks { get; set; }

    }

    public class ProvisionBillingApprovalDataDto
    {

        //project details 
        public string ProjectName { get; set; }
        public string PoNumber { get; set; }
        public string ContractNumber { get; set; }
        public string DeliveryAnchor { get; set; }
        public string Status { get; set; }

        //ContractBillingDetails

        public int ?Contractbillingprovesionid { get; set; }
        public int Contractemployeeid { get; set; }
        public int ContractId { get; set; }
        public string? ResourceName { get;set; }
        public string? TmcId { get; set; }        
        public string? MonthYear { get; set; }
        public decimal? ProvisionAmount { get; set; }
        public decimal? ActualAmount{ get; set; }
        public DateTime? EstimatedBilligDate { get; set; }
      //  public string? Status { get; set; }
        public string? RemarkByDH { get; set; }
        public string? RemarkByBH { get; set; }
    }
    public class RevisionBillingDto
    {
        public int? ContractEmployeeId { get; set; }
        public string? BillingMonthYear { get; set; }
        public decimal? Costing { get; set; }
        public bool? isRevised { get; set; }
    }
    public class BillingDropdownMapperDto
    {
        public string Text { get; set; }
        public string Value { get; set; }
    }
    public class rptContractBillingActualMapperDto
    {
        public int? contractbillingid { get; set; }
        public int? EmpId { get; set; }
        public string? TMC { get; set; }
        public string? ResourceName { get; set; }
        public string? CCname { get; set; }
        public string? ProjectType { get; set; }
        public string? CustomerName { get; set; }
        public string? DAname { get; set; }
        public string? SPname { get; set; }
        public string? Pname { get; set; }
        public string? Cno { get; set; }
        public string? POno { get; set; }
        public DateTime? contractstartdate { get; set; }
        public DateTime? contractenddate { get; set; }
        //public string? Duration { get; set; }
        // public decimal? actualbilling { get; set; }
        public string? invoiceperiod { get; set; }
        public decimal? billing { get; set; }
        public string? projectname { get; set; }
        public string? projectno { get; set; }
        public string? billingmonth { get; set; }
        public int? contractemployeeid { get; set; }
        public bool? isbilled { get; set; }
        public bool? istobebilled { get; set; }
        public DateTime? estimatedbillingdate { get; set; }
        public string?  BillingStatus { get; set; }
        public string? DocumentUrl { get; set; }
        public  int? StatusId { get; set; }


    }

    public class ActualBillingApprovalDataDto
    {

        //project details 
        public string ProjactName { get; set; }
        public string PoNumber { get; set; }
        public string ContractNumber { get; set; }
        public string DeliveryAnchor { get; set; }
        public string Status { get; set; }
        public  string PracticeName { get; set; }

        //ContractBillingDetails

        public int? Contractbillingid { get; set; }
        public int? Contractemployeeid { get; set; }
        public int ContractId { get; set; }
        public string? ResourceName { get; set; }
        public string? TmcId { get; set; }
        public string? ProjectName { get; set; }
        public string? MonthYear { get; set; }
        public decimal? Billing { get; set; }
        public DateTime? EstimatedBilligDate { get; set; }
       
        public string? RemarkByDH { get; set; }
        public string? RemarkByBH { get; set; }
        public int? RevisionNo { get; set; }
        public DateTime? CreatedOn { get; set; }
        public decimal? OldBilling { get; set; }
        public DateTime? OldEstimatedBilligDate { get; set; }
        public DateTime? ApproverActionTakenon { get; set; }
    }
    public class SwappingRequestDto
    {
        public int? contractbillingid { get; set; }
        public int? contrtactbillingprovesionid { get; set; }
        public string? BillingMonthyear { get; set; }
        public decimal? costing { get; set; }
        public DateTime? billingdate { get; set; }
    }
    public class GetEmployeeActualBillinDto
    {
        public int? contractEmployeeId { get; set; }       
        public string? monthyear { get; set; }     
       
    }



}

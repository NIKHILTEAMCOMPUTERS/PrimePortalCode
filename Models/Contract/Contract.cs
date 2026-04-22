using Microsoft.AspNetCore.Identity;
using RMS.Client.Models.Employee;
using System.Globalization;

namespace RMS.Client.Models.Contract
{
    public partial class ContractRequestDto
    {
        public int Contractid { get; set; }

        public string? Contractno { get; set; }

        public string? Ponumber { get; set; }
        public string? customerCompanyName { get; set; }

        public int? Projectid { get; set; }

        public DateTime Contractstartdate { get; set; }

        //public DateTime? _ContractStartDate { get { return string.IsNullOrEmpty(this.Contractstartdate) ? null : DateTime.ParseExact(this.Contractstartdate, "dd/MM/yyyy", CultureInfo.InvariantCulture); } }

        public DateTime Contractenddate { get; set; }

        //public DateTime? _ContractEndDate { get { return string.IsNullOrEmpty(this.Contractstartdate) ? null : DateTime.ParseExact(this.Contractenddate, "dd/MM/yyyy", CultureInfo.InvariantCulture); } }
        public decimal? Amount { get; set; }

        public string? Contactpersonname { get; set; }

        public int? Statusid { get; set; }

        public int? Costsheetid { get; set; }

        public string? Contactnumber { get; set; }

        public string? Remarks { get; set; }

        public int? Deliveryanchorid { get; set; }

        public string? Attachment { get; set; }


        public string? Invoiceperiod { get; set; }

        public string? Ctype { get; set; }
        public string deliveryAnchorName { get; set; }
        public int? Contracttypeid { get; set; }
        public List<Oafcostsheet> Oaf { get; set; }
        public List<ExtendedContract>? extendedcontracts { get; set; }
        public List<ContractlineInfo>? Contractlines { get; set; }

        public List<ContractQuestionsInfo>? Questions { get; set; }
        



        //Extra
        public string? AttachmentURL { get; set; }
        public string StatusName { get; set; }
        public DeliveryanchorInfo? DeliveryanchorInfo { get; set; }
        public ProjectInfo? ProjectInfo { get; set; }
        public List<ContractEmployee>? ContractEmployees { get; set; }
        public int? OldContractId { get; set; }
        public bool isExtended { get; set; } = false;
        public bool isProjection { get; set; }
        public decimal? unlocatedamount { get; set; }
        public List<Employees> GetEmployee { get; set; }
        public bool? Isprojectestimationdone { get; set; }

    }
    public class Oafcostsheet
    {
        public int? oafid { get; set; }
        public int? costsheetid { get; set; }
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
        public string DeliveryAnchorName { get; set; }
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
        public int customerId { get; set; }
        public int? committedClientBillingDate { get; set; }
    }

    public class ContractEmployee
    {
        public int? EmployeeId { get; set; }
        public int ContractEmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string TmcCode { get; set; }
        public string PracticeName { get; set; }
        public string? employeeSataus { get; set; }
        public List<ContractBillinginfo>? ContractBillings { get; set; }
        public List<ContractBillingProvisionals>? contractBillingsProvisional { get; set; }
        public List<InitialBilling>? initialBillings { get; set; }
    }
    public class InitialBilling
    {
        public int ContractBillingId { get; set; }
        public int ContractEmployeeId { get; set; }
        public string BillingMonthYear { get; set; }
        public decimal Costing { get; set; }
    }
    public class ContractBillinginfo
    {
        
        public int ContractBillingId { get; set; }
        public int? Contractemployeeid { get; set; }
        public string BillingMonthYear { get; set; }
        public decimal? Costing { get; set; }
        public DateTime? estimatedDate { get; set; }
        public DateTime? Expecteddate { get; set; }
        public string? billingStatus { get; set;}



    }
    public class ContractBillingProvisionals
    {
        public int? ContractBillingProvisionId { get; set; }
        public int? Contractemployeeid { get; set; }
        public string BillingMonthYear { get; set; }
        public decimal? Costing { get; set; }
        public DateOnly? estimatedDate { get; set; }
        public DateTime? Exptectedbillingdate { get; set; }
        
        public bool? isRevised { get; set; }
        public int? statusId { get; set; }
        public string? status {  get; set; }
        public DateTime? Expecteddate { get; set; }
       


    }
    public class ContractBillingProvision
    {
        public int contractbillingprovesionid { get; set; }
        public int ContractEmployeeId { get; set; }
        public string ResourceName { get; set; }
        public string ProjectName { get; set; }
        public string MonthYear { get; set; }
        public decimal ProvisionAmount { get; set; }
        public DateTime? estimatedBilligDate { get; set; }
        public string Status { get; set; }
        public string RemarkByDH { get; set; }
        public string RemarkByBH { get; set; }
        public int? contractId {  get; set; }
        public string? TmcId { get; set; }
    }
    public class ApprovalRequestItem
    {
        public int contractbillingid { get; set; }
        public decimal costing { get; set; }
        public string exptectedbillingdate { get; set; }
        public string? remarks {  get; set; }
        public string? statusId { get; set; }
        public bool IsRevised { get; set; } = false;
    }
    public class ContractData
    {
        public KeyData Key { get; set; }
        public List<ValueData> Value { get; set; }
    }

    public class KeyData
    {
        public string ProjactName { get; set; }
        public string? projectName { get; set; }
        public string PoNumber { get; set; }
        public string ContractNumber { get; set; }
        public string DeliveryAnchor { get; set; }
        public int ContractId { get; set; }
        public int Contractbillingprovesionid { get; set; }
        public int Contractemployeeid { get; set; }
        public string ResourceName { get; set; }
        public string MonthYear { get; set; }
        public decimal ProvisionAmount { get; set; }
        public DateTime? EstimatedBilligDate { get; set; }
        public string Status { get; set; }
        public object RemarkByDH { get; set; }
        public object RemarkByBH { get; set; }
    }

    public class ValueData
    {
        public string ProjactName { get; set; }
        public string PoNumber { get; set; }
        public string ContractNumber { get; set; }
        public string DeliveryAnchor { get; set; }
        public string Status { get; set; }
        public int Contractbillingprovesionid { get; set; }
        public int Contractemployeeid { get; set; }
        public int ContractId { get; set; }
        public string ResourceName { get; set; }
        public string ProjectName { get; set; }
        public string MonthYear { get; set; }
        public decimal ProvisionAmount { get; set; }
        public DateTime? EstimatedBilligDate { get; set; }
        public object RemarkByDH { get; set; }
        public object RemarkByBH { get; set; }
        public string? TmcId { get; set; }
    }
    public class ExtendedContract
    {
        public int ExtensionId { get; set; }
        public int ExtendedContractId { get; set; }
        public int OldContractId { get; set; }
        public string LastUpdatedBy { get; set; }
        public string CreatedBy { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}

namespace RMS.Client.Models.Master
{
    public class ProvisionManagementItem
    {
        public int ProvisionId { get; set; }
        public string ResourceName { get; set; }
        public string TmcId { get; set; }
        public string CustomerName { get; set; }
        public string ProjectName { get; set; }
        public string ContractNo { get; set; }
        public string BillingMonthYear { get; set; }
        public decimal ProvisionAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal RemainingAmount { get; set; }
        public int CarryForwardCount { get; set; }
        public string ProvisionStatus { get; set; }
        public int? CarryForwardFromId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? EstimatedBillingDate { get; set; }
        // Provision Report columns
        public string? AccountManager { get; set; }
        public string? ProjectType { get; set; }
        public string? DeliveryAnchor { get; set; }
        public string? InvoicePeriod { get; set; }
        public DateTime? ContractStartDate { get; set; }
        public DateTime? ContractEndDate { get; set; }
        // Financial Report columns
        public decimal? ToplineBillingAmt { get; set; }
        public string? Products { get; set; }
        public string? Practice { get; set; }
        public string? PoNumber { get; set; }
        public string? ProjectNo { get; set; }
        // CF target (where this provision's remaining went after carry-forward)
        public string? CarriedForwardToMonth { get; set; }
        public DateTime? CarriedForwardToDate { get; set; }
        public bool HasHistory { get; set; }
        public bool IsTerminalTarget { get; set; }
    }

    public class ProvisionHistoryItem
    {
        public int HistoryId { get; set; }
        public int ProvisionId { get; set; }
        public string ActionType { get; set; }
        public string ActionBy { get; set; }
        public string? Remark { get; set; }
        public string? OldValues { get; set; }
        public string? NewValues { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class ProvisionActionRequest
    {
        public int ProvisionId { get; set; }
        public string? Remark { get; set; }
        public string? TargetMonth { get; set; }
        public string? AttachmentPath { get; set; }
    }

    public class ProvisionEditRequest
    {
        public int ProvisionId { get; set; }
        public DateTime? EstimatedBillingDate { get; set; }
        public string? Remark { get; set; }
    }

    public class UpdatePaidAmountRequest
    {
        public int ProvisionId { get; set; }
        public decimal PaidAmount { get; set; }
        public string? Remark { get; set; }
        public string? RemainingAction { get; set; }   // "none" | "carryforward" | "nullify"
        public string? CarryForwardDate { get; set; }  // yyyy-MM-dd, used when RemainingAction == "carryforward"
    }
}

namespace RMS.Entity.DTO
{
    public class TeamPendingProvisionListDto
    {
        public int ProvisionId { get; set; }
        public int? TeamTrackingId { get; set; }

        // From existing provision + joins
        public string ResourceName { get; set; } = "";
        public string TmcId { get; set; } = "";
        public string CustomerName { get; set; } = "";
        public string ProjectName { get; set; } = "";
        public string ContractNo { get; set; } = "";
        public string PoNumber { get; set; } = "";
        public string BillingMonthYear { get; set; } = "";
        public decimal ProvisionAmount { get; set; }
        public string ProvisionStatus { get; set; } = "";
        public DateTime CreatedDate { get; set; }
        public int DaysPending { get; set; }

        // Report extras
        public string? AccountManager { get; set; }
        public string? DeliveryAnchor { get; set; }
        public string? Practice { get; set; }
        public string? Products { get; set; }
        public string? ProjectType { get; set; }

        // Team tracking (from team_provision_tracking)
        public DateTime? CloserDate { get; set; }
        public string? DocumentNo { get; set; }
        public decimal BilledAmount { get; set; }
        public decimal RemainingAmount => ProvisionAmount - BilledAmount;
        public bool HasHistory { get; set; }

        // Team-level status
        public string TeamStatus =>
            BilledAmount >= ProvisionAmount ? "Settled" :
            BilledAmount > 0 ? "Partial" : "Pending";
    }

    public class TeamUpdateCloserDateDto
    {
        public int ProvisionId { get; set; }
        public DateTime? CloserDate { get; set; }
        public string? Remark { get; set; }
    }

    public class TeamUpdateDocumentNoDto
    {
        public string MonthYear { get; set; } = "";
        public string PoNumber { get; set; } = "";
        public string? DocumentNo { get; set; }
    }

    public class TeamRecordBilledDto
    {
        public int ProvisionId { get; set; }
        public decimal BilledAmount { get; set; }
        public string? Remark { get; set; }
    }

    public class TeamProvisionHistoryDto
    {
        public int Id { get; set; }
        public int ProvisionId { get; set; }
        public string ActionType { get; set; } = "";
        public string ActionBy { get; set; } = "";
        public string? OldValues { get; set; }
        public string? NewValues { get; set; }
        public string? Remark { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}

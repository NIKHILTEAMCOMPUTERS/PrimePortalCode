namespace RMS.Client.Models.Contract
{
    public class Billing
    {
        public string code { get; set; }
        public string name { get; set; }
        public string CustomerName { get; set; }
        public string ProjectName { get; set; }
        public string PracticeName { get; set; }
        public string SubPracticeName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string monthYear { get; set; }
        public decimal? cost { get; set; }
        public decimal? ProvisionCost { get; set; }
        
        public string deliveryAnchor { get; set; }
    }
    // FOR Actual BILLING LIST DH
    public class ActualBillingRevisions
    {
        public Key key { get; set; }
        public List<ProjectValue> value { get; set; }
    }

    public class Key
    {
        public string projectName { get; set; }
        public string ProjactName { get; set; }
        public string PoNumber { get; set; }
        public string ContractNumber { get; set; }
        public string DeliveryAnchor { get; set; }
        public int ContractId { get; set; }
    }

    public class ProjectValue
    {
        public string ProjactName { get; set; }
        public string PoNumber { get; set; }
        public string ContractNumber { get; set; }
        public string DeliveryAnchor { get; set; }
        public string Status { get; set; }
        public int Contractbillingid { get; set; }
        public int Contractemployeeid { get; set; }
        public int ContractId { get; set; }
        public string ResourceName { get; set; }
        public string TmcId { get; set; }
        public string ProjectName { get; set; }
        public string MonthYear { get; set; }
        public decimal Billing { get; set; }
        public DateTime EstimatedBilligDate { get; set; }
        public string RemarkByDH { get; set; }
        public string RemarkByBH { get; set; }
        public int RevisionNo { get; set; }
        public DateTime CreatedOn { get; set; }
        public decimal OldBilling { get; set; }
        public DateTime? OldEstimatedBilligDate { get; set; }
        public DateTime ApproverActionTakenon { get; set; }
    }

}

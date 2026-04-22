namespace RMS.Client.Models.Master
{

    public class ItemDetail
    {
        public int RevisionNumber { get; set; }
        public decimal ProvisionAmount { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class RevisionHistory
    {
        public string Tmc { get; set; }
        public string ResourceName { get; set; }
        public string ProjectName { get; set; }
        public string ProjectNo { get; set; }
        public string ContractNo { get; set; }
        public int ContractId { get; set; }
        public string PoNumber { get; set; }
        public string DeliveryAnchor { get; set; }
        public DateTime EstimatedBillingDate { get; set; }
        public List<ItemDetail> ItemDetails { get; set; }
    }
}

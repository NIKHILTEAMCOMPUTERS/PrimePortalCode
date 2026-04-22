namespace RMS.Client.Models.Master
{
    public class PaymentTerm
    {
        public int PaymentTermId { get; set; }
        public string PaymentTermName { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public int CreatedBy { get; set; }
        public int LastUpdateBy { get; set; }

    }
}

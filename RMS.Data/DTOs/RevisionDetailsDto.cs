using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Data.DTOs
{
    public class RevisionDetailsDto
    {
        public string? TMC { get; set; }
        public string? ResourceName { get; set; }
        public string? ProjectName { get; set; }
        public string? ProjectNo { get; set; }
        public string? ContractNo { get; set; }
        public int? ContractId { get; set; }
        public string? PONumber { get; set; }
        public int? RevisionNumber { get; set; }
        public decimal? ProvisionAmount { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? EstimatedBillingDate { get; set; } // Nullable to allow for null values
        public string? DeliveryAnchor { get; set; }
        public string? ApprovalStatus { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Data.DTOs
{
    public class OafListDto
    {
        public int? OafId { get; set; }
        public string? Ponumber { get; set; }
        public string? Povalue { get; set; }
        public string? OrderDescription { get; set; }
        public string? PoTermsCondition { get; set; }
        public int? CustomerId { get; set; }
        public string Customername { get; set; }
        public string? ProjectName { get; set; }
        public string? ProjectModel { get; set; }
        public int? ProjectTypeId { get; set; }
        public int? SubPracticeId { get; set; }
        public string? ProjectDescription { get; set; }
        public string? ContractNo { get; set; }
        public DateTime? ContractStartDate { get; set; }
        public DateTime? ContractEndDate { get; set; }
        public string? ClientCoordinator { get; set; }
        public string? Milestones { get; set; }
        public int? CostSheetId { get; set; }
        public string? EmailAttachment { get; set; }
        public string? ProposalAttachment { get; set; }
        public string? PoAttachment { get; set; }
        public int? StatusId { get; set; }
        public string? Remarks { get; set; }
        public int? DeliveryAnchorId { get; set; }
        public string? Udf1 { get; set; }
        public string? Udf2 { get; set; }
        public string? Udf3 { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public int? CreatedBy { get; set; }
        public int? LastUpdateBy { get; set; }
        public decimal? XValue { get; set; }
        public string? CostAttachment { get; set; }
        public string? OafNo { get; set; }
        public decimal? AdvanceAmount { get; set; }
        public decimal? AdvancePercent { get; set; }
        public int? AccountManagerId { get; set; }
        public int? CommittedClientBillingDate { get; set; }
        public bool? IsExtended { get; set; }
        public int? RevisionNumber { get; set; }
        public string? CreatedByName { get; set; }
        public string? Status { get; set; }
        public string? PracticeName { get; set; }
    }
}

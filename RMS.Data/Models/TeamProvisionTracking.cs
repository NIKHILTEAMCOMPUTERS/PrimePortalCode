using System;

namespace RMS.Data.Models;

public partial class TeamProvisionTracking
{
    public int Id { get; set; }
    public int ProvisionId { get; set; }
    public DateTime? CloserDate { get; set; }
    public string? DocumentNo { get; set; }
    public decimal BilledAmount { get; set; }
    public string? Remark { get; set; }
    public bool IsDeleted { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public int LastUpdatedBy { get; set; }
    public DateTime LastUpdatedDate { get; set; }

    public virtual Contractbillingprovesion Provision { get; set; } = null!;
    public virtual ICollection<TeamProvisionHistory> Histories { get; set; } = new List<TeamProvisionHistory>();
}

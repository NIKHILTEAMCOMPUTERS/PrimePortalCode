using System;

namespace RMS.Data.Models;

public partial class TeamProvisionHistory
{
    public int Id { get; set; }
    public int? TeamTrackingId { get; set; }
    public int ProvisionId { get; set; }
    public string ActionType { get; set; } = "";
    public string ActionBy { get; set; } = "";
    public string? OldValues { get; set; }
    public string? NewValues { get; set; }
    public string? Remark { get; set; }
    public bool IsDeleted { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }

    public virtual TeamProvisionTracking? TeamTracking { get; set; }
}

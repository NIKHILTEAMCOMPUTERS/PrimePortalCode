using System;
using System.Collections.Generic;

namespace RMS.Data.Models;

public partial class Contractbillingactualhistory
{
    public int Historyid { get; set; }

    public int? Contractbillingid { get; set; }

    public int Revisionnumber { get; set; }

    public string? Remark { get; set; }

    public bool? Isactive { get; set; }

    public bool Isdeleted { get; set; }

    public DateTime Createddate { get; set; }

    public DateTime Lastupdatedate { get; set; }

    public int Createdby { get; set; }

    public decimal? Costing { get; set; }

    public decimal? Oldcosting { get; set; }

    public DateTime? Estimatedbillingdate { get; set; }

    public string? Approveraction { get; set; }

    public int? Statusid { get; set; }

    public string? Billingmonthyear { get; set; }

    public DateTime? Oldestimatedbillingdate { get; set; }

    public int? Lastupdatedby { get; set; }

    public virtual Contractbilling? Contractbilling { get; set; }

    public virtual Rmsemployee CreatedbyNavigation { get; set; } = null!;

    public virtual Rmsemployee? LastupdatedbyNavigation { get; set; }

    public virtual Status? Status { get; set; }
}

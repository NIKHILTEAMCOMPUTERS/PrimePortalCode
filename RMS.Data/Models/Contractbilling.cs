using System;
using System.Collections.Generic;

namespace RMS.Data.Models;

public partial class Contractbilling
{
    public int Contractbillingid { get; set; }

    public int? Contractemployeeid { get; set; }

    public string? Billingmonthyear { get; set; }

    public decimal? Costing { get; set; }

    public bool? Isactive { get; set; }

    public bool Isdeleted { get; set; }

    public DateTime Createddate { get; set; }

    public DateTime Lastupdatedate { get; set; }

    public int Createdby { get; set; }

    public int Lastupdateby { get; set; }

    public bool? Isbilled { get; set; }

    public bool? Istobebilled { get; set; }

    public DateTime? Estimatedbillingdate { get; set; }

    public string? Documenturl { get; set; }

    public bool? Isrevised { get; set; }

    public int? Statusid { get; set; }

    public string? Remark { get; set; }

    public bool? Isswaped { get; set; }

    public bool? Isfromprovision { get; set; }

    public DateTime? Swapingdate { get; set; }

    public virtual ICollection<ContractbillingprovesionToContractbillingHistory> ContractbillingprovesionToContractbillingHistories { get; set; } = new List<ContractbillingprovesionToContractbillingHistory>();

    public virtual Contractemployee? Contractemployee { get; set; }

    public virtual Status? Status { get; set; }
}

using System;
using System.Collections.Generic;

namespace RMS.Data.Models;

public partial class ContractbillingprovesionToContractbillingHistory
{
    public int Id { get; set; }

    public int? Contractbillingprovesionid { get; set; }

    public int? Contractbillingid { get; set; }

    public int? Contractemployeeid { get; set; }

    public string? Billingmonthyear { get; set; }

    public decimal? ProvisionCosting { get; set; }

    public decimal? ActualCosting { get; set; }

    public DateTime? ProvisionEstimatedbillingdate { get; set; }

    public DateTime? ActualEstimatedbillingdate { get; set; }

    public bool? Isactive { get; set; }

    public bool Isdeleted { get; set; }

    public DateTime Createddate { get; set; }

    public DateTime Lastupdatedate { get; set; }

    public int Createdby { get; set; }

    public int Lastupdateby { get; set; }

    public virtual Contractbilling? Contractbilling { get; set; }

    public virtual Contractbillingprovesion? Contractbillingprovesion { get; set; }

    public virtual Contractemployee? Contractemployee { get; set; }
}

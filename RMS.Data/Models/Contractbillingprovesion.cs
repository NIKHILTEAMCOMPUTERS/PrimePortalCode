using System;
using System.Collections.Generic;

namespace RMS.Data.Models;

public partial class Contractbillingprovesion
{
    public int Contractbillingprovesionid { get; set; }

    public int Contractemployeeid { get; set; }

    public string? Billingmonthyear { get; set; }

    public decimal? Costing { get; set; }

    public DateTime? EstimatedBillingDate { get; set; }

    public bool? Isactive { get; set; }

    public bool Isdeleted { get; set; }

    public DateTime Createddate { get; set; }

    public DateTime Lastupdatedate { get; set; }

    public int Createdby { get; set; }

    public int Lastupdateby { get; set; }

    public bool? Isrevised { get; set; }

    public int? Statusid { get; set; }

    public bool? Isbilled { get; set; }

    public decimal? Recievedbillingamount { get; set; }

    public bool? Istobebilled { get; set; }

    public string? Documenturl { get; set; }

    public bool? Isswaped { get; set; }

    public bool? Isfromactual { get; set; }

    public DateTime? Swapingdate { get; set; }

    public virtual ICollection<ContractbillingprovesionToContractbillingHistory> ContractbillingprovesionToContractbillingHistories { get; set; } = new List<ContractbillingprovesionToContractbillingHistory>();

    public virtual ICollection<Contractbillingprovisionhistory> Contractbillingprovisionhistories { get; set; } = new List<Contractbillingprovisionhistory>();

    public virtual Contractemployee Contractemployee { get; set; } = null!;

    public virtual ICollection<ProbillapprlOld> ProbillapprlOlds { get; set; } = new List<ProbillapprlOld>();

    public virtual ICollection<Probillapprl> Probillapprls { get; set; } = new List<Probillapprl>();

    public virtual Status? Status { get; set; }
}

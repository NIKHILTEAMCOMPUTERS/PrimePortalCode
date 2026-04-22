using System;
using System.Collections.Generic;

namespace RMS.Data.Models;

public partial class Contractbillingprovisionhistory
{
    public int Historyid { get; set; }

    public int? Contractbillingprovesionid { get; set; }

    public int? Probillapprlid { get; set; }

    public int? Probillapprldetailid { get; set; }

    public int Revisionnumber { get; set; }

    public bool? Isrevised { get; set; }

    public string? Remark { get; set; }

    public bool? Isactive { get; set; }

    public bool Isdeleted { get; set; }

    public DateTime Createddate { get; set; }

    public DateTime Lastupdatedate { get; set; }

    public int Createdby { get; set; }

    public int Lastupdateby { get; set; }

    public decimal? Costing { get; set; }

    public DateTime? Estimatedbillingdate { get; set; }

    public string? Approveraction { get; set; }

    public int? Statusid { get; set; }

    public decimal? Oldcosting { get; set; }

    public virtual Contractbillingprovesion? Contractbillingprovesion { get; set; }

    public virtual Probillapprl? Probillapprl { get; set; }

    public virtual Probillapprldetail? Probillapprldetail { get; set; }

    public virtual Status? Status { get; set; }
}

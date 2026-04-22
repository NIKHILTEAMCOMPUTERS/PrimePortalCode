using System;
using System.Collections.Generic;

namespace RMS.Data.Models;

public partial class Initialsplitcontractbilling
{
    public int Contractbillingid { get; set; }

    public int? Contractemployeeid { get; set; }

    public string? Billingmonthyear { get; set; }

    public decimal? Costing { get; set; }

    public bool? Isactive { get; set; }

    public bool Isdeleted { get; set; }

    public DateTime Createddate { get; set; }

    public int Createdby { get; set; }

    public int Lastupdateby { get; set; }

    public virtual Contractemployee? Contractemployee { get; set; }
}

using System;
using System.Collections.Generic;

namespace RMS.Data.Models;

public partial class Tempcontractbilling
{
    public string? Contractbillingid { get; set; }

    public string? Contractemployeeid { get; set; }

    public string? Billingmonthyear { get; set; }

    public string? Costing { get; set; }

    public string? Isactive { get; set; }

    public string? Isdeleted { get; set; }

    public string? Createddate { get; set; }

    public string? Lastupdatedate { get; set; }

    public string? Createdby { get; set; }

    public string? Lastupdateby { get; set; }

    public string? Isbilled { get; set; }

    public string? Istobebilled { get; set; }

    public string? Estimatedbillingdate { get; set; }

    public string? Documenturl { get; set; }
}

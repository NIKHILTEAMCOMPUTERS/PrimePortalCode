using System;
using System.Collections.Generic;

namespace RMS.Data.Models;

public partial class TempContract
{
    public string? Contractno { get; set; }

    public string? Ponumber { get; set; }

    public string? Projectno { get; set; }

    public int? Projectid { get; set; }

    public string? Contractstartdate { get; set; }

    public string? Contractenddate { get; set; }

    public string? Amount { get; set; }

    public int? Contracttypeid { get; set; }

    public string? Invoiceperiod { get; set; }

    public string? Serviceperiod { get; set; }

    public string? Type { get; set; }
}

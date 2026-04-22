using System;
using System.Collections.Generic;

namespace RMS.Data.Models;

public partial class TempContractbilling1
{
    public string? Tmc { get; set; }

    public string? Contractno { get; set; }

    public string? Billingmonthyear { get; set; }

    public decimal? Costing { get; set; }

    public int? Employeeid { get; set; }

    public int? Contractid { get; set; }

    public int? Contractemployeeid { get; set; }
}

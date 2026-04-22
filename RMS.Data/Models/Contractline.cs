using System;
using System.Collections.Generic;

namespace RMS.Data.Models;

public partial class Contractline
{
    public int Contractlineid { get; set; }

    public int? Contractid { get; set; }

    public string? Lineno { get; set; }

    public string? Linedescription1 { get; set; }

    public string? Linedescription2 { get; set; }

    public decimal? Lineamount { get; set; }

    public virtual Projectcontract? Contract { get; set; }
}

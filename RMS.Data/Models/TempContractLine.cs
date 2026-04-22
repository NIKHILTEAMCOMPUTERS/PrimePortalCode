using System;
using System.Collections.Generic;

namespace RMS.Data.Models;

public partial class TempContractLine
{
    public string? Contractno { get; set; }

    public int? Contractid { get; set; }

    public string? Lineno { get; set; }

    public string? Linedescription1 { get; set; }

    public string? Linedescription2 { get; set; }

    public string? Lineamount { get; set; }

    public int Id { get; set; }
}

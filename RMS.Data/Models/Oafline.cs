using System;
using System.Collections.Generic;

namespace RMS.Data.Models;

public partial class Oafline
{
    public int Oaflineid { get; set; }

    public int Oafid { get; set; }

    public string? Lineno { get; set; }

    public string? Linedescription1 { get; set; }

    public string? Linedescription2 { get; set; }

    public decimal? Lineamount { get; set; }

    public virtual Oaf Oaf { get; set; } = null!;
}

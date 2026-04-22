using System;
using System.Collections.Generic;

namespace RMS.Data.Models;

public partial class Extendedcontract
{
    public int Extensionid { get; set; }

    public int Extendedcontractid { get; set; }

    public int Oldcontractid { get; set; }

    public string? Lastupdatedby { get; set; }

    public string? Createdby { get; set; }

    public DateTime? Lastupdatedate { get; set; }

    public DateTime? Createddate { get; set; }

    public virtual Projectcontract Oldcontract { get; set; } = null!;
}

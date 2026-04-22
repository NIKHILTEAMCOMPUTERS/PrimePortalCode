using System;
using System.Collections.Generic;

namespace RMS.Data.Models;

public partial class Contracttype
{
    public int Contracttypeid { get; set; }

    public string Contracttypename { get; set; } = null!;

    public string? Description { get; set; }

    public bool? Isactive { get; set; }

    public bool Isdeleted { get; set; }

    public DateTime Createddate { get; set; }

    public DateTime Lastupdatedate { get; set; }

    public int Createdby { get; set; }

    public int Lastupdateby { get; set; }
}

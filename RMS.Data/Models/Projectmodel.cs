using System;
using System.Collections.Generic;

namespace RMS.Data.Models;

public partial class Projectmodel
{
    public int Projectmodelid { get; set; }

    public string Projectmodelname { get; set; } = null!;

    public int? Statusid { get; set; }

    public bool? Isactive { get; set; }

    public bool Isdeleted { get; set; }

    public DateTime Createddate { get; set; }

    public DateTime Lastupdatedate { get; set; }

    public int Createdby { get; set; }

    public int Lastupdateby { get; set; }

    public virtual Status? Status { get; set; }
}

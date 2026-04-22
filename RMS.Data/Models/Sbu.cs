using System;
using System.Collections.Generic;

namespace RMS.Data.Models;

public partial class Sbu
{
    public int Sbuid { get; set; }

    public string Sbucode { get; set; } = null!;

    public string? Sbudesc { get; set; }

    public bool? Isactive { get; set; }

    public bool Isdeleted { get; set; }

    public DateTime Createddate { get; set; }

    public DateTime Lastupdatedate { get; set; }

    public int Createdby { get; set; }

    public int Lastupdateby { get; set; }

    public virtual ICollection<Rmsemployee> Rmsemployees { get; set; } = new List<Rmsemployee>();
}

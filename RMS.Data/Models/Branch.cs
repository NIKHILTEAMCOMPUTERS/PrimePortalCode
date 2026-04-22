using System;
using System.Collections.Generic;

namespace RMS.Data.Models;

public partial class Branch
{
    public int Branchid { get; set; }

    public string Branchcode { get; set; } = null!;

    public string? Branchname { get; set; }

    public bool? Isactive { get; set; }

    public bool Isdeleted { get; set; }

    public DateTime Createddate { get; set; }

    public DateTime Lastupdatedate { get; set; }

    public int Createdby { get; set; }

    public int Lastupdateby { get; set; }

    public virtual ICollection<Rmsemployee> Rmsemployees { get; set; } = new List<Rmsemployee>();
}

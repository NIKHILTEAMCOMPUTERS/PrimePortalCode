using System;
using System.Collections.Generic;

namespace RMS.Data.Models;

public partial class Designation
{
    public int Designationid { get; set; }

    public string Designationname { get; set; } = null!;

    public bool? Isactive { get; set; }

    public bool Isdeleted { get; set; }

    public DateTime Createddate { get; set; }

    public DateTime Lastupdatedate { get; set; }

    public int Createdby { get; set; }

    public int Lastupdateby { get; set; }

    public virtual ICollection<Rmsemployee> Rmsemployees { get; set; } = new List<Rmsemployee>();
}

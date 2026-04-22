using System;
using System.Collections.Generic;

namespace RMS.Data.Models;

public partial class Templatedetail
{
    public int Templatedetailid { get; set; }

    public int Templateid { get; set; }

    public int Departmentid { get; set; }

    public int Projectid { get; set; }

    public string Activity { get; set; } = null!;

    public string Hours { get; set; } = null!;

    public string? Remarks { get; set; }

    public bool? Isactive { get; set; }

    public DateTime Createddate { get; set; }

    public DateTime Lastupdatedate { get; set; }

    public int Createdby { get; set; }

    public int Lastupdateby { get; set; }

    public virtual Department Department { get; set; } = null!;

    public virtual Oldproject Project { get; set; } = null!;

    public virtual Template Template { get; set; } = null!;
}

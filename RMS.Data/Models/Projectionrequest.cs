using System;
using System.Collections.Generic;

namespace RMS.Data.Models;

public partial class Projectionrequest
{
    public int Projectionrequestid { get; set; }

    public int Projectionid { get; set; }

    public int Employeeid { get; set; }

    public string? Remarks { get; set; }

    public string? Status { get; set; }

    public int Requestsentby { get; set; }

    public int Requestsentto { get; set; }

    public int? Lastupdatedby { get; set; }

    public DateTime? Lastupdatedate { get; set; }

    public DateTime? Createddate { get; set; }

    public virtual Projection Projection { get; set; } = null!;
}

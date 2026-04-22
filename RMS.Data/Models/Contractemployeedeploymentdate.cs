using System;
using System.Collections.Generic;

namespace RMS.Data.Models;

public partial class Contractemployeedeploymentdate
{
    public int Id { get; set; }

    public int Contractemployeeid { get; set; }

    public DateTime? Startdate { get; set; }

    public DateTime? Enddate { get; set; }

    public bool? Isactive { get; set; }

    public bool Isdeleted { get; set; }

    public DateTime Createddate { get; set; }

    public DateTime Lastupdatedate { get; set; }

    public int Createdby { get; set; }

    public int Lastupdateby { get; set; }

    public int? Categorysubstatusid { get; set; }

    public virtual Categorysubstatus? Categorysubstatus { get; set; }

    public virtual Contractemployee Contractemployee { get; set; } = null!;
}

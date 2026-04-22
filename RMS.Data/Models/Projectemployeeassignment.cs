using System;
using System.Collections.Generic;

namespace RMS.Data.Models;

public partial class Projectemployeeassignment
{
    public int Projectemployeeassignmentid { get; set; }

    public int Employeeid { get; set; }

    public int Projectid { get; set; }

    public int Categorysubstatusid { get; set; }

    public DateTime? Startdate { get; set; }

    public DateTime? Enddate { get; set; }

    public int? Statusid { get; set; }

    public bool? Isactive { get; set; }

    public bool Isdeleted { get; set; }

    public DateTime Createddate { get; set; }

    public DateTime Lastupdatedate { get; set; }

    public int Createdby { get; set; }

    public int Lastupdateby { get; set; }

    public virtual Categorysubstatus Categorysubstatus { get; set; } = null!;

    public virtual Rmsemployee Employee { get; set; } = null!;

    public virtual Project Project { get; set; } = null!;

    public virtual Status? Status { get; set; }
}

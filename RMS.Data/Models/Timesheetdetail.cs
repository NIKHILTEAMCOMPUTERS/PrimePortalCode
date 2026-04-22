using System;
using System.Collections.Generic;

namespace RMS.Data.Models;

public partial class Timesheetdetail
{
    public int Timesheetdetailid { get; set; }

    public int Timesheetid { get; set; }

    public int Departmentid { get; set; }

    public int? Contractid { get; set; }

    public string Activity { get; set; } = null!;

    public decimal? Dayhours { get; set; }

    public string? Remarks { get; set; }

    public bool? Isdrafted { get; set; }

    public bool? Isactive { get; set; }

    public bool? Isdeleted { get; set; }

    public DateTime? Createddate { get; set; }

    public DateTime? Lastupdatedate { get; set; }

    public int? Createdby { get; set; }

    public int? Lastupdateby { get; set; }

    public DateTime Timesheetdate { get; set; }

    public string? Benchstatus { get; set; }

    public int? Categoryofactivityid { get; set; }

    public virtual Categoryofactivity? Categoryofactivity { get; set; }

    public virtual Projectcontract? Contract { get; set; }

    public virtual Department Department { get; set; } = null!;

    public virtual Timesheetheader Timesheet { get; set; } = null!;
}

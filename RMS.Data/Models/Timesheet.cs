using System;
using System.Collections.Generic;

namespace RMS.Data.Models;

public partial class Timesheet
{
    public int Timesheetid { get; set; }

    public int Employeeid { get; set; }

    public DateTime Timesheetdate { get; set; }

    public string? Totalhours { get; set; }

    public bool Isdrafted { get; set; }

    public bool? Isactive { get; set; }

    public DateTime Createddate { get; set; }

    public DateTime Lastupdatedate { get; set; }

    public int Createdby { get; set; }

    public int Lastupdateby { get; set; }

    public string? Timesheethistoryid { get; set; }

    public int? Timesheetmonthid { get; set; }

    public virtual Rmsemployee Employee { get; set; } = null!;

    public virtual ICollection<Timesheetdetail> Timesheetdetails { get; set; } = new List<Timesheetdetail>();

    public virtual Timesheetmaster? Timesheetmonth { get; set; }
}

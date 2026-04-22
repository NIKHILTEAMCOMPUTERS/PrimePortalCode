using System;
using System.Collections.Generic;

namespace RMS.Data.Models;

public partial class Timesheetmaster
{
    public int Tsmasterid { get; set; }

    public int Employeeid { get; set; }

    public string Tsuniqueid { get; set; } = null!;

    public bool? Isactive { get; set; }

    public bool Isdeleted { get; set; }

    public DateTime Createddate { get; set; }

    public DateTime Lastupdatedate { get; set; }

    public int Createdby { get; set; }

    public int Lastupdateby { get; set; }

    public string? Monthname { get; set; }

    public virtual Rmsemployee Employee { get; set; } = null!;

    public virtual ICollection<Timesheet> Timesheets { get; set; } = new List<Timesheet>();
}

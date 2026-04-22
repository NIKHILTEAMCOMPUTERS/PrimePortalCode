using System;
using System.Collections.Generic;

namespace RMS.Data.Models;

public partial class Timesheetheader
{
    public int Timesheetid { get; set; }

    public string Monthyear { get; set; } = null!;

    public int Employeeid { get; set; }

    public bool? Isactive { get; set; }

    public bool Isdeleted { get; set; }

    public DateTime Createddate { get; set; }

    public DateTime Lastupdatedate { get; set; }

    public int Createdby { get; set; }

    public int Lastupdateby { get; set; }

    public virtual Rmsemployee Employee { get; set; } = null!;

    public virtual ICollection<Timesheetdetail> Timesheetdetails { get; set; } = new List<Timesheetdetail>();
}

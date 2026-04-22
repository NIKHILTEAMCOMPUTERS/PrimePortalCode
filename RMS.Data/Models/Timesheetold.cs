using System;
using System.Collections.Generic;

namespace RMS.Data.Models;

public partial class Timesheetold
{
    public int Timesheetid { get; set; }

    public int Employeeid { get; set; }

    public DateTime Timesheetdate { get; set; }

    public string Totalhours { get; set; } = null!;

    public bool Isdrafted { get; set; }

    public bool? Isactive { get; set; }

    public DateTime Createddate { get; set; }

    public DateTime Lastupdatedate { get; set; }

    public int Createdby { get; set; }

    public int Lastupdateby { get; set; }

    public virtual Rmsemployee Employee { get; set; } = null!;

    public virtual ICollection<Timesheetdetailold> Timesheetdetailolds { get; set; } = new List<Timesheetdetailold>();
}

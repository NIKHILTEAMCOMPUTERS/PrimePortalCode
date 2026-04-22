using System;
using System.Collections.Generic;

namespace RMS.Data.Models;

public partial class Oldproject
{
    public int Projectid { get; set; }

    public string? Projectname { get; set; }

    public string? Projectdiscription { get; set; }

    public int Customerid { get; set; }

    public int Statusid { get; set; }

    public virtual Status Status { get; set; } = null!;

    public virtual ICollection<Templatedetail> Templatedetails { get; set; } = new List<Templatedetail>();

    public virtual ICollection<Timesheetdetailold> Timesheetdetailolds { get; set; } = new List<Timesheetdetailold>();
}

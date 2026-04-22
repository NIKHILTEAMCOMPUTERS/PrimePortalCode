using System;
using System.Collections.Generic;

namespace RMS.Data.Models;

public partial class Categoryofactivity
{
    public int Categoryofactivityid { get; set; }

    public string Categoryofactivityname { get; set; } = null!;

    public virtual ICollection<Timesheetdetail> Timesheetdetails { get; set; } = new List<Timesheetdetail>();
}

using System;
using System.Collections.Generic;

namespace RMS.Data.Models;

public partial class ForeslosureProjectcontractHistory
{
    public int Historyid { get; set; }

    public int Contractid { get; set; }

    public DateTime Oldcontractenddate { get; set; }

    public DateTime Newcontractenddate { get; set; }

    public DateTime Changedate { get; set; }

    public int Changedby { get; set; }

    public virtual Projectcontract Contract { get; set; } = null!;
}

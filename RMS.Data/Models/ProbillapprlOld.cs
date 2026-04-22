using System;
using System.Collections.Generic;

namespace RMS.Data.Models;

public partial class ProbillapprlOld
{
    public int Probillapprlid { get; set; }

    public int Contractbillingprovesionid { get; set; }

    public int Currentstageid { get; set; }

    public int Statusid { get; set; }

    public virtual Contractbillingprovesion Contractbillingprovesion { get; set; } = null!;

    public virtual Probillapprlstage Currentstage { get; set; } = null!;

    public virtual ICollection<ProbillapprldetailOld> ProbillapprldetailOlds { get; set; } = new List<ProbillapprldetailOld>();

    public virtual Status Status { get; set; } = null!;
}

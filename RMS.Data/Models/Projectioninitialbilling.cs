using System;
using System.Collections.Generic;

namespace RMS.Data.Models;

public partial class Projectioninitialbilling
{
    public int Projectioninitialbillingid { get; set; }

    public int Projectionid { get; set; }

    public string Monthyear { get; set; } = null!;

    public decimal Amount { get; set; }

    public virtual Projection Projection { get; set; } = null!;
}

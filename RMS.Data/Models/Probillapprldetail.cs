using System;
using System.Collections.Generic;

namespace RMS.Data.Models;

public partial class Probillapprldetail
{
    public int Probillapprldetailid { get; set; }

    public int Probillapprlid { get; set; }

    public int Stageid { get; set; }

    public bool? Isactiontaken { get; set; }

    public DateTime? Actiontakenon { get; set; }

    public string? Remark { get; set; }

    public virtual ICollection<Contractbillingprovisionhistory> Contractbillingprovisionhistories { get; set; } = new List<Contractbillingprovisionhistory>();

    public virtual Probillapprl Probillapprl { get; set; } = null!;

    public virtual Probillapprlstage Stage { get; set; } = null!;
}

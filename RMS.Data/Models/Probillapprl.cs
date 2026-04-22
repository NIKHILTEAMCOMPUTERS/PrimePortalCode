using System;
using System.Collections.Generic;

namespace RMS.Data.Models;

public partial class Probillapprl
{
    public int Probillapprlid { get; set; }

    public int? Contractbillingprovesionid { get; set; }

    public int Currentstageid { get; set; }

    public int Statusid { get; set; }

    public bool? Isactive { get; set; }

    public bool Isdeleted { get; set; }

    public DateTime Createddate { get; set; }

    public DateTime Lastupdatedate { get; set; }

    public int Createdby { get; set; }

    public int Lastupdateby { get; set; }

    public virtual Contractbillingprovesion? Contractbillingprovesion { get; set; }

    public virtual ICollection<Contractbillingprovisionhistory> Contractbillingprovisionhistories { get; set; } = new List<Contractbillingprovisionhistory>();

    public virtual Probillapprlstage Currentstage { get; set; } = null!;

    public virtual ICollection<Probillapprldetail> Probillapprldetails { get; set; } = new List<Probillapprldetail>();

    public virtual Status Status { get; set; } = null!;
}

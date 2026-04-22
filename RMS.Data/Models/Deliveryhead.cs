using System;
using System.Collections.Generic;

namespace RMS.Data.Models;

public partial class Deliveryhead
{
    public int Deliveryheadid { get; set; }

    public int Practiceid { get; set; }

    public int Employeeid { get; set; }

    public bool? Isactive { get; set; }

    public bool Isdeleted { get; set; }

    public DateTime Createddate { get; set; }

    public DateTime Lastupdatedate { get; set; }

    public int Createdby { get; set; }

    public int Lastupdateby { get; set; }

    public virtual Rmsemployee Employee { get; set; } = null!;

    public virtual Practice Practice { get; set; } = null!;
}

using System;
using System.Collections.Generic;

namespace RMS.Data.Models;

public partial class State
{
    public int Stateid { get; set; }

    public int Countryid { get; set; }

    public string Statename { get; set; } = null!;

    public bool? Isactive { get; set; }

    public bool Isdeleted { get; set; }

    public DateTime Createddate { get; set; }

    public DateTime Lastupdatedate { get; set; }

    public int Createdby { get; set; }

    public int Lastupdateby { get; set; }

    public string? Statecode { get; set; }

    public virtual ICollection<City> Cities { get; set; } = new List<City>();

    public virtual Country Country { get; set; } = null!;
}

using System;
using System.Collections.Generic;

namespace RMS.Data.Models;

public partial class Country
{
    public int Countryid { get; set; }

    public string Countryname { get; set; } = null!;

    public bool? Isactive { get; set; }

    public bool Isdeleted { get; set; }

    public DateTime Createddate { get; set; }

    public DateTime Lastupdatedate { get; set; }

    public int Createdby { get; set; }

    public int Lastupdateby { get; set; }

    public string? Countrycode { get; set; }

    public virtual ICollection<State> States { get; set; } = new List<State>();
}

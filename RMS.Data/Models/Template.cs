using System;
using System.Collections.Generic;

namespace RMS.Data.Models;

public partial class Template
{
    public int Templateid { get; set; }

    public int Employeeid { get; set; }

    public string Templatename { get; set; } = null!;

    public string Totalhours { get; set; } = null!;

    public bool? Isactive { get; set; }

    public DateTime Createddate { get; set; }

    public DateTime Lastupdatedate { get; set; }

    public int Createdby { get; set; }

    public int Lastupdateby { get; set; }

    public virtual Rmsemployee Employee { get; set; } = null!;

    public virtual ICollection<Templatedetail> Templatedetails { get; set; } = new List<Templatedetail>();
}

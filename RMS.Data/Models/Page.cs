using System;
using System.Collections.Generic;

namespace RMS.Data.Models;

public partial class Page
{
    public int Pageid { get; set; }

    public string? Pagename { get; set; }

    public string? Icon { get; set; }

    public int? Moduleid { get; set; }

    public string? Controllername { get; set; }

    public string? Actionname { get; set; }

    public bool? Isactive { get; set; }

    public bool? Isdeleted { get; set; }

    public DateTime? Createddate { get; set; }

    public DateTime? Lastupdateddate { get; set; }

    public int? Createdby { get; set; }

    public int? Lastupdatedby { get; set; }

    public virtual Module? Module { get; set; }

    public virtual ICollection<Rolepage> Rolepages { get; set; } = new List<Rolepage>();
}

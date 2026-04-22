using System;
using System.Collections.Generic;

namespace RMS.Data.Models;

public partial class Module
{
    public int Moduleid { get; set; }

    public string? Modulename { get; set; }

    public string? Icon { get; set; }

    public bool? Isactive { get; set; }

    public bool? Isdeleted { get; set; }

    public DateTime? Createddate { get; set; }

    public DateTime? Lastupdateddate { get; set; }

    public int? Createdby { get; set; }

    public int? Lastupdatedby { get; set; }

    public virtual ICollection<Page> Pages { get; set; } = new List<Page>();
}

using System;
using System.Collections.Generic;

namespace RMS.Data.Models;

public partial class Subpractise
{
    public int Subpractiseid { get; set; }

    public int? Practiceid { get; set; }

    public string Subpractisename { get; set; } = null!;

    public int? Statusid { get; set; }

    public bool? Isactive { get; set; }

    public bool Isdeleted { get; set; }

    public DateTime Createddate { get; set; }

    public DateTime Lastupdatedate { get; set; }

    public int Createdby { get; set; }

    public int Lastupdateby { get; set; }

    public virtual Practice? Practice { get; set; }

    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();

    public virtual Status? Status { get; set; }
}

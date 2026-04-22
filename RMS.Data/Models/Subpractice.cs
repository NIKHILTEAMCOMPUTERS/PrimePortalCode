using System;
using System.Collections.Generic;

namespace RMS.Data.Models;

public partial class Subpractice
{
    public int Subpracticeid { get; set; }

    public int? Practiceid { get; set; }

    public string Subpracticename { get; set; } = null!;

    public int? Statusid { get; set; }

    public bool? Isactive { get; set; }

    public bool Isdeleted { get; set; }

    public DateTime Createddate { get; set; }

    public DateTime Lastupdatedate { get; set; }

    public int Createdby { get; set; }

    public int Lastupdateby { get; set; }

    public virtual ICollection<OafExtendedHistory> OafExtendedHistories { get; set; } = new List<OafExtendedHistory>();

    public virtual ICollection<Oaf> Oafs { get; set; } = new List<Oaf>();

    public virtual Practice? Practice { get; set; }

    public virtual ICollection<Projection> Projections { get; set; } = new List<Projection>();

    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();

    public virtual ICollection<Rmsemployee> Rmsemployees { get; set; } = new List<Rmsemployee>();

    public virtual Status? Status { get; set; }
}

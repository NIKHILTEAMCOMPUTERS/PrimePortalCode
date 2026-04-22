using System;
using System.Collections.Generic;

namespace RMS.Data.Models;

public partial class Projecttype
{
    public int Projecttypeid { get; set; }

    public string Projecttypename { get; set; } = null!;

    public int? Statusid { get; set; }

    public bool? Isactive { get; set; }

    public bool Isdeleted { get; set; }

    public DateTime Createddate { get; set; }

    public DateTime Lastupdatedate { get; set; }

    public int Createdby { get; set; }

    public int Lastupdateby { get; set; }

    public virtual ICollection<OafExtendedHistory> OafExtendedHistories { get; set; } = new List<OafExtendedHistory>();

    public virtual ICollection<Oaf> Oafs { get; set; } = new List<Oaf>();

    public virtual ICollection<Projection> Projections { get; set; } = new List<Projection>();

    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();

    public virtual Status? Status { get; set; }
}

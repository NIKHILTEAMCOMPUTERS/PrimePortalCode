using System;
using System.Collections.Generic;

namespace RMS.Data.Models;

public partial class Employeeskill
{
    public int Employeeskillid { get; set; }

    public int Employeeid { get; set; }

    public int Skillid { get; set; }

    public decimal? Experinceinmonths { get; set; }

    public string? Certificationurl { get; set; }

    public bool? Isprimary { get; set; }

    public decimal? Managerrating { get; set; }

    public decimal? Selfreting { get; set; }

    public int? Statusid { get; set; }

    public bool? Isactive { get; set; }

    public bool Isdeleted { get; set; }

    public DateTime Createddate { get; set; }

    public DateTime Lastupdatedate { get; set; }

    public int Createdby { get; set; }

    public int Lastupdateby { get; set; }

    public virtual Rmsemployee Employee { get; set; } = null!;

    public virtual Skill Skill { get; set; } = null!;

    public virtual Status? Status { get; set; }
}

using System;
using System.Collections.Generic;

namespace RMS.Data.Models;

public partial class Milestonedetail
{
    public int Id { get; set; }

    public int Oafid { get; set; }

    public string Milestonedesc { get; set; } = null!;

    public decimal? Percentage { get; set; }

    public decimal? Amount { get; set; }

    public bool? Isactive { get; set; }

    public bool Isdeleted { get; set; }

    public DateTime Createddate { get; set; }

    public DateTime Lastupdatedate { get; set; }

    public int Createdby { get; set; }

    public int Lastupdateby { get; set; }

    public int? Days { get; set; }

    public virtual Oaf Oaf { get; set; } = null!;
}

using System;
using System.Collections.Generic;

namespace RMS.Data.Models;

public partial class Categorystatus
{
    public int Categorystatusid { get; set; }

    public string Categorystatusname { get; set; } = null!;

    public string? Categorystatusdescription { get; set; }

    public int? Statusid { get; set; }

    public bool? Isactive { get; set; }

    public bool Isdeleted { get; set; }

    public DateTime Createddate { get; set; }

    public DateTime Lastupdatedate { get; set; }

    public int Createdby { get; set; }

    public int Lastupdateby { get; set; }

    public int Categorysubstatusid { get; set; }

    public string? Categorysubstatusname { get; set; }

    public virtual ICollection<Categorysubstatus> Categorysubstatuses { get; set; } = new List<Categorysubstatus>();

    public virtual Status? Status { get; set; }
}

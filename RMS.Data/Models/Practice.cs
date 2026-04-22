using System;
using System.Collections.Generic;

namespace RMS.Data.Models;

public partial class Practice
{
    public int Practiceid { get; set; }

    public string Practicename { get; set; } = null!;

    public int? Statusid { get; set; }

    public bool? Isactive { get; set; }

    public bool Isdeleted { get; set; }

    public DateTime Createddate { get; set; }

    public DateTime Lastupdatedate { get; set; }

    public int Createdby { get; set; }

    public int Lastupdateby { get; set; }

    public string? Code { get; set; }

    public virtual ICollection<Deliveryhead> Deliveryheads { get; set; } = new List<Deliveryhead>();

    public virtual ICollection<Practicehead> Practiceheads { get; set; } = new List<Practicehead>();

    public virtual ICollection<Presalesquestionmaster> Presalesquestionmasters { get; set; } = new List<Presalesquestionmaster>();

    public virtual Status? Status { get; set; }

    public virtual ICollection<Subpractice> Subpractices { get; set; } = new List<Subpractice>();
}

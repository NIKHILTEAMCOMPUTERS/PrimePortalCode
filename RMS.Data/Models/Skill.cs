using System;
using System.Collections.Generic;

namespace RMS.Data.Models;

public partial class Skill
{
    public int Skillid { get; set; }

    public string Skillname { get; set; } = null!;

    public bool? Isactive { get; set; }

    public bool Isdeleted { get; set; }

    public DateTime Createddate { get; set; }

    public DateTime Lastupdatedate { get; set; }

    public int Createdby { get; set; }

    public int Lastupdateby { get; set; }

    public virtual ICollection<CostsheetdetailHistory> CostsheetdetailHistories { get; set; } = new List<CostsheetdetailHistory>();

    public virtual ICollection<Costsheetdetail> Costsheetdetails { get; set; } = new List<Costsheetdetail>();

    public virtual ICollection<Employeeskill> Employeeskills { get; set; } = new List<Employeeskill>();

    public virtual ICollection<Skillcosting> Skillcostings { get; set; } = new List<Skillcosting>();

    public virtual ICollection<Skilltag> Skilltags { get; set; } = new List<Skilltag>();
}

using System;
using System.Collections.Generic;

namespace RMS.Data.Models;

public partial class Costsheet
{
    public int Costsheetid { get; set; }

    public string Costsheetname { get; set; } = null!;

    public bool? Isactive { get; set; }

    public bool Isdeleted { get; set; }

    public DateTime Createddate { get; set; }

    public DateTime Lastupdatedate { get; set; }

    public int Createdby { get; set; }

    public int Lastupdateby { get; set; }

    public virtual ICollection<CostsheetdetailHistory> CostsheetdetailHistories { get; set; } = new List<CostsheetdetailHistory>();

    public virtual ICollection<Costsheetdetail> Costsheetdetails { get; set; } = new List<Costsheetdetail>();

    public virtual ICollection<OafExtendedHistory> OafExtendedHistories { get; set; } = new List<OafExtendedHistory>();

    public virtual ICollection<Oaf> Oafs { get; set; } = new List<Oaf>();

    public virtual ICollection<Projection> Projections { get; set; } = new List<Projection>();
}

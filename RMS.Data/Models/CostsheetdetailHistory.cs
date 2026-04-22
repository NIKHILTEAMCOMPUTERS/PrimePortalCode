using System;
using System.Collections.Generic;

namespace RMS.Data.Models;

public partial class CostsheetdetailHistory
{
    public int Costsheetdetailid { get; set; }

    public int Costsheetid { get; set; }

    public int Skillid { get; set; }

    public string? Skillexperince { get; set; }

    public int? Requiredresource { get; set; }

    public decimal? Skillcost { get; set; }

    public decimal? Xvalue { get; set; }

    public decimal? Perioddays { get; set; }

    public decimal? Customerprice { get; set; }

    public decimal? Totalcost { get; set; }

    public decimal? Totalprice { get; set; }

    public int? Version { get; set; }

    public virtual Costsheet Costsheet { get; set; } = null!;

    public virtual Skill Skill { get; set; } = null!;
}

using System;
using System.Collections.Generic;

namespace RMS.Data.Models;

public partial class Skillcosting
{
    public int Skillcostingid { get; set; }

    public int? Skillid { get; set; }

    public string? Expname { get; set; }

    public int? Fromexpmonth { get; set; }

    public int? Toexpmonth { get; set; }

    public decimal? Amount { get; set; }

    public virtual Skill? Skill { get; set; }
}

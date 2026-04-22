using System;
using System.Collections.Generic;

namespace RMS.Data.Models;

public partial class AggrEmployeeSkillExpeCost
{
    public int? Employeeid { get; set; }

    public string? Employeename { get; set; }

    public decimal? Experinceinmonths { get; set; }

    public decimal? Managerrating { get; set; }

    public decimal? Selfreting { get; set; }

    public bool? Isprimary { get; set; }

    public bool? Isactive { get; set; }

    public bool? Isdeleted { get; set; }

    public string? Skillname { get; set; }

    public int? Skillid { get; set; }

    public string? Expname { get; set; }

    public decimal? Amount { get; set; }
}

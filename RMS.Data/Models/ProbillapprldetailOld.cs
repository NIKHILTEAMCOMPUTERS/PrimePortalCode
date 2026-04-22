using System;
using System.Collections.Generic;

namespace RMS.Data.Models;

public partial class ProbillapprldetailOld
{
    public int Probillapprldetail { get; set; }

    public int Probillapprlid { get; set; }

    public int Statusid { get; set; }

    public bool? Isactiontaken { get; set; }

    public DateTime? Actiontakenon { get; set; }

    public string? Remark { get; set; }

    public virtual ProbillapprlOld Probillapprl { get; set; } = null!;

    public virtual Status Status { get; set; } = null!;
}

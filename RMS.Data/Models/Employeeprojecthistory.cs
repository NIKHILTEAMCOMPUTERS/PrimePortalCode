using System;
using System.Collections.Generic;

namespace RMS.Data.Models;

public partial class Employeeprojecthistory
{
    public int Historyid { get; set; }

    public int? Contractemployeeid { get; set; }

    public int? Categorysubstatusid { get; set; }

    public DateTime? Effectivestartdate { get; set; }

    public DateTime? Effectiveenddate { get; set; }

    public DateTime? Actiontakenon { get; set; }

    public int? Actiontakenby { get; set; }

    public virtual Categorysubstatus? Categorysubstatus { get; set; }

    public virtual Contractemployee? Contractemployee { get; set; }
}

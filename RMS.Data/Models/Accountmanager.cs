using System;
using System.Collections.Generic;

namespace RMS.Data.Models;

public partial class Accountmanager
{
    public int Accountmanagerid { get; set; }

    public int? Employeeid { get; set; }

    public string? Userid { get; set; }

    public int? Departmentid { get; set; }

    public virtual Department? Department { get; set; }

    public virtual Rmsemployee? Employee { get; set; }
}

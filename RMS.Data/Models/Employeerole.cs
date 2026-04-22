using System;
using System.Collections.Generic;

namespace RMS.Data.Models;

public partial class Employeerole
{
    public int Employeeroleid { get; set; }

    public int? Roleid { get; set; }

    public int Employeeid { get; set; }

    public virtual Rmsemployee Employee { get; set; } = null!;

    public virtual Role? Role { get; set; }
}

using System;
using System.Collections.Generic;

namespace RMS.Data.Models;

public partial class TempProject
{
    public string? Projectname { get; set; }

    public string? Projectdescription { get; set; }

    public int? Customerid { get; set; }

    public string? Customername { get; set; }

    public int? Projecttypeid { get; set; }

    public int? Subpracticeid { get; set; }

    public string? Projectno { get; set; }
}

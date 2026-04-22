using System;
using System.Collections.Generic;

namespace RMS.Data.Models;

public partial class ProjectTemp
{
    public int? Projectid { get; set; }

    public string? Projectname { get; set; }

    public string? Projectdescription { get; set; }

    public int? Customerid { get; set; }

    public int? Projectmodelid { get; set; }

    public string? Projectheadid { get; set; }

    public int? Projecttypeid { get; set; }

    public int? Subpracticeid { get; set; }

    public int? Statusid { get; set; }

    public bool? Isactive { get; set; }

    public bool? Isdeleted { get; set; }

    public DateTime? Createddate { get; set; }

    public DateTime? Lastupdatedate { get; set; }

    public int? Createdby { get; set; }

    public int? Lastupdatedby { get; set; }

    public string? Projectno { get; set; }
}

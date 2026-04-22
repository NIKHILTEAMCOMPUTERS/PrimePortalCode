using System;
using System.Collections.Generic;

namespace RMS.Data.Models;

public partial class ProjectOld
{
    public int Projectid { get; set; }

    public string? Projectname { get; set; }

    public string? Projectdescription { get; set; }

    public int? Customerid { get; set; }

    public string? Ponumber { get; set; }

    public int? Projectmodelid { get; set; }

    public string? Projectheadid { get; set; }

    public int? Projecttypeid { get; set; }

    public int? Subpractiseid { get; set; }

    public string? Contractno { get; set; }

    public DateOnly? Contractstartdate { get; set; }

    public DateOnly? Contractenddate { get; set; }

    public string? Contactpersonname { get; set; }

    public int? Statusid { get; set; }

    public bool? Isactive { get; set; }

    public bool Isdeleted { get; set; }

    public DateTime Createddate { get; set; }

    public DateTime Lastupdatedate { get; set; }

    public int Createdby { get; set; }

    public int Lastupdateby { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual Customer? Projectmodel { get; set; }

    public virtual Customer? Projecttype { get; set; }

    public virtual Status? Status { get; set; }

    public virtual Customer? Subpractise { get; set; }
}

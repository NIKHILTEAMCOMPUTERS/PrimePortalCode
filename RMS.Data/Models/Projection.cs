using System;
using System.Collections.Generic;

namespace RMS.Data.Models;

public partial class Projection
{
    public int Projectionid { get; set; }

    public string? Projectionno { get; set; }

    public string? Projectionname { get; set; }

    public string? Projectiondescription { get; set; }

    public int Customerid { get; set; }

    public int? Subpracticeid { get; set; }

    public int? Projectheadid { get; set; }

    public DateTime Startdate { get; set; }

    public DateTime Enddate { get; set; }

    public decimal? Projectioncost { get; set; }

    public int? Statusid { get; set; }

    public bool? Isactive { get; set; }

    public bool Isdeleted { get; set; }

    public DateTime Createddate { get; set; }

    public DateTime? Lastupdatedate { get; set; }

    public int Createdby { get; set; }

    public int? Lastupdatedby { get; set; }

    public int CrmDealsid { get; set; }

    public int? Projecttypeid { get; set; }

    public int? Costsheetid { get; set; }

    public virtual Costsheet? Costsheet { get; set; }

    public virtual CrmDeal CrmDeals { get; set; } = null!;

    public virtual Customer Customer { get; set; } = null!;

    public virtual Rmsemployee? Projecthead { get; set; }

    public virtual ICollection<ProjectionEmployeeDeployement> ProjectionEmployeeDeployements { get; set; } = new List<ProjectionEmployeeDeployement>();

    public virtual ICollection<Projectioninitialbilling> Projectioninitialbillings { get; set; } = new List<Projectioninitialbilling>();

    public virtual ICollection<Projectionrequest> Projectionrequests { get; set; } = new List<Projectionrequest>();

    public virtual Projecttype? Projecttype { get; set; }

    public virtual Status? Status { get; set; }

    public virtual Subpractice? Subpractice { get; set; }
}

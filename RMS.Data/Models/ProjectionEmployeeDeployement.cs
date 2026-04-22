using System;
using System.Collections.Generic;

namespace RMS.Data.Models;

public partial class ProjectionEmployeeDeployement
{
    public int ProjectionEmployeeDeployementId { get; set; }

    public int Projectionid { get; set; }

    public DateTime? Startdate { get; set; }

    public DateTime? Enddate { get; set; }

    public int? Categorysubstatusid { get; set; }

    public bool? Isactive { get; set; }

    public bool Isdeleted { get; set; }

    public DateTime Createddate { get; set; }

    public DateTime Lastupdatedate { get; set; }

    public int Createdby { get; set; }

    public int Lastupdateby { get; set; }

    public string? Remarks { get; set; }

    public int DeployedEmployeeId { get; set; }

    public virtual Categorysubstatus? Categorysubstatus { get; set; }

    public virtual Rmsemployee DeployedEmployee { get; set; } = null!;

    public virtual Projection Projection { get; set; } = null!;
}

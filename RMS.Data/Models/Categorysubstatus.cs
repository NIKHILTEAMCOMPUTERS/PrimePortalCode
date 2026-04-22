using System;
using System.Collections.Generic;

namespace RMS.Data.Models;

public partial class Categorysubstatus
{
    public int Categorysubstatusid { get; set; }

    public int Categorystatusid { get; set; }

    public string Categorysubstatusname { get; set; } = null!;

    public string? Categorysubstatusdescription { get; set; }

    public int? Statusid { get; set; }

    public bool? Isactive { get; set; }

    public bool Isdeleted { get; set; }

    public DateTime Createddate { get; set; }

    public DateTime Lastupdatedate { get; set; }

    public int Createdby { get; set; }

    public int Lastupdateby { get; set; }

    public virtual Categorystatus Categorystatus { get; set; } = null!;

    public virtual ICollection<Contractemployeedeploymentdate> Contractemployeedeploymentdates { get; set; } = new List<Contractemployeedeploymentdate>();

    public virtual ICollection<Contractemployee> Contractemployees { get; set; } = new List<Contractemployee>();

    public virtual ICollection<Employeeprojecthistory> Employeeprojecthistories { get; set; } = new List<Employeeprojecthistory>();

    public virtual ICollection<Projectemployeeassignment> Projectemployeeassignments { get; set; } = new List<Projectemployeeassignment>();

    public virtual ICollection<ProjectionEmployeeDeployement> ProjectionEmployeeDeployements { get; set; } = new List<ProjectionEmployeeDeployement>();

    public virtual ICollection<Rmsemployee> Rmsemployees { get; set; } = new List<Rmsemployee>();

    public virtual Status? Status { get; set; }
}

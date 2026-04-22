using System;
using System.Collections.Generic;

namespace RMS.Data.Models;

public partial class Employee
{
    public int Employeeid { get; set; }

    public string? Userid { get; set; }

    public string? Employeename { get; set; }

    public string? Companyemail { get; set; }

    public string? Sbu { get; set; }

    public string? Contactno { get; set; }

    public DateOnly? Dateofjoining { get; set; }

    public DateOnly? Dateofbirth { get; set; }

    public int Designationid { get; set; }

    public int? Reportheadid { get; set; }

    public int Departmentid { get; set; }

    public int? Statusid { get; set; }

    public bool? Isactive { get; set; }

    public bool? Isdeleted { get; set; }

    public int? Createdby { get; set; }

    public DateOnly? Craeatedon { get; set; }

    public DateOnly? Lastupdateby { get; set; }

    public DateOnly? Lastupdatedon { get; set; }

    public string? Udf1 { get; set; }

    public string? Udf2 { get; set; }

    public string? Udf3 { get; set; }

    public int? Sbuid { get; set; }

    public int? Branchid { get; set; }

    public virtual Branch? Branch { get; set; }

    public virtual Department Department { get; set; } = null!;

    public virtual Designation Designation { get; set; } = null!;

    public virtual Sbu? SbuNavigation { get; set; }

    public virtual Status? Status { get; set; }

    public virtual ICollection<Template> Templates { get; set; } = new List<Template>();

    public virtual ICollection<Timesheet> Timesheets { get; set; } = new List<Timesheet>();
}

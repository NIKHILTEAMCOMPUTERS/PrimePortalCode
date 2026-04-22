using System;
using System.Collections.Generic;

namespace RMS.Data.Models;

public partial class Rmsemployee
{
    public int Employeeid { get; set; }

    public string? Userid { get; set; }

    public string? Employeename { get; set; }

    public string? Companyemail { get; set; }

    public string? Sbu { get; set; }

    public string? Contactno { get; set; }

    public int Designationid { get; set; }

    public string? Reportheadid { get; set; }

    public int Departmentid { get; set; }

    public int? Statusid { get; set; }

    public bool? Isactive { get; set; }

    public bool? Isdeleted { get; set; }

    public int? Createdby { get; set; }

    public DateOnly? Createdon { get; set; }

    public DateOnly? Lastupdatedon { get; set; }

    public string? Udf1 { get; set; }

    public int? Udf2 { get; set; }

    public string? Udf3 { get; set; }

    public int? Sbuid { get; set; }

    public int? Branchid { get; set; }

    public string? Dateofbirth { get; set; }

    public string? Dateofjoining { get; set; }

    public int? Lastupdatedby { get; set; }

    public int? Categorysubstatusid { get; set; }

    public int? Subpracticeid { get; set; }

    public string? Employeeregion { get; set; }

    public string? Baseoffice { get; set; }

    public string? Costcenter { get; set; }

    public string? Workexperience { get; set; }

    public int? Workexedays { get; set; }

    public bool Ade { get; set; }

    public string? Resignationdate { get; set; }

    public virtual ICollection<Acprojecthistory> Acprojecthistories { get; set; } = new List<Acprojecthistory>();

    public virtual Branch? Branch { get; set; }

    public virtual Categorysubstatus? Categorysubstatus { get; set; }

    public virtual ICollection<Contractemployee> Contractemployees { get; set; } = new List<Contractemployee>();

    public virtual ICollection<Deliveryhead> Deliveryheads { get; set; } = new List<Deliveryhead>();

    public virtual Department Department { get; set; } = null!;

    public virtual Designation Designation { get; set; } = null!;

    public virtual ICollection<Employeerole> Employeeroles { get; set; } = new List<Employeerole>();

    public virtual ICollection<Employeeskill> Employeeskills { get; set; } = new List<Employeeskill>();

    public virtual ICollection<Oaf> OafAccountmanagers { get; set; } = new List<Oaf>();

    public virtual ICollection<Oaf> OafDeliveryanchors { get; set; } = new List<Oaf>();

    public virtual ICollection<OafExtendedHistory> OafExtendedHistoryAccountmanagers { get; set; } = new List<OafExtendedHistory>();

    public virtual ICollection<OafExtendedHistory> OafExtendedHistoryDeliveryanchors { get; set; } = new List<OafExtendedHistory>();

    public virtual ICollection<Practicehead> Practiceheads { get; set; } = new List<Practicehead>();

    public virtual ICollection<Probillapprlstage> Probillapprlstages { get; set; } = new List<Probillapprlstage>();

    public virtual ICollection<Projectcontract> Projectcontracts { get; set; } = new List<Projectcontract>();

    public virtual ICollection<Projectemployeeassignment> Projectemployeeassignments { get; set; } = new List<Projectemployeeassignment>();

    public virtual ICollection<ProjectionEmployeeDeployement> ProjectionEmployeeDeployements { get; set; } = new List<ProjectionEmployeeDeployement>();

    public virtual ICollection<Projection> Projections { get; set; } = new List<Projection>();

    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();

    public virtual Sbu? SbuNavigation { get; set; }

    public virtual Status? Status { get; set; }

    public virtual Subpractice? Subpractice { get; set; }

    public virtual ICollection<Template> Templates { get; set; } = new List<Template>();

    public virtual ICollection<Timesheetheader> Timesheetheaders { get; set; } = new List<Timesheetheader>();

    public virtual ICollection<Timesheetold> Timesheetolds { get; set; } = new List<Timesheetold>();

    public virtual Vendor? Udf2Navigation { get; set; }
}

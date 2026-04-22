using System;
using System.Collections.Generic;

namespace RMS.Data.Models;

public partial class Status
{
    public int Statusid { get; set; }

    public string? Statusname { get; set; }

    public string? Statusdiscription { get; set; }

    public int? Statuscode { get; set; }

    public virtual ICollection<Categorystatus> Categorystatuses { get; set; } = new List<Categorystatus>();

    public virtual ICollection<Categorysubstatus> Categorysubstatuses { get; set; } = new List<Categorysubstatus>();

    public virtual ICollection<Contractbillingprovesion> Contractbillingprovesions { get; set; } = new List<Contractbillingprovesion>();

    public virtual ICollection<Contractbillingprovisionhistory> Contractbillingprovisionhistories { get; set; } = new List<Contractbillingprovisionhistory>();

    public virtual ICollection<Contractbilling> Contractbillings { get; set; } = new List<Contractbilling>();

    public virtual ICollection<Contractpresalesresponse> Contractpresalesresponses { get; set; } = new List<Contractpresalesresponse>();

    public virtual ICollection<Employeeskill> Employeeskills { get; set; } = new List<Employeeskill>();

    public virtual ICollection<OafExtendedHistory> OafExtendedHistories { get; set; } = new List<OafExtendedHistory>();

    public virtual ICollection<Oafchecklist> Oafchecklists { get; set; } = new List<Oafchecklist>();

    public virtual ICollection<Oaf> Oafs { get; set; } = new List<Oaf>();

    public virtual ICollection<Oldproject> Oldprojects { get; set; } = new List<Oldproject>();

    public virtual ICollection<Practice> Practices { get; set; } = new List<Practice>();

    public virtual ICollection<Presalesquestionmaster> Presalesquestionmasters { get; set; } = new List<Presalesquestionmaster>();

    public virtual ICollection<ProbillapprlOld> ProbillapprlOlds { get; set; } = new List<ProbillapprlOld>();

    public virtual ICollection<ProbillapprldetailOld> ProbillapprldetailOlds { get; set; } = new List<ProbillapprldetailOld>();

    public virtual ICollection<Probillapprl> Probillapprls { get; set; } = new List<Probillapprl>();

    public virtual ICollection<ProjectOld> ProjectOlds { get; set; } = new List<ProjectOld>();

    public virtual ICollection<Projectcontract> Projectcontracts { get; set; } = new List<Projectcontract>();

    public virtual ICollection<Projectemployeeassignment> Projectemployeeassignments { get; set; } = new List<Projectemployeeassignment>();

    public virtual ICollection<Projection> Projections { get; set; } = new List<Projection>();

    public virtual ICollection<Projectmodel> Projectmodels { get; set; } = new List<Projectmodel>();

    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();

    public virtual ICollection<Projecttype> Projecttypes { get; set; } = new List<Projecttype>();

    public virtual ICollection<Rmsemployee> Rmsemployees { get; set; } = new List<Rmsemployee>();

    public virtual ICollection<Subpractice> Subpractices { get; set; } = new List<Subpractice>();
}

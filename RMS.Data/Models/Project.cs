using System;
using System.Collections.Generic;

namespace RMS.Data.Models;

public partial class Project
{
    public int Projectid { get; set; }

    public string? Projectname { get; set; }

    public string? Projectdescription { get; set; }

    public int? Customerid { get; set; }

    public int? Projecttypeid { get; set; }

    public int? Subpracticeid { get; set; }

    public int? Statusid { get; set; }

    public bool? Isactive { get; set; }

    public bool Isdeleted { get; set; }

    public DateTime Createddate { get; set; }

    public DateTime? Lastupdatedate { get; set; }

    public int Createdby { get; set; }

    public int? Lastupdatedby { get; set; }

    public string? Projectno { get; set; }

    public int? Accountmanagerid { get; set; }

    public DateTime? Billingcycledate { get; set; }

    public int? CommittedClientBillingDate { get; set; }

    public virtual Rmsemployee? Accountmanager { get; set; }

    public virtual ICollection<Acprojecthistory> Acprojecthistories { get; set; } = new List<Acprojecthistory>();

    public virtual Customer? Customer { get; set; }

    public virtual ICollection<Projectcontract> Projectcontracts { get; set; } = new List<Projectcontract>();

    public virtual ICollection<Projectemployeeassignment> Projectemployeeassignments { get; set; } = new List<Projectemployeeassignment>();

    public virtual Projecttype? Projecttype { get; set; }

    public virtual Status? Status { get; set; }

    public virtual Subpractice? Subpractice { get; set; }
}

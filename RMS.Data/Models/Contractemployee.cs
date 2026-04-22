using System;
using System.Collections.Generic;

namespace RMS.Data.Models;

public partial class Contractemployee
{
    public int Contractemployeeid { get; set; }

    public int Contractid { get; set; }

    public int Employeeid { get; set; }

    public int? Categorysubstatusid { get; set; }

    public DateTime? Lastupdateon { get; set; }

    public int? Lastupdatedby { get; set; }

    public int Createdby { get; set; }

    public DateTime? Createddate { get; set; }

    public int? Costsheetdetailid { get; set; }

    public decimal? Empxvalue { get; set; }

    public virtual Categorysubstatus? Categorysubstatus { get; set; }

    public virtual Projectcontract Contract { get; set; } = null!;

    public virtual ICollection<ContractbillingprovesionToContractbillingHistory> ContractbillingprovesionToContractbillingHistories { get; set; } = new List<ContractbillingprovesionToContractbillingHistory>();

    public virtual ICollection<Contractbillingprovesion> Contractbillingprovesions { get; set; } = new List<Contractbillingprovesion>();

    public virtual ICollection<Contractbilling> Contractbillings { get; set; } = new List<Contractbilling>();

    public virtual ICollection<Contractemployeedeploymentdate> Contractemployeedeploymentdates { get; set; } = new List<Contractemployeedeploymentdate>();

    public virtual Costsheetdetail? Costsheetdetail { get; set; }

    public virtual Rmsemployee Employee { get; set; } = null!;

    public virtual ICollection<Employeeprojecthistory> Employeeprojecthistories { get; set; } = new List<Employeeprojecthistory>();

    public virtual ICollection<Initialsplitcontractbilling> Initialsplitcontractbillings { get; set; } = new List<Initialsplitcontractbilling>();
}

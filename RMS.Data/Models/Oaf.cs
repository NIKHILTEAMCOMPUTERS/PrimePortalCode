using System;
using System.Collections.Generic;

namespace RMS.Data.Models;

public partial class Oaf
{
    public int Oafid { get; set; }

    public string? Ponumber { get; set; }

    public string? Povalue { get; set; }

    public string? Orderdescription { get; set; }

    public string? Potermscondition { get; set; }

    public int Customerid { get; set; }

    public string? Projectname { get; set; }

    public string? Projectmodel { get; set; }

    public int? Projecttypeid { get; set; }

    public int? Subpracticeid { get; set; }

    public string? Projectdescription { get; set; }

    public string? Contractno { get; set; }

    public DateTime? Contractstartdate { get; set; }

    public DateTime? Contractenddate { get; set; }

    public string? Clientcoordinator { get; set; }

    public string? Milestones { get; set; }

    public int? Costsheetid { get; set; }

    public string? Emailattachment { get; set; }

    public string? Proposalattachment { get; set; }

    public string? Poattachment { get; set; }

    public int? Statusid { get; set; }

    public string? Remarks { get; set; }

    public int? Deliveryanchorid { get; set; }

    public string? Udf1 { get; set; }

    public string? Udf2 { get; set; }

    public string? Udf3 { get; set; }

    public bool? Isactive { get; set; }

    public bool Isdeleted { get; set; }

    public DateTime Createddate { get; set; }

    public DateTime Lastupdatedate { get; set; }

    public int Createdby { get; set; }

    public int Lastupdateby { get; set; }

    public decimal? Xvalue { get; set; }

    public string? Costattachment { get; set; }

    public string? Oafno { get; set; }

    public decimal? Advanceamount { get; set; }

    public decimal? Advancepercent { get; set; }

    public int? Accountmanagerid { get; set; }

    public int? CommittedClientBillingDate { get; set; }

    public bool? Isextended { get; set; }

    public virtual Rmsemployee? Accountmanager { get; set; }

    public virtual Costsheet? Costsheet { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual Rmsemployee? Deliveryanchor { get; set; }

    public virtual ICollection<Milestonedetail> Milestonedetails { get; set; } = new List<Milestonedetail>();

    public virtual ICollection<OafExtendedHistory> OafExtendedHistories { get; set; } = new List<OafExtendedHistory>();

    public virtual ICollection<Oafchecklist> Oafchecklists { get; set; } = new List<Oafchecklist>();

    public virtual ICollection<Oafline> Oaflines { get; set; } = new List<Oafline>();

    public virtual ICollection<Projectcontract> Projectcontracts { get; set; } = new List<Projectcontract>();

    public virtual Projecttype? Projecttype { get; set; }

    public virtual Status? Status { get; set; }

    public virtual Subpractice? Subpractice { get; set; }
}

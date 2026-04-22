using System;
using System.Collections.Generic;

namespace RMS.Data.Models;

public partial class Projectcontract
{
    public int Contractid { get; set; }

    public string? Contractno { get; set; }

    public string? Ponumber { get; set; }

    public int? Projectid { get; set; }

    public DateTime? Contractstartdate { get; set; }

    public DateTime? Contractenddate { get; set; }

    public decimal? Amount { get; set; }

    public string? Contactpersonname { get; set; }

    public int? Statusid { get; set; }

    public bool? Isactive { get; set; }

    public bool Isdeleted { get; set; }

    public DateTime Createddate { get; set; }

    public DateTime Lastupdatedate { get; set; }

    public int Createdby { get; set; }

    public int Lastupdateby { get; set; }

    public int? Costsheetid { get; set; }

    public string? Contactnumber { get; set; }

    public string? Remarks { get; set; }

    public int? Deliveryanchorid { get; set; }

    public string? Attachment { get; set; }

    public string? Udf1 { get; set; }

    public string? Invoiceperiod { get; set; }

    public decimal? Povalue { get; set; }

    public int? Oafid { get; set; }

    public bool? Isforeclosure { get; set; }

    public bool? Isprojectestimationdone { get; set; }

    public virtual ICollection<Contractemployee> Contractemployees { get; set; } = new List<Contractemployee>();

    public virtual ICollection<Contractline> Contractlines { get; set; } = new List<Contractline>();

    public virtual ICollection<Contractpresalesresponse> Contractpresalesresponses { get; set; } = new List<Contractpresalesresponse>();

    public virtual Rmsemployee? Deliveryanchor { get; set; }

    public virtual ICollection<Extendedcontract> Extendedcontracts { get; set; } = new List<Extendedcontract>();

    public virtual ICollection<ForeslosureProjectcontractHistory> ForeslosureProjectcontractHistories { get; set; } = new List<ForeslosureProjectcontractHistory>();

    public virtual Oaf? Oaf { get; set; }

    public virtual Project? Project { get; set; }

    public virtual Status? Status { get; set; }

    public virtual ICollection<Timesheetdetail> Timesheetdetails { get; set; } = new List<Timesheetdetail>();
}

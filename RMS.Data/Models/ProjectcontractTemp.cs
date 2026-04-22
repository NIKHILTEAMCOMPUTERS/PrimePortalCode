using System;
using System.Collections.Generic;

namespace RMS.Data.Models;

public partial class ProjectcontractTemp
{
    public int? Contractid { get; set; }

    public string? Contractno { get; set; }

    public string? Ponumber { get; set; }

    public int? Projectid { get; set; }

    public DateOnly? Contractstartdate { get; set; }

    public DateOnly? Contractenddate { get; set; }

    public decimal? Amount { get; set; }

    public string? Contactpersonname { get; set; }

    public int? Statusid { get; set; }

    public bool? Isactive { get; set; }

    public bool? Isdeleted { get; set; }

    public DateTime? Createddate { get; set; }

    public DateTime? Lastupdatedate { get; set; }

    public int? Createdby { get; set; }

    public int? Lastupdateby { get; set; }

    public int? Costsheetid { get; set; }

    public string? Contactnumber { get; set; }

    public string? Remarks { get; set; }

    public int? Deliveryanchorid { get; set; }

    public string? Attachment { get; set; }
}

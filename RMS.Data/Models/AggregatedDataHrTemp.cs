using System;
using System.Collections.Generic;

namespace RMS.Data.Models;

public partial class AggregatedDataHrTemp
{
    public string? Tmc { get; set; }

    public string? Practice { get; set; }

    public int? Practiceid { get; set; }

    public string? Subpractice { get; set; }

    public string? Region { get; set; }

    public string? Function { get; set; }

    public string? Resourcename { get; set; }

    public int? Employeeid { get; set; }

    public string? Globalstatus { get; set; }

    public string? Billablenonbillable { get; set; }

    public string? Projectname { get; set; }

    public string? Projecttypename { get; set; }

    public string? Companyname { get; set; }

    public string? Udf3 { get; set; }

    public DateTime? Contractstartdate { get; set; }

    public DateTime? Contractenddate { get; set; }

    public int? Categorysubstatusid { get; set; }

    public string? Empstatus { get; set; }

    public string? Contracstatus { get; set; }
}

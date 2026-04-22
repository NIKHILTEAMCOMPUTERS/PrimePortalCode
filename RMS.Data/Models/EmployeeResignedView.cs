using System;
using System.Collections.Generic;

namespace RMS.Data.Models;

public partial class EmployeeResignedView
{
    public int? Employeeid { get; set; }

    public string? Userid { get; set; }

    public string? Employeename { get; set; }

    public string? Companyemail { get; set; }

    public string? Sbu { get; set; }

    public string? Contactno { get; set; }

    public int? Designationid { get; set; }

    public string? Reportheadid { get; set; }

    public int? Departmentid { get; set; }

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

    public bool? Ade { get; set; }
}

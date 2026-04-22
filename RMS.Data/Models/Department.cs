using System;
using System.Collections.Generic;

namespace RMS.Data.Models;

public partial class Department
{
    public int Departmentid { get; set; }

    public string Departmentname { get; set; } = null!;

    public string? Description { get; set; }

    public bool? Isactive { get; set; }

    public bool Isdeleted { get; set; }

    public DateTime Createddate { get; set; }

    public DateTime Lastupdatedate { get; set; }

    public int Createdby { get; set; }

    public int Lastupdateby { get; set; }

    public virtual ICollection<Rmsemployee> Rmsemployees { get; set; } = new List<Rmsemployee>();

    public virtual ICollection<Templatedetail> Templatedetails { get; set; } = new List<Templatedetail>();

    public virtual ICollection<Timesheetdetailold> Timesheetdetailolds { get; set; } = new List<Timesheetdetailold>();

    public virtual ICollection<Timesheetdetail> Timesheetdetails { get; set; } = new List<Timesheetdetail>();
}

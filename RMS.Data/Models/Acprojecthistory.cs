using System;
using System.Collections.Generic;

namespace RMS.Data.Models;

public partial class Acprojecthistory
{
    public int Acprojecthistoryid { get; set; }

    public int? Projectid { get; set; }

    public int? Acmanagerid { get; set; }

    public bool? Isactive { get; set; }

    public bool Isdeleted { get; set; }

    public DateTime? Startdate { get; set; }

    public DateTime? Enddate { get; set; }

    public DateTime Createddate { get; set; }

    public DateTime Lastupdatedate { get; set; }

    public int Createdby { get; set; }

    public int Lastupdateby { get; set; }

    public virtual Rmsemployee? Acmanager { get; set; }

    public virtual Project? Project { get; set; }
}

using System;
using System.Collections.Generic;

namespace RMS.Data.Models;

public partial class Probillapprlstage
{
    public int Probillapprlstageid { get; set; }

    public string Stagename { get; set; } = null!;

    public int Stageorder { get; set; }

    public int? Stageapproverid { get; set; }

    public int? Practiceheadid { get; set; }

    public virtual Practicehead? Practicehead { get; set; }

    public virtual ICollection<ProbillapprlOld> ProbillapprlOlds { get; set; } = new List<ProbillapprlOld>();

    public virtual ICollection<Probillapprldetail> Probillapprldetails { get; set; } = new List<Probillapprldetail>();

    public virtual ICollection<Probillapprl> Probillapprls { get; set; } = new List<Probillapprl>();

    public virtual Rmsemployee? Stageapprover { get; set; }
}

using System;
using System.Collections.Generic;

namespace RMS.Data.Models;

public partial class Presalesquestionmaster
{
    public int Questionid { get; set; }

    public string Question { get; set; } = null!;

    public int Practiceid { get; set; }

    public string? Refrenceresponse { get; set; }

    public int? Statusid { get; set; }

    public bool? Isactive { get; set; }

    public bool Isdeleted { get; set; }

    public DateTime Createddate { get; set; }

    public DateTime Lastupdatedate { get; set; }

    public int Createdby { get; set; }

    public int Lastupdateby { get; set; }

    public virtual Practice Practice { get; set; } = null!;

    public virtual Status? Status { get; set; }
}

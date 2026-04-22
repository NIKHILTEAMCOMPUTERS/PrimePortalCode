using System;
using System.Collections.Generic;

namespace RMS.Data.Models;

public partial class Oafchecklist
{
    public int Oafchecklistid { get; set; }

    public int Oafid { get; set; }

    public string? Question { get; set; }

    public string? Clientresponse { get; set; }

    public bool? Isextra { get; set; }

    public int? Statusid { get; set; }

    public string? Remarks { get; set; }

    public virtual Oaf Oaf { get; set; } = null!;

    public virtual Status? Status { get; set; }
}

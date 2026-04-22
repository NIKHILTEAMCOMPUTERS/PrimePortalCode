using System;
using System.Collections.Generic;

namespace RMS.Data.Models;

public partial class Contractpresalesresponse
{
    public int Responseid { get; set; }

    public int Contractid { get; set; }

    public string? Clientresponse { get; set; }

    public int? Statusid { get; set; }

    public bool? Isactive { get; set; }

    public bool Isdeleted { get; set; }

    public DateTime Createddate { get; set; }

    public DateTime Lastupdatedate { get; set; }

    public int Createdby { get; set; }

    public int Lastupdateby { get; set; }

    public string? Question { get; set; }

    public string? Refresponse { get; set; }

    public string? Remarks { get; set; }

    public bool? Isextra { get; set; }

    public virtual Projectcontract Contract { get; set; } = null!;

    public virtual Status? Status { get; set; }
}

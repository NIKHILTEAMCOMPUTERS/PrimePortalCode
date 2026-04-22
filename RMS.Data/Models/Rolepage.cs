using System;
using System.Collections.Generic;

namespace RMS.Data.Models;

public partial class Rolepage
{
    public int Rolepageid { get; set; }

    public int? Roleid { get; set; }

    public int? Pageid { get; set; }

    public bool? Isreadpermit { get; set; }

    public bool? Iswritepermit { get; set; }

    public bool? Isdeletepermit { get; set; }

    public int? Pagesequence { get; set; }

    public bool? Isbillingpermit { get; set; }

    public virtual Page? Page { get; set; }

    public virtual Role? Role { get; set; }
}

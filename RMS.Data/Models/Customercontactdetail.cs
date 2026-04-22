using System;
using System.Collections.Generic;

namespace RMS.Data.Models;

public partial class Customercontactdetail
{
    public int Customercontactdetailid { get; set; }

    public int Customerid { get; set; }

    public string Firstname { get; set; } = null!;

    public string Lastname { get; set; } = null!;

    public string? Mobile { get; set; }

    public string? Emailid { get; set; }

    public virtual Customer Customer { get; set; } = null!;
}

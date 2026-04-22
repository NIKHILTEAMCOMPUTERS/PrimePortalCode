using System;
using System.Collections.Generic;

namespace RMS.Data.Models;

public partial class Customertype
{
    public int Customertypeid { get; set; }

    public string Customertypename { get; set; } = null!;

    public bool? Isactive { get; set; }

    public bool Isdeleted { get; set; }

    public DateTime Createddate { get; set; }

    public DateTime Lastupdatedate { get; set; }

    public int Createdby { get; set; }

    public int Lastupdateby { get; set; }

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();
}

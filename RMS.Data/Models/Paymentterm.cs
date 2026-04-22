using System;
using System.Collections.Generic;

namespace RMS.Data.Models;

public partial class Paymentterm
{
    public int Paymenttermid { get; set; }

    public string Paymenttermname { get; set; } = null!;

    public string? Description { get; set; }

    public bool? Isactive { get; set; }

    public bool Isdeleted { get; set; }

    public DateTime Createddate { get; set; }

    public DateTime Lastupdatedate { get; set; }

    public int Createdby { get; set; }

    public int Lastupdateby { get; set; }

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();

    public virtual ICollection<Vendor> Vendors { get; set; } = new List<Vendor>();
}

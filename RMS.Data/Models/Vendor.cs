using System;
using System.Collections.Generic;

namespace RMS.Data.Models;

public partial class Vendor
{
    public int Vendorid { get; set; }

    public string? Vendorcode { get; set; }

    public string? Vendorname { get; set; }

    public string? Vendorcontact { get; set; }

    public string? Email { get; set; }

    public string? Phone1 { get; set; }

    public string? Phone2 { get; set; }

    public string? Pannumber { get; set; }

    public int? Currencyid { get; set; }

    public string? Gstnumber { get; set; }

    public int? Paymenttermid { get; set; }

    public int? Cityid { get; set; }

    public string? Zipcode { get; set; }

    public string? Address1 { get; set; }

    public string? Address2 { get; set; }

    public bool? Isactive { get; set; }

    public bool? Isdeleted { get; set; }

    public DateTime? Createdate { get; set; }

    public DateTime? Lastupdatedate { get; set; }

    public int? Createdby { get; set; }

    public int? Lastupdatedby { get; set; }

    public string? Udf1 { get; set; }

    public string? Udf2 { get; set; }

    public string? Udf3 { get; set; }

    public virtual City? City { get; set; }

    public virtual Currency? Currency { get; set; }

    public virtual Paymentterm? Paymentterm { get; set; }

    public virtual ICollection<Rmsemployee> Rmsemployees { get; set; } = new List<Rmsemployee>();
}

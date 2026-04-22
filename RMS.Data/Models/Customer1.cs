using System;
using System.Collections.Generic;

namespace RMS.Data.Models;

public partial class Customer1
{
    public int Customerid { get; set; }

    public int Customertypeid { get; set; }

    public string Firstname { get; set; } = null!;

    public string? Lastname { get; set; }

    public string? Companyname { get; set; }

    public string? Companylogourl { get; set; }

    public string? Companyemail { get; set; }

    public string? Phone1 { get; set; }

    public string? Phone2 { get; set; }

    public string? Pannumber { get; set; }

    public int? Currencyid { get; set; }

    public string? Gstnumber { get; set; }

    public int? Paymenttermid { get; set; }

    public int? Cityid { get; set; }

    public string? Zipcode { get; set; }

    public string? Address1 { get; set; }

    public bool? Isactive { get; set; }

    public bool? Isdeleted { get; set; }

    public DateTime? Createddate { get; set; }

    public DateTime? Lastupdatedate { get; set; }

    public int? Createdby { get; set; }

    public int? Lastupdateby { get; set; }

    public string? Customercode { get; set; }

    public string? Address2 { get; set; }

    public string? Contact { get; set; }

    public string? Countryregioncode { get; set; }

    public decimal? Amount { get; set; }

    public string? Oldcustomercode { get; set; }

    public string? Globaldimension9code { get; set; }

    public string? Globaldimension10code { get; set; }

    public string? Globaldimension11code { get; set; }

    public string? Globaldimension12code { get; set; }

    public string? Globaldimension13code { get; set; }

    public string? Globaldimension14code { get; set; }

    public string? Statecode { get; set; }

    public int? Paymentmethodid { get; set; }

    public virtual City? City { get; set; }

    public virtual Currency? Currency { get; set; }

    public virtual ICollection<Customercontactdetail> Customercontactdetails { get; set; } = new List<Customercontactdetail>();

    public virtual Customertype Customertype { get; set; } = null!;

    public virtual Paymentmethod? Paymentmethod { get; set; }

    public virtual Paymentterm? Paymentterm { get; set; }

    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();
}

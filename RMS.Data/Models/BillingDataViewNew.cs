using System;
using System.Collections.Generic;

namespace RMS.Data.Models;

public partial class BillingDataViewNew
{
    public string? Tmc { get; set; }

    public string? Resourcename { get; set; }

    public string? Empstatus { get; set; }

    public string? Accountmanager { get; set; }

    public string? Projectname { get; set; }

    public string? Projectno { get; set; }

    public string? Projecttype { get; set; }

    public string? Customername { get; set; }

    public string? Daname { get; set; }

    public string? Projectsubpractice { get; set; }

    public string? Projectpractice { get; set; }

    public string? Empsubpractice { get; set; }

    public string? Emppractice { get; set; }

    public string? Contractno { get; set; }

    public string? Ponumber { get; set; }

    public decimal? Amount { get; set; }

    public DateTime? Contractstartdate { get; set; }

    public DateTime? Contractenddate { get; set; }

    public decimal? Actualbilling { get; set; }

    public decimal? Provesionbilling { get; set; }

    public string? Billingmonthyear { get; set; }

    public DateTime? Actualestimatedbillingdate { get; set; }

    public DateTime? Provisionestimatedbillingdate { get; set; }

    public DateTime? Billingdate { get; set; }

    public string? Contractstatus { get; set; }
}

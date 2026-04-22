using System;
using System.Collections.Generic;

namespace RMS.Data.Models;

public partial class CrmAggregatedDataFromAllTable
{
    public int? Crmid { get; set; }

    public long? CrmDealsid { get; set; }

    public string? Dealname { get; set; }

    public decimal? Amount { get; set; }

    public decimal? BaseCurrencyAmount { get; set; }

    public DateTime? ExpectedClose { get; set; }

    public DateTime? ClosedDate { get; set; }

    public DateTime? StageUpdatedTime { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? Stagename { get; set; }

    public int? Stageposition { get; set; }

    public string? Satgeforcasttype { get; set; }

    public string? Createdby { get; set; }

    public string? Creatoremail { get; set; }

    public string? Creatormobile { get; set; }

    public string? Territoryname { get; set; }

    public string? Salesaccountname { get; set; }

    public string? Website { get; set; }

    public int? Projectionid { get; set; }
}

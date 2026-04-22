using System;
using System.Collections.Generic;

namespace RMS.Data.Models;

public partial class CrmDeal
{
    public int Id { get; set; }

    public long? CrmDealsid { get; set; }

    public string? Name { get; set; }

    public decimal? Amount { get; set; }

    public decimal? BaseCurrencyAmount { get; set; }

    public DateTime? ExpectedClose { get; set; }

    public DateTime? ClosedDate { get; set; }

    public DateTime? StageUpdatedTime { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? CreatedAt { get; set; }

    public long? CrmDealStagesid { get; set; }

    public long? CreaterId { get; set; }

    public long? UpdaterId { get; set; }

    public long? OwnerId { get; set; }

    public long? CrmLeadSourcesid { get; set; }

    public long? CrmTerritoriesid { get; set; }

    public long? CurrencyId { get; set; }

    public long? CrmSalesAccountsid { get; set; }

    public virtual ICollection<Projection> Projections { get; set; } = new List<Projection>();
}

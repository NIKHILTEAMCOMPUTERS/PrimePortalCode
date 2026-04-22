using System;
using System.Collections.Generic;

namespace RMS.Data.Models;

public partial class CrmSalesAccount
{
    public int Id { get; set; }

    public long? CrmSalesAccountsid { get; set; }

    public string? Name { get; set; }

    public string? Avatar { get; set; }

    public string? Website { get; set; }

    public decimal? OpenDealsAmount { get; set; }

    public int? OpenDealsCount { get; set; }

    public decimal? WonDealsAmount { get; set; }

    public int? WonDealsCount { get; set; }
}

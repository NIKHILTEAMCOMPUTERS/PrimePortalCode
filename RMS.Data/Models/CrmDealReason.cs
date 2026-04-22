using System;
using System.Collections.Generic;

namespace RMS.Data.Models;

public partial class CrmDealReason
{
    public int Id { get; set; }

    public long? CrmDealReasonsid { get; set; }

    public string? Name { get; set; }

    public int? Position { get; set; }
}

using System;
using System.Collections.Generic;

namespace RMS.Data.Models;

public partial class CrmDealType
{
    public int Id { get; set; }

    public long? CrmDealTypesid { get; set; }

    public string? Name { get; set; }

    public int? Position { get; set; }
}

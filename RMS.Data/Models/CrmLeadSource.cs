using System;
using System.Collections.Generic;

namespace RMS.Data.Models;

public partial class CrmLeadSource
{
    public int Id { get; set; }

    public long? CrmLeadSourcesid { get; set; }

    public string? Name { get; set; }

    public int? Position { get; set; }
}

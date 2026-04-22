using System;
using System.Collections.Generic;

namespace RMS.Data.Models;

public partial class CrmTerritory
{
    public int Id { get; set; }

    public long? CrmTerritoriesid { get; set; }

    public string? Name { get; set; }

    public int? Position { get; set; }
}

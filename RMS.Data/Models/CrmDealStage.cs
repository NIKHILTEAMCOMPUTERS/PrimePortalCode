using System;
using System.Collections.Generic;

namespace RMS.Data.Models;

public partial class CrmDealStage
{
    public int Id { get; set; }

    public long? CrmDealStagesid { get; set; }

    public string? Name { get; set; }

    public int? Position { get; set; }

    public string? ForecastType { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public long? DealPipelineId { get; set; }

    public int? ChoiceType { get; set; }

    public int? Probability { get; set; }
}

namespace RMS.Client.Models.Projection
{
    public class CrmDeal
    {
        public int? Crmid { get; set; }
        public long CrmDealsId { get; set; }
        public string DealName { get; set; }
        public decimal Amount { get; set; }
        public decimal BaseCurrencyAmount { get; set; }
        public DateTime? ExpectedClose { get; set; } // Make DateTime nullable
        public DateTime? ClosedDate { get; set; } // Make DateTime nullable
        public DateTime StageUpdatedTime { get; set; }
        public DateTime CreatedAt { get; set; }
        public string StageName { get; set; }
        public int StagePosition { get; set; }
        public string StageForecastType { get; set; }
        public string CreatedBy { get; set; }
        public string CreatorEmail { get; set; }
        public string CreatorMobile { get; set; }
        public string TerritoryName { get; set; }
        public string SalesAccountName { get; set; }
        public string Website { get; set; }
        public int? Projectionid { get; set; } = 0;
    }


}

namespace RMS.Client.Models.Master
{
    public class MonthWisePOReport
    {
        public string dAname { get; set; }
        public string projectCount { get; set; }
        public double totalActualBilling { get; set; }
        public double totalProvisionBilling { get; set; }
        public int fixedBidCount { get; set; }
        public int presalesCount { get; set; }
        public int internalCount { get; set; }
        public int tAndMCount { get; set; }
        public double TotalProjectionAmount { get; set; }

    }
}

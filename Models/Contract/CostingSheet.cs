namespace RMS.Client.Models.Contract
{
    public class CostSheet
    {
        public int costsheetid { get; set; }
        public string costsheetname { get; set; }
        public decimal AverageXValue { get; set; }
        public decimal? TotalAmount { get; set; }

        public string OrderDescription { get; set; }
        public int? OafId { get; set; }
        public List<CostSheetDetail> costsheetdetails { get; set; }
        public List<CostSheetHistory> CostSheetHistory { get; set; }
    }
    public class CostSheetDetail
    {
        public int? Costsheetdetailid { get; set; }
        public int? Costsheetid { get; set; }
        public int Skillid { get; set; }
        public string? skillexperience { get; set; }
        public int Requiredresource { get; set; }
        public decimal Skillcost { get; set; }
        public string Skillname { get; set; } = null!;
        public decimal? Xvalue { get; set; }

        public decimal? Perioddays { get; set; }

        public decimal? Customerprice { get; set; }

        public decimal? Totalcost { get; set; }

        public decimal? Totalprice { get; set; }
    }

    public class CostSheetHistory
    {
        public int? CostSheetId { get; set; }
        public int? Version { get; set; }
        public List<CostSheetDetail> CostSheetDetails { get; set; }
    }
}

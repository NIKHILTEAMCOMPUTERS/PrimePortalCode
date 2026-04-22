namespace RMS.Client.Models.Oaf
{
    public class OAFwiseResourceAllotmentDto
    {
        public int projectId { get; set; } = 0;
        public int contractid { get; set; }
        public List<ResourceDetail> resource_details { get; set; }
    }
    public class ResourceDetail
    {
        public int employeeid { get; set; }
        public int? costsheetdetailid { get; set; }
        public decimal? xvalue { get; set; }
        public List<DeploymentDetail> deployement_details { get; set; }
        public List<BillingDetail> billing_Details { get; set; }
    }


    public class DeploymentDetail
    {
        public DateTime? startdate { get; set; }
        public DateTime? enddate { get; set; }
        public int categorysubstatusid { get; set; }
    }

    public class BillingDetail
    {
        public int employeeid { get; set; }
        public string Monthyear { get; set; }
        public int costing { get; set; }
    }


}

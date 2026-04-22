using RMS.Client.Models.Contract;

namespace RMS.Client.Models.Master
{
    public class Report
    {
        public int? EmpId { get; set; }
        public string? TMC { get; set; }
        public string? ResourceName { get; set; }
        public string? CCname { get; set; }
        public string? ProjectType { get; set; }
        public string? CustomerName { get; set; }
        public string? DAname { get; set; }
        public string? SPname { get; set; }
        public string? Pname { get; set; }
        public string? Cno { get; set; }
        public string? POno { get; set; }
        public DateTime? contractstartdate { get; set; }
        public DateTime? contractenddate { get; set; }
        //public string? Duration { get; set; }
        public decimal? actualbilling { get; set; }
        public string? invoiceperiod { get; set; }
        public decimal? provesionbilling { get; set; }
        public string? projectname { get; set; }
        public string? projectno { get; set; }
        public string? billingmonth { get; set; }
        public int? contractemployeeid { get; set; }
        // added new properties for the billed report
        public decimal? recievedbillingamount { get; set; }
        public bool? isbilled { get; set; }
        public int? contractbillingprovesionid { get; set; }
        public DateTime? estimatedbillingdate { get; set; }
        public DateTime? actualbillingdate { get; set; }
        // added new for acctual billing report
        public string? billingStatus { get; set; }
        public int? contractbillingid { get; set; }
        public decimal? billing { get; set; }
        public string? documentUrl { get; set; }
        public string? Status { get; set; }
        public int? Statusid { get; set; }
        public bool isrevised { get; set; }

    }

    public class ViewModalReport
    {
        public List<Report> ProvisionReportData { get; set; }
        public List<Report> ActualReportData { get; set; }

    }
    public class ProvisionViewModal
    {
        public List<ContractBillingProvision> Empwiseprovisiondata { get; set; }
        public List<ContractData> Projectwiseprovisiondata { get; set; }

       
    }
}

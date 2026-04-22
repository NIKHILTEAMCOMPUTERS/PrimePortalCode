using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Entity.DTO
{
    public class OAFwiseResourceAllotmentDto
    {
        public int projectId { get; set; }
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
        public decimal costing { get; set; }
    }


    public class ExtendOafDto
    {
        public int ?HistoryId { get; set; } 
        public int? Oafid { get; set; }
        public string? Contractno { get; set; }
        public string? Ponumber { get; set; }
        public DateTime? Contractstartdate { get; set; }
        public DateTime? Contractenddate { get; set; }
        
        public string? Povalue { get; set; }
        public string? Clientcoordinator { get; set; }
        public string? Contactno { get; set; }
        public int? Costsheetid { get; set; }
        public decimal? Xvalue { get; set; }
    }




}

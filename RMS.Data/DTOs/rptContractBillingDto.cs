using NpgsqlTypes;
using RMS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Data.DTOs
{
    public class rptContractBillingDto
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
        public DateTime? estimatedbillingdate { get; set; }
        public DateTime? actualbillingdate { get; set; }
        public int? provisionstatusid { get; set; }
    }
    public class rptContractBillingProvisionDto
    {
        public int? contractbillingprovesionid { get; set; }
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
        // public decimal? actualbilling { get; set; }
        public string? invoiceperiod { get; set; }
        public decimal? provesionbilling { get; set; }
        public string? projectname { get; set; }
        public string? projectno { get; set; }
        public string? billingmonth { get; set; }
        public int? contractemployeeid { get; set; }
        public decimal? recievedbillingamount { get; set; }
        public bool? isbilled { get; set; }
        public DateTime? estimatedbillingdate { get; set; }

    }
    public class rptContractBillingActualDto
    {
        public int? contractbillingid { get; set; }
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
        // public decimal? actualbilling { get; set; }
        public string? invoiceperiod { get; set; }
        public decimal? billing { get; set; }
        public string? projectname { get; set; }
        public string? projectno { get; set; }
        public string? billingmonth { get; set; }
        public int? contractemployeeid { get; set; } 
        public bool? isbilled { get; set; }       
        public bool? istobebilled { get; set; }
        public DateTime? estimatedbillingdate { get; set; }
        public string? documenturl { get; set; }
        public int? statusid { get; set; }
        public  bool? isrevised { get; set; }

    }
}

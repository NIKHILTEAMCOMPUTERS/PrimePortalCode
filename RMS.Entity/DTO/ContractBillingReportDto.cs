using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Entity.DTO
{
    public class rptContratBillingDA_WiseDto
    {
        public string? CCname { get; set; }
        public string? ProjectType { get; set; }
        public string? CustomerName { get; set; }
        public string? DAname { get; set; }

        public string? Cno { get; set; }
        public string? POno { get; set; }

        public DateTime? contractstartdate { get; set; }
        public DateTime? contractenddate { get; set; }

        public string? projectname { get; set; }
        public string? projectno { get; set; }
        public string? billingmonth { get; set; }
        public decimal? totalActualBilling { get; set; }
        public decimal? totalProvisionBilling { get; set; }
    }

}

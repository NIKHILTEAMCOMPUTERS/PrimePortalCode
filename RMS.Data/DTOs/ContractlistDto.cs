using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Data.DTOs
{
    public class ContractlistDto
    {
        public int? Contractid { get; set; }
        public string? Contractno { get; set; }
        public string? Ponumber { get; set; }
        public DateTime? Contractstartdate { get; set; }
        public DateTime? Contractenddate { get; set; }
        public decimal? Amount { get; set; }
        public int? Statusid { get; set; }
        public string? StatusName { get; set; }
        public string? customerCompanyName { get; set; }
        public string? DeliveryAnchorName { get; set; }
    }
}

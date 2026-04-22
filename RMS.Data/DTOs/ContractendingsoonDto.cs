using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Data.DTOs
{
    
        public class ContractendingsoonDto
        {
            public int? ContractId { get; set; }
            public string? ContractNo { get; set; }
        public string? Companyname { get; set; }
        public string? PoNumber { get; set; }
        public DateTime? ContractEndDate { get; set; }
            public string? DeliveryAnchorEmail { get; set; }
            public string? AccountManagerEmail { get; set; }
            public string? PracticeHeadEmail { get; set; }
            public TimeSpan? Duration { get; set; }
        }
    
}

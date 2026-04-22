using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Entity.DTO
{
    public class AccountManagerDetailsDto
    {
        public int AccountManagerId { get; set; }
        public string AccountManagerName { get; set; }
    }
    public class ProvisionBillingSubmittingDataDto
    {
        public int ProvisionBillingId { get; set; }
        public decimal? BilledAmount { get; set; }
    }
}

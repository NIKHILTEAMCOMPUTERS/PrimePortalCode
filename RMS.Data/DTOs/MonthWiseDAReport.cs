using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Data.Models
{
    public class MonthWiseDAReport
    {
        public string DAname {  get; set; }
        public int TAndMCount {  get; set; }
        public int FixedBidCount {  get; set; }
        public int PresalesCount {  get; set; }
        public int InternalCount {  get; set; }
        public int ProjectCount {  get; set; }
        public decimal? TotalActualBilling {  get; set; }
        public decimal? TotalProvisionBilling {  get; set; }
       public decimal? TotalPendingProvisions {  get; set; }
    }
}

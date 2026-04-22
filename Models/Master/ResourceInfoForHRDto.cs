using Newtonsoft.Json;

namespace RMS.Client.Models.Master
{
    //public class ResourceInfoForHRDto
    //{
    //    public string? TMC { get; set; }
    //    public string? Practice { get; set; }
    //    public string? SubPractice { get; set; }
    //    public string? Region { get; set; }
    //    public string? Function { get; set; }
    //    public string? ResourceName { get; set; }
    //    public string? Flag { get; set; }
    //    public string? BillableNonbillable { get; set; }
    //    public string? ProjectNames { get; set; }
    //    public string? ProjectTypes { get; set; }
    //    public string? CustomerNames { get; set; }
    //}


    public class ResourceInfoForHRDto
    {
        [JsonProperty("employeeid")]
        public int Employeeid { get; set; }

        [JsonProperty("tmc")]
        public string TMC { get; set; }

        [JsonProperty("practice", NullValueHandling = NullValueHandling.Ignore)]
        public string Practice { get; set; }

        // ⚠️ NOTE: Not returned by function currently
        [JsonProperty("practiceid")]
        public int? practiceid { get; set; }

        [JsonProperty("subPractice", NullValueHandling = NullValueHandling.Ignore)]
        public string SubPractice { get; set; }

        [JsonProperty("region", NullValueHandling = NullValueHandling.Ignore)]
        public string Region { get; set; }

        [JsonProperty("function", NullValueHandling = NullValueHandling.Ignore)]
        public string Function { get; set; }

        [JsonProperty("resourceName")]
        public string ResourceName { get; set; }

        [JsonProperty("flag")]
        public string Flag { get; set; }

        [JsonProperty("billableNonbillable")]
        public string BillableNonbillable { get; set; }

        [JsonProperty("projectNames")]
        public string ProjectNames { get; set; }

        [JsonProperty("projectTypes")]
        public string ProjectTypes { get; set; }

        [JsonProperty("customerNames")]
        public string CustomerNames { get; set; }

        // ✅ NEW FIELD (from function)
        [JsonProperty("customerCodes")]
        public string CustomerCodes { get; set; }
        // ✅ Actual customer company names (always from customer table)
        [JsonProperty("customerCompanyNames")]
        public string CustomerCompanyNames { get; set; }
        [JsonProperty("da")]
        public string DA { get; set; }
    }

}

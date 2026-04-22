using RMS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Entity.DTO
{
    public class VendorDto
    {
        public string? Vendorcode { get; set; }

        public int? Vendorid { get; set; }

        public string? Vendorname { get; set; }

        public string? Vendorcontact { get; set; }

        public string? Email { get; set; }

        public string? Phone1 { get; set; }

        public string? Phone2 { get; set; }

        public string? Pannumber { get; set; }

        public int? Currencyid { get; set; }

        public string? Gstnumber { get; set; }

        public int? Paymenttermid { get; set; }

        public int? Cityid { get; set; }

        public string? Zipcode { get; set; }

        public string? Address1 { get; set; }

        public string? Address2 { get; set; }

        public bool? Isactive { get; set; }

        public bool? Isdeleted { get; set; }

        public DateTime? Createdate { get; set; }

        public DateTime? Lastupdatedate { get; set; }

        public int? Createdby { get; set; }

        public int? Lastupdatedby { get; set; }

        public string? Udf1 { get; set; }

        public string? Udf2 { get; set; }

        public string? Udf3 { get; set; }

        public PaymentTermDto? PaymentTerm { get; set; }
        public CurrencyDto? Currency { get; set; }

        public CityInfo? City { get; set; }
        public List<Rmsemployee>? Employees { get; set; }
        public List<Department>? Departments { get; set; }
        public Branch branch { get; set; }
    
        
    }

 
}

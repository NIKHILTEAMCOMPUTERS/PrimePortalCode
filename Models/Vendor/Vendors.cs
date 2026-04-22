using RMS.Client.Models.Customer;
using RMS.Client.Models.Employee;
using RMS.Client.Models.Master;
using System.ComponentModel.DataAnnotations;

namespace RMS.Client.Models.Vendor
{
    public class Vendors
    {
        public int VendorId { get; set; }
        public int? vendorid { get; set; }
        public string VendorCode { get; set; }
        public string VendorName { get; set; }
        public string Vendorcontact { get; set; }
        public string Email { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string Pannumber { get; set; }
        public int? Currencyid { get; set; }
        public string Gstnumber { get; set; }
        public int? Paymenttermid { get; set; }
        public int? Cityid { get; set; }
        public string Zipcode { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public int? countryId { get; set; }
        public int? stateId { get; set; }
        public int? userid { get; set; }
        public string companyemail { get; set; }
        public int? PracticeId { get; set; }
        public string PracticeName { get; set; }
        public int? SubPracticeId { get; set; }
        public string SubPracticeName { get; set; }
    }

    public class VendorWithDetailDto
    {
        public string vendorcode { get; set; }
        public int vendorid { get; set; }
        public string vendorname { get; set; }
        public string vendorcontact { get; set; }
        public string email { get; set; }
        public string phone1 { get; set; }
        public string phone2 { get; set; }
        public string pannumber { get; set; }
        public int? currencyid { get; set; }
        public string gstnumber { get; set; }
        public int? paymenttermid { get; set; }
        public int? cityid { get; set; }
        public string zipcode { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public bool? isactive { get; set; }
        public bool? isdeleted { get; set; }
        public DateTime? createdate { get; set; }
        public DateTime? lastupdatedate { get; set; }
        public string createdby { get; set; }
        public string lastupdatedby { get; set; }
        public string udf1 { get; set; }
        public int? udf2 { get; set; }
        public string udf3 { get; set; }
        public PaymentTermDto paymentTerm { get; set; }
        public CurrencyDto currency { get; set; }
        public City city { get; set; }
        public List<EmployeeDt> employees { get; set; }
        public List <EmployeeDt> rmsemployees { get; set; }
        public int TotalEmployee { get; set; }
    }

    public class PaymentTermDto
    {
        public int paymentTermId { get; set; }
        public string paymentTermName { get; set; }
    }

    public class CurrencyDto
    {
        public int currencyId { get; set; }
        public string currencyName { get; set; }
    }

    public class City
    {
        public int cityId { get; set; }
        public string cityName { get; set; }
        public State state { get; set; }
    }

    public class State
    {
        public int stateId { get; set; }
        public string stateName { get; set; }
        public CountryDto country { get; set; }
    }

    public class CountryDto
    {
        public int countryId { get; set; }
        public string countryName { get; set; }
    }

    public class EmployeeDt
    {
        public int? employeeid { get; set; }
        public string userid { get; set; }
        public string Employeename { get; set; }
        public string companyemail { get; set; }
        public string contactno { get; set; }
        public int? designationid { get; set; }
        public string reportheadid { get; set; }
        public int? departmentid { get; set; }
        public bool? isactive { get; set; }
        public bool isdeleted { get; set; }
        public string? createdon { get; set; }
        public string? lastupdatedon { get; set; }
        public int? Udf2 { get; set; }
        public int? branchid { get; set; }
        public string dateofjoining { get; set; }
        public int? categorysubstatusid { get; set; }
        public int? subpracticeid { get; set; }
        public int? PracticeId { get; set; }
        public int? SubPracticeId { get; set; }
        public string SubPracticeName { get; set; }
        public int? VendorId { get; set; }
        public string VendorCode { get; set; }
        public string VendorName { get; set; }
        public int? Designationid { get; set; }
        public Branch branch { get; set; }
        public string TmcId { get; set; }
        public string email { get; set; }
        public Vendors? vendor { get; set; }
         
        
        public Subpractice? subpractice { get; set; }
        public List<Department> departments { get; set; }
    }

    public class Practice
    {
        public int? practiceid { get; set; }
    }
    public class Branch
    {
        public int Branchid { get; set; }
        public string branchcode { get; set; }
        public string branchname { get; set; }
        public bool? isactive { get; set; }
        public bool isdeleted { get; set; }
        public DateTime createddate { get; set; }
        public DateTime lastupdatedate { get; set; }
        public int createdby { get; set; }
        public int lastupdateby { get; set; }
        public List<object> rmsemployees { get; set; }
    }

    public class Department
    {
        public int Departmentid { get; set; }
        public string DepartmentName { get; set; }
        public string Description { get; set; }
        public bool? IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public int CreatedBy { get; set; }
        public int LastUpdateBy { get; set; }
       
    }

}

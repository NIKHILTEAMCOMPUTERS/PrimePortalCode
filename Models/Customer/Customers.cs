using RMS.Client.Models.Oaf;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;

namespace RMS.Client.Models.Customer
{

    //Insert / Update /Delete
    public class Customers
    {
        public int Customerid { get; set; }

        public int Customertypeid { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Companyname { get; set; }

        public string Companyemail { get; set; }
        //[Required(ErrorMessage = "Phone number is required.")]
        //[RegularExpression(@"^\d{10}$", ErrorMessage = "Please enter a valid 10-digit phone number.")]
        public string Phone1 { get; set; }

        //[RegularExpression(@"^\d{10}$", ErrorMessage = "Please enter a valid 10-digit phone number.")]
        public string Phone2 { get; set; }
        public string CustomerAddress1 { get; set; }
        public string CustomerAddress2 { get; set; }
        public string CustomerPhone1 { get; set; }
        public string CustomerPhone2 {  get; set; }
        public string Pannumber { get; set; }
        public int Currencyid { get; set; }
        public string Gstnumber { get; set; }
        public int Paymenttermid { get; set; }
        public int Cityid { get; set; }
        public string Zipcode { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Customercode { get; set; }
        public string Companylogourl { get; set; }

        public int Stateid { get; set; }
        public int Countryid { get; set; }

    }




    //Listing, CardView, DetailView
    public class CustomerWithDetailsDto
    {
        public string? Customercode { get; set; }

        public int CustomerId { get; set; }
        public string CustomerFirstName { get; set; }
        public string CustomerLastName { get; set; }
        public string CustomerCompanyName { get; set; }
        public string CustomerCompanyLogoUrl { get; set; }  
        public string customerCompanyLogoUrl { get; set; }
        public string PAN { get; set; }
        public string GST { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerPhone1 { get; set; }
        public string CustomerPhone2 { get; set; }
        public string CustomerAddress { get; set; }
        public string Address1 { get; set; }
        public string CustomerAddress2 { get; set; }
        public string customerAddress1 { get; set; }
        public string customerAddress2 { get; set; }
        public string countryregioncode { get; set; }
        public string ZipCode { get; set; }


        public PaymentTermDto PaymentTerm { get; set; }
        public CurrencyDto Currency { get; set; }

        public CityInfo City { get; set; }
        public int TotalProjects { get; set; }
        public List<ProjectInfoDto> ProjectsInfo { get; set; }
    }

    public class CityInfo
    {
        public int CityId { get; set; }
        public string CityName { get; set; }
        public StateInfo State { get; set; }
    }
    public class StateInfo
    {
        public int StateId { get; set; }
        public string StateName { get; set; }
        public CountryDto Country { get; set; }

    }
    public class CountryDto
    {
        public int CountryId { get; set; }
        public string CountryName { get; set; }
    }
    public class PaymentTermDto
    {
        public int PaymentTermId { get; set; }
        public string PaymentTermName { get; set; }
    }
    public class CurrencyDto
    {
        public int CurrencyId { get; set; }
        public string CurrencyName { get; set; }
    }

    public class ProjectInfoDto
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public int? TotalEmployees { get; set; }
        public decimal? TotalAmount { get; set; }
        public string ProjectType { get; set; }
        public string ProjrectSubPractice { get; set; }
        public string ProjectPractice { get; set; }
        public string ProjectStatus { get; set; }
        public List<ContractDto> ContractInfo { get; set; }

    }
    public class ContractDto
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public decimal? Amount { get; set; }
        public ProjectDto project { get; set; }
    }
    public class ProjectDto
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public customer customer { get; set; }  = null;
    }



}
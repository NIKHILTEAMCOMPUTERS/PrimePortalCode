using RMS.Client.Models.Master;

namespace RMS.Client.Models.Customer
{
    public class CustomerDetails
    {
        public int CustomerId { get; set; }
        public string CustomerFirstName { get; set; }
        public string CustomerLastName { get; set; }
        public string CustomerCompanyName { get; set; }
        public string CustomerCompanyLogoUrl { get; set; }
        public string Pan { get; set; }
        public string Gst { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerAddress { get; set; }
        public PaymentTerm PaymentTerm { get; set; }
        public City City { get; set; }
        public int TotalProjects { get; set; }
        public List<ProjectInfo> ProjectsInfo { get; set; }
    }

    //public class PaymentTerm
    //{
    //    public int PaymentTermId { get; set; }
    //    public string PaymentTermName { get; set; }
    //}

    //public class City
    //{
    //    public int CityId { get; set; }
    //    public string CityName { get; set; }
    //    public State State { get; set; }
    //}

    //public class State
    //{
    //    public int StateId { get; set; }
    //    public string StateName { get; set; }
    //    public Country Country { get; set; }
    //}

    //public class Country
    //{
    //    public int CountryId { get; set; }
    //    public string CountryName { get; set; }
    //}

    public class ProjectInfo
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public int TotalEmployees { get; set; }
        public double TotalAmount { get; set; }
        public string ProjectType { get; set; }
        public string ProjectSubPractice { get; set; }
        public string ProjectPractice { get; set; }
        public string ProjectStatus { get; set; }
        public List<ContractInfo> ContractInfo { get; set; }
    }

    public class ContractInfo
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double Amount { get; set; }
    }
}

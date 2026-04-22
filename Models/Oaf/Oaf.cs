using RMS.Client.Models.Contract;

namespace RMS.Client.Models.Oaf
{
    public class Oaf
    {
        public int oafid { get; set; }
        public string oafNo { get; set; }
        public string ponumber { get; set; }
        public string povalue { get; set; }
        public string orderdescription { get; set; }
        public string potermscondition { get; set; }
        public int customerid { get; set; }
        public string customername { get; set; }
        public string projectname { get; set; }
        public string projectmodel { get; set; }
        public int projecttypeid { get; set; }
        public string projecttype { get; set; }
        public int subpracticeid { get; set; }
        public string subpractice { get; set; }
        public int practiceid { get; set; }
        public string practice { get; set; }
        public string projectdescription { get; set; }
        public string contractno { get; set; }
        public int? contractId { get; set; }
        public DateTime? contractstartdate { get; set; }
        public DateTime? contractenddate { get; set; }
        public string clientcoordinator { get; set; }
        public string milestones { get; set; }
        public int? costsheetid { get; set; }
        public string costsheetName { get; set; }
        public string emailattachment { get; set; }
        public string proposalattachment { get; set; }
        public string poattachment { get; set; }
        public string costattachment { get; set; }
        public string email { get; set; }
        public string phonenumber { get; set; }
        public string address1 { get; set; }
       
        public string status { get; set; }
        public string? StatusName { get; set; }
        public object remarks { get; set; }
        public double? xvalue { get; set; }
        public decimal? totalamount { get; set; }
        public string createdBy { get; set; }
        public string? CreatedByName { get; set; }
        public DateTime? createdDate { get; set; }
        public string CustomerCompanyLogoUrl { get; set; }
        public DateTime createdOn { get; set; }
        public List<Oafline> oaflines { get; set; }
        public List<Oafchecklist> oafchecklists { get; set; }
        public customer customer { get; set; }
        public int? statusid { get; set; }
        public int? deliveryanchorid { get; set; }
        public int? accountmanagerid { get; set; }
        public decimal? Advanceamount { get; set; }
        public bool isResourceDeployed { get; set; }
        public decimal? Advancepercent { get; set; }
        public int? CommittedClientBillingDate { get; set; }
        public List<MilestonInfoDto>? Milestonlist { get; set; }
        public ActiveContract? ActiveContract { get; set; }
        public CostSheet? Costsheetdata { get; set; }
        public bool? isextended { get; set; } = false;
        public bool? IsExtendable { get; set; } = false;
    }

    public class Oafline
    {
        public int oaflineid { get; set; }
        public string lineno { get; set; }
        public string linedescription1 { get; set; }
        public string linedescription2 { get; set; }
        public decimal lineamount { get; set; }
    }

    public class Oafchecklist
    {
        public int oafchecklistid { get; set; }
        public string question { get; set; }
        public string clientresponse { get; set; }
        public bool isextra { get; set; }
        public int statusid { get; set; }
        public string status { get; set; }
        public object remarks { get; set; }
    }

    public class customer
    {

        public string companyname { get; set; }
        public string companylogourl { get; set; }
        public string companyemail { get; set; }
        public string phoneNumber { get; set; }
        public string address { get; set; }
        public int? customerid { get; set; }
        public string? customerCompanyName { get; set; }

    }
    public class MilestonInfoDto
    {
        public int? Id { get; set; }

        public int? Oafid { get; set; }

        public string Milestonedesc { get; set; } = null!;

        public decimal? Percentage { get; set; }

        public decimal? Amount { get; set; }
        public int? Days { get; set; }
    }
    public class ActiveContract
    {
        public string ContractNo { get; set; }
        public int ContractId { get; set; }
        public DateTime ContractStartDate { get; set; }
        public DateTime ContractEndDate { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
        public List<Resource> Resources { get; set; }
    }

    public class Resource
    {
        public int EmployeeId { get; set; }
        public string Tmc { get; set; }
        public string EmployeeName { get; set; }
    }

}


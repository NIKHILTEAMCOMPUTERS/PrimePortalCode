using RMS.Client.Extensions;
using System.Text.Json.Serialization;

namespace RMS.Client.Models.Master
{
    public class Projects
    {
        public int? ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string? ponumber { get; set; }
        public string ProjectDescription { get; set; }
        public int? CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string customerCompanyname { get; set; }
        //public int? ProjectModelId { get; set; }
        //public string ProjectModelName { get; set; }
        //public string ProjectHeadId { get; set; }
        public int? ProjectTypeId { get; set; }
        public string ProjectTypeName { get; set; }
        public int? PracticeId { get; set; }
        public string PracticeName { get; set; }
        public int? SubPracticeId { get; set; }
        public string SubPracticeName { get; set; }
        //public int? StatusId { get; set; }
        public int? ContractId { get; set; }
        public string ContractNo { get; set; }
        //public string PONumber { get; set; }
        public string ContractStartDate { get; set; }
        public string ContractEndDate { get; set; }
        public decimal? Amount { get; set; }
        public string ContactPersonName { get; set; }
        public string Projectno { get; set; }
        public string Status { get; set; }
       public int? Statusid {  get; set; }
        public string IsCompleted { get; set; }
        public int? AccountManagerId { get; set; }
        public bool poawaited { get; set; } = false;
        public int? AccounmanagerId { get; set; }
        public int? CommittedClientBillingDate { get; set; }

    }
    public class ProjectAddDto
    {
        public int? ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string ProjectNo { get; set; }
        public string ProjectDescription { get; set; }
        public int? CustomerId { get; set; }
        //public int ?ProjectModelId { get; set; }
        //public string ProjectHeadId { get; set; }
        public int? ProjectTypeId { get; set; }
        public int? SubPractiseId { get; set; }
        public int? accountmanagerid { get; set; }
        public int? CommittedClientBillingDate { get; set; }
    }

    public class ProjectDetail
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string ProjectDescription { get; set; }
        public DateTime ProjectCreatedOn { get; set; }
        public string ProjectCreatedBy { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerCompanyname { get; set; }
        public string CustomerEmailid { get; set; }
        public string CustomerPhoneno { get; set; }
        public string? accounmanagerName { get; set; }
        public string CustomerLocationAddress { get; set; }
        public string CustomerProfilePhoto { get; set; }
        //public int ProjectModelId { get; set; }
        //public string ProjectModelName { get; set; }
        //public string ProjectHeadId { get; set; }
        public int ProjectTypeId { get; set; }
        public string ProjectTypeName { get; set; }
        public int PracticeId { get; set; }
        public string PracticeName { get; set; }
        public int SubPracticeId { get; set; }
        public string SubPracticeName { get; set; }
        public int StatusId { get; set; }
        public int? ContractId { get; set; }
        public string ContractType { get; set; }
        public string ContractNo { get; set; }
        public string PoNumber { get; set; }

        public string ContractStartDate { get; set; }
        public string ContractEndDate { get; set; }
        public string Amount { get; set; }
        public string ContactPersonName { get; set; }
        public string ProjectNo { get; set; }
        public string Status { get; set; }
        public List<string> TeamMembers { get; set; }
        public List <Contract>? Contracts { get; set; }
    }
    public class Contract
    {
        public int? ContractId { get; set; }
        public string ContractNo { get; set; }
        public string PONumber { get; set; }
        public string ContractStartDate { get; set; }
        public string ContractEndDate { get; set; }
        public decimal? Amount { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
    }
}

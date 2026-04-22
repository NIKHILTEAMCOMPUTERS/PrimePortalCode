using RMS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Entity.DTO
{
    public class ProjectSaveAsDraftRequestDto
    {
        public int ?ProjectId { get; set; }
        public string? Projectname { get; set; }
        public string? Projectno { get; set; }

        public string? Projectdescription { get; set; }

        public int? Customerid { get; set; }    

        public int? Projectmodelid { get; set; }

        public string ? Projectheadid { get; set; }

        public int? Projecttypeid { get; set; }

        public int? Subpractiseid { get; set; }
        public int? Accountmanagerid { get; set; }
        public DateTime? Billingcycledate { get; set; }
        public int? CommittedClientBillingDate  { get; set; }



    }
    public class ProjectListResponseDto
    {
        public int? ProjectId { get; set; }
        public string? Projectname { get; set; }
        public int? PracticeId { get; set; }
        public string PracticeName { get; set; }
        public int? SubpracticeId { get; set; }
        public string ?SubPracticeName { get; set; }
        public DateTime? ContractEndDate { get; set; }
    }
    public class ResponseForProjectsWithDetailsDto
    {
        public  string? AccounmanagerName { get; set; }
        public int? AccounmanagerId { get; set; }
        public int Projectid { get; set; }

        public string? Projectname { get; set; }
        public string? Ponumber { get; set; }
        public string? Projectdescription { get; set; }
        public DateTime? ProjectCreatedOn { get; set; }
        public string? ProjectCreatedBy { get; set; }
        public DateTime? Billingcycledate { get; set; }
        public int? CommittedClientBillingDate { get; set; }
        
        //CustomerDetails 
        public int? Customerid { get; set; }
        public string CustomerName { get; set; }
        public string CustomerCompanyname { get; set; }
        public string CustomerEmailid { get; set; }
        public string CustomerPhoneno { get; set; }
        public string CustomerLocationAddress { get; set; }
        public string CustomerProfilePhoto { get; set; }

        public int? Projectmodelid { get; set; }
        public string ProjectmodelName { get; set; }

        public string? Projectheadid { get; set; }

        public int? Projecttypeid { get; set; }
        public string ProjecttypeName { get; set; }
        public int? PracticeId { get; set; }
        public string PracticeName { get; set; }

        public int? Subpracticeid { get; set; }
        public string SubpracticeName { get; set; }

       // public int? Statusid { get; set; }

        //project contract details
        public int? Contractid { get; set; }
        public string Contracttype { get; set; }    

        public string? Contractno { get; set; }
        public string? DeliveryAnchorName { get; set; }

        //public string? Ponumber { get; set; }   

        public DateTime? Contractstartdate { get; set; }

        public DateTime? Contractenddate { get; set; }

        public decimal? Amount { get; set; }

       // public string? Contactpersonname { get; set; }
        public string ProjectNo { get; set; }
        public string Status { get; set; }
        public List<Rmsemployee> TeamMembers { get; set; }
        public List<ContrtInfoDetails>  contracts { get; set; }

    }

    public class ContrtInfoDetails
    {
        public int Contractid { get; set; }

        public string? Contractno { get; set; }

        public string? Ponumber { get; set; }

        public DateTime? Contractstartdate { get; set; }

        public DateTime? Contractenddate { get; set; }

        public decimal? Amount { get; set; }
        public int? Statusid { get; set; }
        public string? Status { get; set; }

    }
     public class ProjectCheckDto
    {
        public int Projectid { get; set; }
        public string? Projectno { get; set; }
    }

    public class ActiveProjectRequest
    {
        public int EmployeeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }


    public class EmployeeReleaseRequest
    {
        public int ProjectionId { get; set; }
        public int ProjectionRequestId { get; set; }
        public int EmployeeId { get; set; }
        public int SentTo { get; set; }
        public string Status { get; set; }
        public string? Remarks { get; set; }

    }
}

using Microsoft.AspNetCore.Mvc.Rendering;
using RMS.Client.Models.Contract;

namespace RMS.Client.Models.Projection
{


    public class ResponseModelProjection
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }
        public List<Projection> Data { get; set; }
        public object ErrorDetails { get; set; }
    }
    public class Projection
    {
        public int Projectionid { get; set; }
        public string? ProjectionNo { get; set; }
        public string? ProjectionName { get; set; }
        public string? ProjectionDescription { get; set; }
        public int CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public int SubPracticeId { get; set; }
        public string? SubPracticeName { get; set; }
        public int? PracticeId { get; set; }
        public string? PracticeName { get; set; }
        public int? ProjectHeadId { get; set; }
        public string? ProjectHeadName { get; set; }
        public string? ProjectType { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? ProjectionCost { get; set; }
        public int? StatusId { get; set; }
        public string? StatusName { get; set; }
        public int CrmId { get; set; }
        public List<DeployedEmployee>? DeployedEmployees { get; set; }
        public List<Projectionrequest> ProjectionRequest { get; set; }
        public List<ProjectionInitialBilling> ProjectionInitialBilling { get; set; }
        public CostSheet? Costsheetdata { get; set; }
        public int? costsheetId { get; set; }


    }
    public class ProjectionInitialBilling
    {

        public int ProjectionInitialBillingId { get; set; }

        public int ProjectionId { get; set; }

        public string MonthYear { get; set; }

        public decimal Amount { get; set; }

    }
    public class DeployedEmployee
    {
        public int DeployedEmployeeId { get; set; }
        public string? DeployedEmployeeName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int CategorySubStatusId { get; set; }
        public string? CategorySubStatusName { get; set; }
        public string? Remarks { get; set; }
        public string? Status { get; set; }
        public int DeliveryAnchorId { get; set; }
        public string? DeliveryAnchorName { get; set; }

    }
    public class Employee_Skill_Exe_Dto
    {
        public int? SkillId { get; set; }
        public string? Experince { get; set; }
    }
    public class Projectionrequest
    {
        public int DeliveryAnchorId { get; set; }
        public string? DeliveryAnchorName { get; set; }
        public int Projectionrequestid { get; set; }

        public int Projectionid { get; set; }

        public int Employeeid { get; set; }

        public string? Remarks { get; set; }

        public string? Status { get; set; }
        public string? RequestFrom { get; set; }

        public int Requestsentby { get; set; }

        public int Requestsentto { get; set; }

        public int? Lastupdatedby { get; set; }

        public DateTime ContractStartDate { get; set; }

        public DateTime ContractEndDate { get; set; }

        public string ProjectionName { get; set; }

    }
    public class ProjectionViewModel
    {
        public Projection Projections { get; set; }
        public List<SelectListItem> Costsheets { get; set; }
    }
}
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Entity.DTO
{
    public class ProjectionRequestResponseDto
    {
        public int CrmId { get; set; }
        public int? Projectionid { get; set; }
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
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? ProjectionCost { get; set; }
        public int? StatusId { get; set; }
        public int? ProjectTypeId { get; set; }
        public string? ProjectType { get; set; }
        public string? StatusName { get; set; }

        public List<DeployedEmployee>? DeployedEmployees { get; set; }
        public List<ProjectionRequestDto> ProjectionRequest { get; set; }
        public List<ProjectionInitialBilling> ProjectionInitialBilling { get;set;}
        public int? CostsheetId { get; set; }
    }
    public class DeployedEmployee
    {
        public int DeployedEmployeeId { get; set; }
        public string? DeployedEmployeeName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int CategorySubStatusId { get; set; }
        public string? CategorySubStatusName { get; set; }
        public int DeliveryAnchorId { get; set; }
        public string? DeliveryAnchorName { get; set; }
        public string? Status { get; set; }
        public string? Remarks { get; set; }

    }
    public class Employee_Skill_Exe_Dto
    {
        public int? SkillId { get; set; }
        public string? Experince { get; set; }
    }
    public class ProjectionRequestDto
    {
        public int DeliveryAnchorId { get; set; }
        public string? DeliveryAnchorName { get; set; }
        public string? Status { get; set; }
        public string? Remarks { get; set; }
        public int EmployeeId { get; set; }
    }
    public class ProjectionInitialBilling
    {

        public int ProjectionInitialBillingId { get; set; }

        public int ProjectionId { get; set; }

        public string MonthYear { get; set; }

        public decimal Amount { get; set; }

    }
}

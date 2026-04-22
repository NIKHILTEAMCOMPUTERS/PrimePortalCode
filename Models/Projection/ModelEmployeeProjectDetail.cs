namespace RMS.Client.Models.Projection
{
    public class ModelEmployeeProjectDetail
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }
        public List<EmployeeProjectDetails> Data { get; set; }
        public object ErrorDetails { get; set; } 
    }

    public class EmployeeProjectDetails
    {
        public string? Tmc { get; set; }
        public string ?Practice { get; set; }
        public int? PracticeId { get; set; }
        public string? SubPractice { get; set; }
        public string? Region { get; set; }
        public string? Function { get; set; }
        public string? ResourceName { get; set; }
        public int? EmployeeId { get; set; }
        public string? GlobalStatus { get; set; }
        public string BillableNonBillable { get; set; }
        public string? ProjectName { get; set; }
        public string ?ProjectTypeName { get; set; }
        public string? CompanyName { get; set; }
        public DateTime? ContractStartDate { get; set; }
        public DateTime? ContractEndDate { get; set; }
        public int? CategorySubStatusId { get; set; }
        public string? EmpStatus { get; set; }
        public string? ContractStatus { get; set; }
        public string? Da { get; set; }
    }
}

namespace RMS.Client.Models.Employee
{
    public class ProjectAssign
    {
        public int employeeid { get; set; }
        public Employees employees { get; set; }
        public int categorysubstatusid { get; set; }
        
        public List<ProjectWithStatus> ProjectWithStatus { get; set; }
    }
    public class ProjectWithStatus
    {
        public int Projectid { get; set; }
        public int Categorysubstatusid { get; set; }
        public string? contractno { get; set; }
        public int? Contractid { get; set; }
    }
    public class EmployeeAssignDto
    {
        public List<EmployeeList> Employees { get; set; }
        public List<ProjectWithStatus> ProjectWithStatus { get; set; }
        public int contractId {  get; set; }


    }

    public class EmployeeList
    {
        public int Employeeid { get; set; }
        public int CategorySubStatusId { get; set; }
        public string? Udf3 { get; set; }
    }
}
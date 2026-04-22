namespace RMS.Client.Models.Projection
{
    public class ModelEmpExpList
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }
        public List<EmployeeExpData> Data { get; set; }
        public object ErrorDetails { get; set; } 
    }

    public class EmployeeExpData
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
    }
}

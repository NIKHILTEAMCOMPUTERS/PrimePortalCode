using RMS.Client.Models.Master;

namespace RMS.Client.Models.Dashboard
{
    public class HRDashboard
    {
        public List<Projects> Projects { get; set; }
        public int EmployeeCount { get; set; }
        public int EmployeeOverrun { get; set; }
        public int EmployeeOnbench { get; set; }
        public int EmployeeDeployed { get; set; }
        public int BenchFreshersCount { get; set; }
        public int BenchExpCount { get; set; }
        
    }
}
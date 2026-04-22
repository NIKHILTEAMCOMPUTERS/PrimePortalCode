namespace RMS.Client.Models.Master
{
    public class Department
    {
        public int departmentid { get; set; }
        public string departmentname { get; set; }
    }
    public class Branch
    {
        public int branchid { get; set; }
                
        public string branchname { get; set; }
        public string branchcode { get; set; }
    }
    public class Designation
    {
        public int designationid { get; set; }
        public string designationname { get; set;}
    }
}

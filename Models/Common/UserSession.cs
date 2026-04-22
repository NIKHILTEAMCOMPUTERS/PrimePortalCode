namespace RMS.Client.Models.Common
{
    public class UserSession
    {
        public int EmployeeId { get; set; }
        public string Token { get; set; }
        public string EmpCode { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string RoleName {  get; set; }
    }
}

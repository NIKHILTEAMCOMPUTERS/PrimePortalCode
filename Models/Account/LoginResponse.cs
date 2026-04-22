namespace RMS.Client.Models.Account
{
    public class LoginResponse
    {
        public int EmployeeId { get; set; }
        public string Token { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }
        public string Designation { get; set; }
        public string ReportingHeadId { get; set; }
        public string DateofJoining { get; set; }
        public string Department { get; set; }
        public string Role { get; set; }
        public bool CanSeeMemberTimesheet { get; set; }

    }
}

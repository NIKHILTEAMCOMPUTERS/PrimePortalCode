namespace RMS.Entity.Account
{
    public class AppUser
    {
        public int EmployeeId { get; set; }
        public string UserId { get; set; }      
        public string Name { get; set; }      
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Designation { get; set; }
        public string ReportingHeadId { get; set; }
        public string DateofJoining { get; set; }
        public string Department { get; set; }
        public bool CanSeeMemberTimesheet { get; set; }

    }
    public class UserRequetDto
    {
        public string TMC_Id { get; set; }
        public string Password { get; set; }
    }

    public class UserResponse
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public List<UserData> Data { get; set; }
    }

    public class UserData
    {
        public string Name { get; set; }
        public string UserId { get; set; }
        public string ContactNo { get; set; }
        public string CompanyEmail { get; set; }
        public string Designation { get; set; }
        public string SBU { get; set; }
        public string ReportingHeadId { get; set; }
        public string DateofJoining { get; set; }
        public string DateOfBirth { get; set; }
        public string Token { get; set; }
        public string LoginExpiry { get; set; }
        public string LoginUniqueId { get; set; }
        public string AsLoginUniqueId { get; set; }
        public string Admin { get; set; }
        public string Department { get; set; }
    }
}

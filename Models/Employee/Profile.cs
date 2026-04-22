namespace RMS.Client.Models.Employee
{
    public class Profile
    {

        public int? Employeeid { get; set; }
        public string Userid { get; set; }
        public string Employeename { get; set; }
        public string Companyemail { get; set; }
        public string Sbu { get; set; }
        public string Contactno { get; set; }
        public int? Designationid { get; set; }
        public string DesignationName { get; set; }
        public string Reportheadid { get; set; }
        public string ReportheadName { get; set; }
        public int? Departmentid { get; set; }
        public string DepartmentName { get; set; }
        public int? Branchid { get; set; }
        public string BranchName { get; set; }
        public string Dateofbirth { get; set; }
        public string Dateofjoining { get; set; }
        public List<EmployeeRoleDto> EmployeeRoles { get; set; }
        public EmployeeSkillDto PrimarySkill { get; set; }
        public List<EmployeeSkillDto> SecondarySkill { get; set; }
        public string SubPracticeName { get; set; }
        public string PracticeName { get; set; }
        public string Employeeregion { get; set; }
        public string Baseoffice { get; set; }
        public string Costcenter { get; set; }
        public string? Workexperience { get; set; }
        public int? Workexedays { get; set; }
        public string pastworkexperience { get; set; }
        public string teamworkexperience { get; set; }
    }

    public class EmployeeRoleDto
    {
        public int? Roleid { get; set; }
        public string Rolename { get; set; }
    }

    public class EmployeeSkillDto
    {
        public int? EmployeeskillId { get; set; }
        public int? SkillId { get; set; }
        public string SkillName { get; set; }
        public bool? Isprimary { get; set; }
        public decimal? ManagerRating { get; set; }
        public decimal? SelfRating { get; set; }
        public decimal? Experinceinmonth { get; set; } = 0;
    }
}


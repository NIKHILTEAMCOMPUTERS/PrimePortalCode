using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Entity.DTO
{

    public class ProjectemployeeassignmentRequestDto
    {
        public int Employeeid { get; set; }
        public int CategorySubStatusId { get; set; }
       
        public List<ProjectWithStatus> ProjectWithStatus { get; set; }
        public List<RoleInfo> Roles { get; set; }

    }
    public class ProjectWithStatus
    {
        public int Projectid { get; set; }
        public int Categorysubstatusid { get; set; }
        public string? contractno {  get; set; }
        public int? Contractid { get; set; }
    }
  public class RoleInfo
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }

    }

}

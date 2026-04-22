using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Entity.DTO
{

    public class EmployeeAssignDto
    {
        public List<EmployeeList> Employees { get; set; }
        public List<ProjectWithStatus> ProjectWithStatus { get; set; }
        public int contractId { get; set; } = 0;


    }

    public class EmployeeList
    {
        public int Employeeid { get; set; }
        public int CategorySubStatusId { get; set; }
    }

}
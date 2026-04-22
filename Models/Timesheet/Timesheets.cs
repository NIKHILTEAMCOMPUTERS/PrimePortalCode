using RMS.Client.Models.Customer;
using RMS.Client.Models.Master;

namespace RMS.Client.Models.Timesheet
{
    public class Timesheets
    {
        public int Timesheetid { get; set; }

        public int Employeeid { get; set; }

        public DateTime Timesheetdate { get; set; }

        public string Totalhours { get; set; } = null!;

        public bool Isdrafted { get; set; }

        public bool? Isactive { get; set; }

        public DateTime Createddate { get; set; }

        public DateTime Lastupdatedate { get; set; }

   //     public int Createdby { get; set; }

        public int Lastupdateby { get; set; }
        public string Timesheethistoryid { get; set; }
        //public virtual Rmsemployee Employee { get; set; } = null!;

        public List<TimesheetsDetailDTO> timesheetdetails { get; set; }
        public Employee Employee { get; set; }

    }
    public class TimesheetsDetailDTO
    {
        public bool Isdrafted { get; set; }
        public int? Timesheetdetailid { get; set; }

        public int? Timesheetid { get; set; }

        public int? Departmentid { get; set; }

        //public int? projectId { get; set; }
        //public int? customerid { get; set; }
        //public string? projectName { get; set; }

        public string Activity { get; set; } = null!;

        public string? Dayhours { get; set; } = null!;

        public string? Remarks { get; set; }
        public DateTime Timesheetdate { get; set; }
        public bool? Isactive { get; set; }

        public DateTime? Lastupdatedate { get; set; }

   //     public int Createdby { get; set; }

        public int? Lastupdateby { get; set; }
        public ContractDto contract { get; set; }

        public string? Benchstatus { get; set; }
        public int? ProjectId { get; set; }
        public int? CustomerId { get; set; }
        public string? Companyname { get; set; }
        public string? Projectname { get; set; }
        public int? categoryofactivityid { get; set; }

    }
    public class MembersTimesheet
    {      
        public List<Timesheets> timesheets { get; set; }
    }
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string userid { get; set; }
        public string employeename { get; set; }
        public string companyemail { get; set; }
    }

    public class SubmitAllTimesheet
    {
        public int tsmasterid {  get; set; }
        public List<Timesheets> timesheets { get; set; }
    }
    public class TimesheetMasterDTO
    {
        public int tsmasterid { get; set; }
        public string Monthname { get; set; }
        public int employeeid { get; set; }

        public string tsuniqueid { get; set; } = null!;

        public bool? isactive { get; set; }

        public bool isdeleted { get; set; }

        public DateTime createddate { get; set; }

        public DateTime lastupdatedate { get; set; }

  //      public int createdby { get; set; }
        public int lastupdateby { get; set; }
        public List<Timesheets> timesheets { get; set; }
       
    }
    //   {"tsmasterid":"5","timesheets":[{"Timesheetdate":"25 January 2024","timesheetdetails":
    //   [{"Departmentid":22,"Projectid":143,"Activity":"test","Hours":"3"}]}]}
    public class TimesheetHRDto
    {
        public string MonthYear { get; set; }
        public string ReportingHead { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeUserId { get; set; }
        public int DetailCount { get; set; }
    }
}


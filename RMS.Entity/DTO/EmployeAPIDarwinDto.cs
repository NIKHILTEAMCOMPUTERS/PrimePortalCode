using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Entity.DTO
{
    public class EmployeAPIDarwinDto
    {
        [JsonProperty("sbu")]
        public string Sbu { get; set; }

        [JsonProperty("sbu_code")]
        public string SbuCode { get; set; }

        [JsonProperty("employee_id")]
        public string EmployeeId { get; set; }

        [JsonProperty("date_of_birth")]
        public string DateOfBirth { get; set; }

        [JsonProperty("reporting_head_id")]
        public string ReportingHeadId { get; set; }

        [JsonProperty("base_office_location")]
        public string BaseOfficeLocation { get; set; }

        [JsonProperty("leavingdate")]
        public string LeavingDate { get; set; }

        [JsonProperty("employee_name")]
        public string EmployeeName { get; set; }

        [JsonProperty("designation")]
        public string Designation { get; set; }

        [JsonProperty("company_email_id")]
        public string CompanyEmailId { get; set; }

        [JsonProperty("office_mobile_no")]
        public string OfficeMobileNo { get; set; }

        [JsonProperty("joiningdate")]
        public string JoiningDate { get; set; }

        [JsonProperty("branch_code")]
        public string BranchCode { get; set; }

        [JsonProperty("employee_region")]
        public string EmployeeRegion { get; set; }

        [JsonProperty("holiday_calendar")]
        public string HolidayCalendar { get; set; }

        [JsonProperty("department")]
        public string Department { get; set; }
    }
    public class EmployeAPIDarwinDtoJson
    {
        [JsonProperty("sbu")]
        public string sbu { get; set; }

        [JsonProperty("sbu_code")]
        public string sbu_code { get; set; }

        [JsonProperty("employee_id")]
        public string employee_id { get; set; }

        [JsonProperty("date_of_birth")]
        public string date_of_birth { get; set; }

        [JsonProperty("reporting_head_id")]
        public string reporting_head_id { get; set; }

        [JsonProperty("base_office_location")]
        public string base_office_location { get; set; }

        [JsonProperty("leavingdate")]
        public string leavingdate { get; set; }

        [JsonProperty("employee_name")]
        public string employee_name { get; set; }

        [JsonProperty("designation")]
        public string designation { get; set; }

        [JsonProperty("company_email_id")]
        public string company_email_id { get; set; }

        [JsonProperty("office_mobile_no")]
        public string office_mobile_no { get; set; }

        [JsonProperty("joiningdate")]
        public string joiningdate { get; set; }

        [JsonProperty("branch_code")]
        public string branch_code { get; set; }

        [JsonProperty("employee_region")]
        public string employee_region { get; set; }

        [JsonProperty("holiday_calendar")]
        public string holiday_calendar { get; set; }

        [JsonProperty("department")]
        public string department { get; set; }

        [JsonProperty("cost_center")]
        public string cost_center { get; set; }

        [JsonProperty("cost_center_id")]
        public string CostCenterId { get; set; }

        [JsonProperty("past_work_experience")]
        public string past_work_experience { get; set; }

        [JsonProperty("department_code")]
        public string DepartmentCode { get; set; }

        [JsonProperty("date_of_resignation")]
        public string date_of_resignation { get; set; }

    }
}

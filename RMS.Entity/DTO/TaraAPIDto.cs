using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Entity.DTO
{
    public class TaraAPIResponse
    {
        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("employee_data")]
        public List<TaraEmployeeData> EmployeeData { get; set; }
    }

    public class TaraEmployeeData
    {
        [JsonProperty("EmployeeSBU")]
        public string EmployeeSBU { get; set; }

        [JsonProperty("Key")]
        public string Key { get; set; }

        [JsonProperty("TMC")]
        public string TMC { get; set; }

        [JsonProperty("Full_Name")]
        public string FullName { get; set; }

        [JsonProperty("Department")]
        public string Department { get; set; }

        [JsonProperty("Designation")]
        public string Designation { get; set; }

        [JsonProperty("Branch")]
        public string Branch { get; set; }

        [JsonProperty("SBU")]
        public string SBU { get; set; }

        [JsonProperty("SBUSpecified")]
        public bool SBUSpecified { get; set; }

        [JsonProperty("Reporting_Head_ID")]
        public string ReportingHeadID { get; set; }

        [JsonProperty("Date_of_Joining")]
        public string DateOfJoining { get; set; }

        [JsonProperty("Date_of_JoiningSpecified")]
        public bool DateOfJoiningSpecified { get; set; }

        [JsonProperty("Birth_Date")]
        public string BirthDate { get; set; }

        [JsonProperty("Birth_DateSpecified")]
        public bool BirthDateSpecified { get; set; }

        [JsonProperty("Comm_Addr")]
        public string CommAddr { get; set; }

        [JsonProperty("Phone_No")]
        public string PhoneNo { get; set; }

        [JsonProperty("Company_E_Mail")]
        public string CompanyEMail { get; set; }

        [JsonProperty("Personal_E_Mail")]
        public string PersonalEMail { get; set; }

        [JsonProperty("Bank_Name")]
        public string BankName { get; set; }

        [JsonProperty("Account_No")]
        public string AccountNo { get; set; }

        [JsonProperty("SBUCode")]
        public string SBUCode { get; set; }

        [JsonProperty("Mobile_Phone_No")]
        public string MobilePhoneNo { get; set; }

        [JsonProperty("Location_Code")]
        public string LocationCode { get; set; }

        [JsonProperty("First_Name")]
        public string FirstName { get; set; }

        [JsonProperty("Qualification_Code")]
        public string QualificationCode { get; set; }

        [JsonProperty("Gender")]
        public int? Gender { get; set; }

        [JsonProperty("GenderSpecified")]
        public bool GenderSpecified { get; set; }

        [JsonProperty("Confirmation_Date")]
        public string ConfirmationDate { get; set; }

        [JsonProperty("Confirmation_DateSpecified")]
        public bool ConfirmationDateSpecified { get; set; }

        [JsonProperty("Marital_Status")]
        public int? MaritalStatus { get; set; }

        [JsonProperty("Marital_StatusSpecified")]
        public bool MaritalStatusSpecified { get; set; }

        [JsonProperty("Entitlement_to_ESI")]
        public bool? EntitlementToESI { get; set; }

        [JsonProperty("Entitlement_to_ESISpecified")]
        public bool EntitlementToESISpecified { get; set; }

        [JsonProperty("Is_Confirmed")]
        public bool? IsConfirmed { get; set; }

        [JsonProperty("Is_ConfirmedSpecified")]
        public bool IsConfirmedSpecified { get; set; }

        [JsonProperty("Probation_Status")]
        public int? ProbationStatus { get; set; }

        [JsonProperty("Probation_StatusSpecified")]
        public bool ProbationStatusSpecified { get; set; }

        [JsonProperty("HR_Admin")]
        public bool? HRAdmin { get; set; }

        [JsonProperty("HR_AdminSpecified")]
        public bool HRAdminSpecified { get; set; }

        [JsonProperty("SUBBU_Code")]
        public string SUBBUCode { get; set; }

        [JsonProperty("Employee_Band")]
        public string EmployeeBand { get; set; }

        [JsonProperty("Date_Of_Exit")]
        public string DateOfExit { get; set; }

        [JsonProperty("Group_Company")]
        public string GroupCompany { get; set; }

        [JsonProperty("isTcplEngineer")]
        public string IsTcplEngineer { get; set; }

        [JsonProperty("cost_center")]
        public string CostCenter { get; set; }

        [JsonProperty("cost_center_id")]
        public string CostCenterId { get; set; }

        [JsonProperty("past_work_experience")]
        public string PastWorkExperience { get; set; }

        [JsonProperty("department_code")]
        public string DepartmentCode { get; set; }

        [JsonProperty("holiday_calendar")]
        public string HolidayCalendar { get; set; }
    }
}

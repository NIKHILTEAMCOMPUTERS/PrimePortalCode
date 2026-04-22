using RMS.Data.DTOs;
using RMS.Data.Models;
using RMS.Entity.DTO;
using System.Collections;

namespace RMS.Service.Interfaces.Master
{
    public interface IEmployeeRepository : IGenericRepository<Rmsemployee>
    {
        public Task<Rmsemployee> GetByName(string name);
        public Task<Rmsemployee> GetByTmc(string tmc);
        public Task<bool> UpsertEmployeesOnebyOne(List<EmployeAPIDarwinDto> emplooyeeList);
        public Task<bool> UpsertEmployeesBulkData(List<EmployeAPIDarwinDtoJson> employeeList);
        public Task<bool> UpsertCRMBulkData(CRMresponseDto crmdata);
        public Task<bool> CheckMemberCanSeeResourceTimesheet(string tmc);
        //public Task<List<employeeResponseDto>> GetEmployeesWithDetails();
        public Task<List<employeeResponseDto>> GetEmployeesWithDetails();
        // public Task<employeeResponseDto> GetEmployeesByIdWithDetails(int EmployeeId);
        public Task<employeeResponseDto> GetEmployeesByIdWithDetails(int EmployeeId);

        public Task<List<employeeResponseDto>> GetEmployeesOfDA(int DeliveryAnchorId);
        public Task<List<EmployeeDetailDto>> GetEmployeelistStoredProcedure();
        //public Task<List<rptContractBillingDto>> GetReportByMonthYearProcedure(string monthyear);

        //public Task<IEnumerable> GetReportByMonthYearProcedure_DA_Wise(string monthyear);

        public Task<Response> EmployeeResignedStatus(int id, Rmsemployee employeeResponseDto, JwtLoginDetailDto LoginDetails);
        public Task<Response> AdeUpdation(adeUpdationDto Value, JwtLoginDetailDto LoginDetails);

        public Task<Response> UpdateEmployeeSubpractice(RequestSubpracticeUpdationDto Value, JwtLoginDetailDto LoginDetails);

        //public Task<IEnumerable> GetRptProvisionHistory(string monthyear, int contractid);
        //public  Task<List<rptContractBillingProvisionDto>> GetReportByMonthYearProcedureProvision(string monthyear);
        public Task<List<employeeResponseDto>> GetEmployees();

        public Task<List<object>> GetAllBillingDataReourceWiseNew();
        public Task<object> GetProfile(JwtLoginDetailDto logindetasils);
        public Task<List<employeeResponseDto>> GetEmployeesForCount();

        public Task<List<DataForEmployeeReport>> GetEmployeeDetailsView(int? id);
        public Task<List<DaListDto>> DaList();
        public Task<List<employeeResponseDto>> GetEmployeesBySkill(int skillId);

    }
}
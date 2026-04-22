using RMS.Data.DTOs;
using RMS.Data.Models;
using RMS.Entity.DTO;
using System.Collections;

namespace RMS.Service.Interfaces.Master
{
    public interface IReportRepository 
    {

        public Task<List<rptContractBillingDto>> GetReportByMonthYearProcedure(string monthyear);
        public Task<IEnumerable> GetReportByMonthYearProcedure_DA_Wise(string monthyear);
        public Task<List<MonthWiseDAReport>> GetReportByMonthYearProcedure_DA_WiseForMailer(string monthyear);
        public Task<IEnumerable> GetRptProvisionHistory(string monthyear, int contractid);
        public Task<List<rptContractBillingProvisionDto>> GetReportByMonthYearProcedureProvision(string monthyear, int? DeliveryAnchorId);
        public Task<List<BillingDropdownMapperDto>> BillingDropDown();
        public Task<List<rptContractBillingActualMapperDto>> GetReportByMonthYearProcedureActualBilling(string monthyear, int? DeliveryAnchorId);
        public  Task<List<ResourceInfoForHRDto>> GetResourcesInfoForHR();
    }
}
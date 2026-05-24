using RMS.Entity.DTO;

namespace RMS.Service.Interfaces.Transection
{
    public interface IProvisionManagementRepository
    {
        Task<List<ProvisionManagementListDto>> GetProvisionList(string monthYear, JwtLoginDetailDto loginDetails);
        Task<Response> CarryForward(ProvisionActionDto dto, JwtLoginDetailDto loginDetails);
        Task<Response> ReverseProvision(ProvisionActionDto dto, JwtLoginDetailDto loginDetails);
        Task<List<ProvisionHistoryDto>> GetProvisionHistory(int provisionId);
        Task<Response> UpdateEstimatedBillingDate(UpdateEstimatedBillingDateDto dto, JwtLoginDetailDto loginDetails);
        Task<Response> UpdatePaidAmount(UpdatePaidAmountDto dto, JwtLoginDetailDto loginDetails);
        // Returns list of auto-reversed provisions so the caller can send email notifications
        Task<List<ProvisionManagementListDto>> ProcessMonthlyCarryForward();
    }
}

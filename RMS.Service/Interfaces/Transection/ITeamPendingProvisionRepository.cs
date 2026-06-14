using RMS.Entity.DTO;

namespace RMS.Service.Interfaces.Transection
{
    public interface ITeamPendingProvisionRepository
    {
        Task<List<TeamPendingProvisionListDto>> GetPendingList(JwtLoginDetailDto loginDetails);
        Task<Response> UpdateCloserDate(TeamUpdateCloserDateDto dto, JwtLoginDetailDto loginDetails);
        Task<Response> UpdateDocumentNo(TeamUpdateDocumentNoDto dto, JwtLoginDetailDto loginDetails);
        Task<Response> RecordBilled(TeamRecordBilledDto dto, JwtLoginDetailDto loginDetails);
        Task<List<TeamProvisionHistoryDto>> GetHistory(int provisionId);
    }
}

using RMS.Data.Models;
using RMS.Entity.DTO;

namespace RMS.Service.Interfaces.Master
{
    public interface ICostSheetRepository : IGenericRepository<Costsheet>
    {
        public Task<Costsheet> GetByName(string name);
        public Task<List<CostSheetDto>> GetCostSheetWithDetails();
        public Task<CostSheetDto> GetCostSheetById(int id);
        public Task<CostSheetDto> UpsertCostsheet(CostSheetDto value, JwtLoginDetailDto logindetails);
        public Task<Response> DeleteCostsheet(int CostSheetId);

        public Task<bool> CheckCostsheet(int? costSheetid, string CostSheetName);



    }
}
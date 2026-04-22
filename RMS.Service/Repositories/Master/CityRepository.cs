using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RMS.Data.Models;
using RMS.Service.Interfaces.Master;

namespace RMS.Service.Repositories.Master
{
    public class CityRepository : GenericRepository<City>, ICityRepository
    {
        private readonly RmsDevContext _context;
        public CityRepository(RmsDevContext context) : base(context)
        {
            _context = context;
        }

        public async Task<City> GetByName(string name)
        {
            return await _context.Cities.AsNoTracking().Where(c=>c.Cityname == name).FirstOrDefaultAsync();
        }
        public async Task<List<City>> GetCitiesByStateId(int StateId)
        {
            return await _context.Cities.AsNoTracking().Where(s=>s.Stateid==StateId).ToListAsync();
        }

        public async  Task<List<CrmAggregatedDataFromAllTable>> GetCRMdataAsyncAsync()
        {
            var result = await _context.CrmAggregatedDataFromAllTables.ToListAsync();

            var data = result.Where(deal => deal.Stageposition == 5 || deal.Stageposition == 7 || deal.Stageposition == 6)
                    .OrderByDescending(deal => deal.CreatedAt)
                    .ToList();


            return data;
        }
    }
}
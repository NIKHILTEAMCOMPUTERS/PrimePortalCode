using RMS.Data.Models;
using RMS.Entity.DTO;
using System.Collections;

namespace RMS.Service.Interfaces.Master
{
    public interface IAccountManagerRepository : IGenericRepository<Rmsemployee>
    {
   
        
        public Task<List<AccountManagerDetailsDto>> GetAccountManagerList();
        public Task<IEnumerable> GetDAList();
        public Task<Response> ProvisionBillingSubmission(List<ProvisionBillingSubmittingDataDto> obj);



    }


}
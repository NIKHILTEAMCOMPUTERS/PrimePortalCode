using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using RMS.Data.Models;
using RMS.Entity.DTO;
using System.Collections;

namespace RMS.Service.Interfaces.Master
{
    public interface IOafRepository : IGenericRepository<Oaf>
    {
        //new chnages 
        public Task<Oaf> GetByName(string name);
        // public  Task<List<ContractListResponseDto>> GetList();
        public Task<IEnumerable> GetList(JwtLoginDetailDto LoginDetails);
        public Task<IEnumerable> GetOafListByDAId(int DeliveryAnchorId);
        public Task<OafDto> GetOafById(int id);
     
        public Task<Response> UpsertOaf(int? OafId, OafDto Value, IFormFile Emailattachment, IFormFile Proposalattachment,
            IFormFile Poattachment, IFormFile Costattachment, JwtLoginDetailDto LoginDetails);

        public Task<Response> ApproverAction(OafDto Value, JwtLoginDetailDto LoginDetails);
        public Task<Response> OAFwiseResourceAllotment(OAFwiseResourceAllotmentDto Value, JwtLoginDetailDto Userdetails);

        public Task<Response> ExtendOaf(ExtendOafDto value, JwtLoginDetailDto LoginDetails);


    }
}
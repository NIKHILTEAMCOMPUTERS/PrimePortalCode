using Microsoft.AspNetCore.Http;
using RMS.Data;
using RMS.Data.DTOs;
using RMS.Data.Models;
using RMS.Entity.DTO;

namespace RMS.Service.Interfaces.Master
{
    public interface IContractRepository : IGenericRepository<Projectcontract>
    {
        //new chnages 
        public Task<Projectcontract> GetByName(string name);
        // public  Task<List<ContractListResponseDto>> GetList();
        public Task<List<ContractRequestDto>> GetList();
        public Task<ContractRequestDto> GetContractById(int id);
        public Task<List<ContractRequestDto>> GetContractByAnchorId(int id);
        public Task<Response> UpsertContracts(int? ContractId, ContractRequestDto Value, IFormFile Attachment, JwtLoginDetailDto LoginDetails);
        public Task<Response> UpdateContractStatus(int? ContractId, ContractRequestDto Value,JwtLoginDetailDto LoginDetails, int? newcontractid);
        public Task<bool> CheckContractNo(int? ContractId,string Contractno);
        public Task<bool> CheckPoNo(int? ContractId, string PoNo);
        public Task<List<ContractRequestDto>> GetContractlistByProcedure(int? deliveryAnchorId);
        public Task<int> ContractCount();
        public Task<List<ContractRequestDto>> GetListPagination(int pageNumber, int pageSize);
        public Task<object> ForeclosureBillingAction(ForeclosureInputDTO value,JwtLoginDetailDto logindetails);
        public Task<object> CheckContractForeClosure(ContractCheckDto value);
        public Task<List<ContractendingsoonDto>> getcontractsendingsoon();
        public Task<Response> projectestimationstatusupdate(ContractRequestDto Value, JwtLoginDetailDto LoginDetails);
    }
}
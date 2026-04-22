using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RMS.Data;
using RMS.Data.Models;
using RMS.Entity.DTO;
using System.Collections;

namespace RMS.Service.Interfaces.Master
{
    public interface IContractBillinRepository : IGenericRepository<Contractbilling>
    {       
        public Task<List<Contractbilling>> GetList();
        public Task<Contractbilling> GetContractBillingsById(int id);       
        public Task<Response> UpsertContractBillings(List<ContractEmpBillingDto> Value, JwtLoginDetailDto LoginDetails);
        public Task<List<ResponseForBillingDetailsDto>> BilligDetails(RequestforBillingDetailsDto Value);
        public Task<Response> Provision_UpsertContractBillings(List<ContractEmpBillingDto> Value, JwtLoginDetailDto LoginDetails);
        public Task<Response> UpsertSendForApproval_Provision(JwtLoginDetailDto logindetails, List<BillingApprvalDto> Value);
        public Task<IEnumerable> GetListForApprovals_Provision(JwtLoginDetailDto logindetails);
        public  Task<Contractbillingprovesion> Provision_GetContractBillingsById(int id);
        public Task<Contractbillingprovesion> Provision_delete(int contractbillingid);
        public Task<Response> UpsertApproverAction_Provision(List<ApproverActionDto> Value, JwtLoginDetailDto logindetails);
        public Task<Response> UpdateBillingStatus(List<BillingStatusUpdateRequestDto> value, IFormFile file, JwtLoginDetailDto logindetails);

        public Task<Response> UpdateActualBilling(List<BillingApprvalDto> Value, JwtLoginDetailDto logindetails);

        public Task<Response> SendApprovalActualBilling(List<BillingApprvalDto> Value, JwtLoginDetailDto logindetails);
        

        public Task<IEnumerable> ListofActualContractbillingForApproval(JwtLoginDetailDto logindetails);

        public Task<IEnumerable> GetProvisionRequestProjectWise(JwtLoginDetailDto logindetails);

        public Task<Response> SwapFromProvisionToActual(List<SwappingRequestDto> values, JwtLoginDetailDto loginDetails);

        public Task<dynamic> GetEmployeeActualBillins(GetEmployeeActualBillinDto Value);


    }


    
}
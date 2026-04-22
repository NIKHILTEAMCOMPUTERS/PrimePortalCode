using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RMS.Data.Models;
using RMS.Entity.DTO;
using RMS.Extensions;
using RMS.Service.Interfaces;
using System.Diagnostics.Contracts;
using System.Security.Claims;
using System.Security.Cryptography.Xml;

namespace RMS.Controllers.Master
{
    public class ContractBillingController : BaseApiController
    {
        private readonly IUnitOfWork _uow;
        public ContractBillingController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpGet]
        public async Task<IEnumerable<Contractbilling>> Get()
        {            
          return await _uow.ContractBillinRepository.GetList();            
        }
        [HttpGet("{id}")]
        public async Task<Contractbilling> Get(int id)
        {
             return  await _uow.ContractBillinRepository.GetContractBillingsById(id);               
               
        }
        [HttpGet("ProvisionGet/{id:int}")]
        public async Task<IActionResult> ProvisionGet(int id)
        {
            var result= await _uow.ContractBillinRepository.Provision_GetContractBillingsById(id);
            if (result == null)
            {
                return NotFound("No data found");
            }
            return Ok(result);

        }

        [HttpGet("GetBillings")]
        public async Task<IActionResult> GetBillings( [FromBody] RequestforBillingDetailsDto value)
        {
            if (value == null)
            {
                return BadRequest("Please send data/ it won't accept empty");
            }

            var result = await _uow.ContractBillinRepository.BilligDetails(value);
            return Ok(result);
        }


        [HttpPost,Authorize]      
        public async Task<IActionResult> Post([FromBody] List<ContractEmpBillingDto> Value)
        {
            JwtLoginDetailDto Userdetails = new JwtLoginDetailDto();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
           
            if (identity == null)
                return BadRequest("Authentication Fails");
            else
            {          
                if (Value == null)
                {
                    return BadRequest("Invalid request data.");
                }      
               
                var LoginDetails = AuthenticUserDetails.GetCurrentUserDetails(identity);                
                var result = await _uow.ContractBillinRepository.UpsertContractBillings(Value,LoginDetails);
                return Ok(result);
               
            }
        }

        [HttpPost("BilligProvision"), Authorize]
        public async Task<IActionResult> BilligProvision([FromBody] List<ContractEmpBillingDto> Value)
        {
            JwtLoginDetailDto Userdetails = new JwtLoginDetailDto();
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity == null)
                return BadRequest("Authentication Fails");
            else
            {
                if (Value == null)
                {
                    return BadRequest("Invalid request data.");
                }

                var LoginDetails = AuthenticUserDetails.GetCurrentUserDetails(identity);
                var result = await _uow.ContractBillinRepository.Provision_UpsertContractBillings(Value, LoginDetails);
                return Ok(result);

            }
        }
        //[HttpPut("{id:int}"), Authorize]
        //public async Task<IActionResult> Put(int id , [FromBody] List<ContractEmpBillingDto> Value)
        //{
        //    JwtLoginDetailDto Userdetails = new JwtLoginDetailDto();
        //    var identity = HttpContext.User.Identity as ClaimsIdentity;

        //    if (id <=0)
        //    {
        //        return BadRequest("ContractBilling ID can not be zero/less tahn zero");
        //    }
        //    if (identity == null)
        //        return BadRequest("Authentication Fails");
        //    else
        //    {

        //        if (Value == null)
        //        {
        //            return BadRequest("Invalid request data.");
        //        }
        //        if (id != Value.FirstOrDefault().ConractEmployeeId)
        //        {
        //            return BadRequest(" Both the IDs inside json data and querystring are not same ");
        //        }

        //        var LoginDetails = AuthenticUserDetails.GetCurrentUserDetails(identity);
        //        var result = await _uow.ContractBillinRepository.UpsertContracts(Value,LoginDetails);
        //        return Ok(result);

        //    }
        //}

        [HttpDelete("{id}"),Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            //    _uow.ProjectRepository.Delete(id);
            //    return await _uow.Complete();
            var deletedItem = await _uow.ContractBillinRepository.Provision_delete(id);

            if (deletedItem == null)
            {
                return NotFound("No data found for deletion.");
            }

            return Ok(deletedItem);

        }

        [HttpPost("SendForApproval"),Authorize]
        public async Task<IActionResult> SendForApproval([FromBody] List<BillingApprvalDto>  Value)
        {
            JwtLoginDetailDto Userdetails = new JwtLoginDetailDto();
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity == null)
                return BadRequest("Authentication Fails");
            else
            {
                if (Value == null)
                {
                    return BadRequest("Invalid request data.");
                }

                var LoginDetails = AuthenticUserDetails.GetCurrentUserDetails(identity);
                var result = await _uow.ContractBillinRepository.UpsertSendForApproval_Provision(LoginDetails, Value);
                return Ok(result);
            }
            
        }
        //[HttpGet("approvallist"),Authorize]
        [HttpGet("approvallist"), Authorize]
        public async Task<IActionResult> GetApprovalList()
        {
            JwtLoginDetailDto Userdetails = new JwtLoginDetailDto();
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity == null)
                return BadRequest("Authentication Fails");
            else
            {
                var LoginDetails = AuthenticUserDetails.GetCurrentUserDetails(identity);
                var result = await _uow.ContractBillinRepository.GetListForApprovals_Provision(LoginDetails);
                if(result == null)
                {
                    return NotFound("No data found for approval");
                }
                return Ok(result);  
                
            }

        }
        [HttpGet("GetListProvisionRequestProjectWise"), Authorize]
        public async Task<IActionResult> GetProvisionRequestProjectWise()
        {
            JwtLoginDetailDto Userdetails = new JwtLoginDetailDto();
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity == null)
                return BadRequest("Authentication Fails");
            else
            {
                var LoginDetails = AuthenticUserDetails.GetCurrentUserDetails(identity);
                var result = await _uow.ContractBillinRepository.GetProvisionRequestProjectWise(LoginDetails);
                if (result == null)
                {
                    return NotFound("No data found for approval");
                }
                return Ok(result);

            }

        }

        [HttpPost("ApproverAction"), Authorize]
        public async Task<IActionResult> ApproverAction([FromBody] List<ApproverActionDto> Value)
        {
            JwtLoginDetailDto Userdetails = new JwtLoginDetailDto();
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity == null)
                return BadRequest("Authentication Fails");
            else
            {
                if (Value == null)
                {
                    return BadRequest("Invalid request data.");
                }

                var LoginDetails = AuthenticUserDetails.GetCurrentUserDetails(identity);
                var result = await _uow.ContractBillinRepository.UpsertApproverAction_Provision(Value, LoginDetails);
                return Ok(result);

            }
        }
        [HttpPost("UpdateBillingStatus"),Authorize]
        public async Task<IActionResult> UpdateBillingSataus([FromForm] JsonWithFileDto DtoObject)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity == null)
            {
                return BadRequest("Authentication Fails");
            }
                var LoginDetails = AuthenticUserDetails.GetCurrentUserDetails(identity);
                var BillingObj = JsonConvert.DeserializeObject<List<BillingStatusUpdateRequestDto>>(DtoObject.JasonData);
                var photos = DtoObject.Files;
            if (photos != null)
            {
                foreach (var files in photos)
                {
                    var extension = Path.GetExtension(files.FileName).ToLower(); // Convert to lower case for case-insensitive comparison
                    if (extension != ".jpg" && extension != ".jpeg" && extension != ".gif" && extension != ".png"
                        && extension != ".pdf" && extension != ".xls" && extension != ".xlsx"
                        && extension != ".doc" && extension != ".docx")
                    {
                        return BadRequest("Only image formats (jpg, png, gif), PDF, Excel, and Word documents are accepted ");
                    }
                }
            }
            var fileToUpload = photos?.FirstOrDefault();
            var result = await _uow.ContractBillinRepository.UpdateBillingStatus(BillingObj, fileToUpload, LoginDetails);
                return Ok(result);
        }

        [HttpPost("UpdateActualBilling"),Authorize]
        public async Task<IActionResult> UpdateActualBilling([FromBody] List<BillingApprvalDto> Value)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity == null)
            {
                return BadRequest("Authentication Fails");
            }
            var LoginDetails = AuthenticUserDetails.GetCurrentUserDetails(identity);
            var result = await _uow.ContractBillinRepository.UpdateActualBilling(Value,LoginDetails);
            return Ok(result);  
        }

        [HttpPost("SendApprovalActualBilling"), Authorize]
        public async Task<IActionResult> SendApprovalActualBilling([FromBody] List<BillingApprvalDto> Value)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity == null)
            {
                return BadRequest("Authentication Fails");
            }
            var LoginDetails = AuthenticUserDetails.GetCurrentUserDetails(identity);
            var result = await _uow.ContractBillinRepository.SendApprovalActualBilling(Value, LoginDetails);
            return Ok(result);
        }
        [HttpGet("GetActualBillingApprovalList"),Authorize]
        public async Task<IActionResult> ListofActualContractbillingForApproval()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity == null)
            {
                return BadRequest("Authentication Fails");
            }
            var LoginDetails = AuthenticUserDetails.GetCurrentUserDetails(identity);

            var result= await _uow.ContractBillinRepository.ListofActualContractbillingForApproval(LoginDetails);
            return Ok(result);
        }
        [HttpPost("SwapFromProvisionToActual")]
        public async Task<IActionResult> SwapFromProvisionToActual([FromBody] List<SwappingRequestDto> Value)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity == null)
            {
                return BadRequest("Authentication Fails");
            }
            var LoginDetails = AuthenticUserDetails.GetCurrentUserDetails(identity);

            var result = await _uow.ContractBillinRepository.SwapFromProvisionToActual(Value, LoginDetails);
             return Ok(result);         

        }
        [HttpPost("GetEmployeeActualBillins")]
        public async Task<IActionResult> GetEmployeeActualBillins([FromBody] GetEmployeeActualBillinDto Value)
        {
            var result = await _uow.ContractBillinRepository.GetEmployeeActualBillins(Value);
            return Ok(result);

        }

    }
    
}
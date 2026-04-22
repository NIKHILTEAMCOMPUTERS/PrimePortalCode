using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RMS.Data;
using RMS.Data.DTOs;
using RMS.Data.Models;
using RMS.Entity.DTO;
using RMS.Extensions;
using RMS.Service.Interfaces;
using System.Diagnostics.Contracts;
using System.Security.Claims;
using System.Security.Cryptography.Xml;

namespace RMS.Controllers.Master
{
    public class ContractController : BaseApiController
    {
        private readonly IUnitOfWork _uow;
        public ContractController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpGet]
        public async Task<IEnumerable<ContractRequestDto>> Get()
        {            
          return await _uow.ContractRepository.GetList();            
        }
        [HttpGet("GetContractsByProcedure")]
        public async Task<IEnumerable<ContractRequestDto>> GetContractsByProcedure()
        {
            return await _uow.ContractRepository.GetContractlistByProcedure(null);
        }

        [HttpGet("GetContractsByProcedure/{DeliveryAnchorId:int}")]
        public async Task<IEnumerable<ContractRequestDto>> GetContractsByProcedure(int DeliveryAnchorId)
        {
            return await _uow.ContractRepository.GetContractlistByProcedure(DeliveryAnchorId);
        }

        [HttpGet("GetContractsByPagination")]
        public async Task<ActionResult<IEnumerable<ContractRequestDto>>> GetContractsByPagination(int pageNumber = 1, int pageSize = 10)
        {
           
            var totalRecords = await _uow.ContractRepository.ContractCount();
            var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

            
            pageNumber = Math.Max(1, pageNumber);
            pageNumber = Math.Min(totalPages, pageNumber);

            // Get the paginated list
            var paginatedList = await _uow.ContractRepository.GetListPagination(pageNumber, pageSize);

            
            Response.Headers.Add("X-Pagination", Newtonsoft.Json.JsonConvert.SerializeObject(new { pageNumber, pageSize, totalRecords, totalPages }));

            return Ok(paginatedList);
        }


        [HttpGet("{id}")]
        public async Task<ContractRequestDto> Get(int id)
        {
            var result= await _uow.ContractRepository.GetContractById(id);
            return result; 
        }

        [HttpPost,Authorize]      
        public async Task<IActionResult> Post([FromForm] JsonWithFileDto DtoObject)
        {
            JwtLoginDetailDto Userdetails = new JwtLoginDetailDto();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
           
            if (identity == null)
                return BadRequest("Authentication Fails");
            else
            {
                

                var Value = JsonConvert.DeserializeObject<ContractRequestDto>(DtoObject.JasonData);
                if (Value.Statusid == 6)
                {
                    var LoginDetail = AuthenticUserDetails.GetCurrentUserDetails(identity);
                    var updatestatus = await _uow.ContractRepository.UpdateContractStatus(Value.Contractid, Value, LoginDetail,null);
                    if (updatestatus != null )
                    {
                        return Ok(updatestatus);
                    }
                }
                if (Value == null)
                {
                    return BadRequest("Invalid request data.");
                }
                // Post([FromForm] JsonWithFileDto DtoObject)
                var fileItem = DtoObject.Files;
                if (fileItem != null)
                {
                    foreach (var file in fileItem)
                    {
                        var extension = Path.GetExtension(file.FileName);
                        if (extension != null &&
                            !string.Equals(extension, ".doc", StringComparison.OrdinalIgnoreCase) &&
                            !string.Equals(extension, ".docx", StringComparison.OrdinalIgnoreCase) &&
                            !string.Equals(extension, ".pdf", StringComparison.OrdinalIgnoreCase) &&
                            !string.Equals(extension, ".xlsx", StringComparison.OrdinalIgnoreCase))
                        {
                            return BadRequest("Only document formats (.doc, .docx, .pdf, .xlsx) are accepted.");
                        }
                    }

                }
                IFormFile fileToUpload = null;
                if (fileItem != null && fileItem.Count > 0)
                {
                    fileToUpload = fileItem[0];
                }
                var LoginDetails = AuthenticUserDetails.GetCurrentUserDetails(identity);                
                var result = await _uow.ContractRepository.UpsertContracts(null,Value, fileToUpload,LoginDetails);
                return Ok(result);
               
            }
        }
        [HttpPut("{id:int}"), Authorize]
        public async Task<IActionResult> Put(int id ,[FromForm] JsonWithFileDto DtoObject)
        {
            JwtLoginDetailDto Userdetails = new JwtLoginDetailDto();
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (id <=0)
            {
                return BadRequest("Contract id can not be zero/less tahn zero");
            }
            if (identity == null)
                return BadRequest("Authentication Fails");
            else
            {
                var Value = JsonConvert.DeserializeObject<ContractRequestDto>(DtoObject.JasonData);
                if (Value.Statusid == 6)
                {
                    var LoginDetail = AuthenticUserDetails.GetCurrentUserDetails(identity);
                    var updatestatus = await _uow.ContractRepository.UpdateContractStatus(Value.Contractid, Value, LoginDetail, null);
                    if (updatestatus != null)
                    {
                        return Ok(updatestatus);
                    }
                }
                if (Value == null)
                {
                    return BadRequest("Invalid request data.");
                }
                if (id != Value.Contractid)
                {
                    return BadRequest(" both the IDs inside inside jsaon data and querystring are not same ");
                }
                // Post([FromForm] JsonWithFileDto DtoObject)
                var fileItem = DtoObject.Files;
                if (fileItem != null)
                {
                    foreach (var files in fileItem)
                    {
                        var extension = Path.GetExtension(files.FileName);
                        if (extension != ".doc" && extension != ".docx" && extension != ".pdf" && extension != ".exel" && extension != ".xlsx" && extension != ".xls")
                        {
                            return BadRequest("Only documents formats (.doc, .docx, .pdf,.exel) are accepted ");
                        }
                    }
                }
                IFormFile fileToUpload = null;
                if (fileItem != null && fileItem.Count > 0)
                {
                    fileToUpload = fileItem[0];
                }
                var LoginDetails = AuthenticUserDetails.GetCurrentUserDetails(identity);
                var result = await _uow.ContractRepository.UpsertContracts(id, Value, fileToUpload, LoginDetails);
                return Ok(result);

            }
        }
        [HttpDelete("{id}")]
        public async Task<bool> Delete(int id)
        {
            _uow.ProjectRepository.Delete(id);
            return await _uow.Complete();
        }
        [HttpGet("GetContractsByDeliveryAnchor/{DeliveryAnchorId:int}")]
        public async Task<IEnumerable<ContractRequestDto>> GetContractsByAnchorId(int DeliveryAnchorId)
        {
            return await _uow.ContractRepository.GetContractByAnchorId(DeliveryAnchorId);
        }

        [HttpPost("CheckContractNo")]
        public async Task<IActionResult> CheckContractNo([FromBody] ContractCheckDto DtoObject)
        {
            
            var resut = await _uow.ContractRepository.CheckContractNo(DtoObject.Contractid,DtoObject.Contractno);
            return Ok(resut);
        }
        [HttpPost("CheckPOno")]
        public async Task<IActionResult> CheckPOno([FromBody] PoCheckDto DtoObject)
        {

            var resut = await _uow.ContractRepository.CheckPoNo(DtoObject.Contractid, DtoObject.PoNo);
            return Ok(resut);
        }
       [HttpPost("ForeclosureBillingAction"),Authorize]
        public async Task<IActionResult> ForeclosureBillingAction(ForeclosureInputDTO value)
        {
            JwtLoginDetailDto Userdetails = new JwtLoginDetailDto();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            
            if (identity == null)
                return BadRequest("Authentication Fails");
            var LoginDetail = AuthenticUserDetails.GetCurrentUserDetails(identity);
            var result=await _uow.ContractRepository.ForeclosureBillingAction(value, LoginDetail);  
            return Ok(result);  
           
        }
        [HttpPost("CheckContractForeClosure")]
        public async Task<IActionResult> CheckContractForeClosure(ContractCheckDto value)
        {
            //JwtLoginDetailDto Userdetails = new JwtLoginDetailDto();
            //var identity = HttpContext.User.Identity as ClaimsIdentity;

            //if (identity == null)
            //    return BadRequest("Authentication Fails");
            //var LoginDetail = AuthenticUserDetails.GetCurrentUserDetails(identity);
            var result = await _uow.ContractRepository.CheckContractForeClosure(value);
            return Ok(result);  

        }
        [HttpGet("Contractendingsoon")]
        public async Task<IEnumerable<ContractendingsoonDto>> Contractendingsoon()
        {

            var result = await _uow.ContractRepository.getcontractsendingsoon();
            return result;
        }
        [HttpPost("isprojectestimationdone"), Authorize]
        public async Task<IActionResult> isprojectestimationdone([FromBody] ContractRequestDto Value)
        {

            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity == null)
                return BadRequest("Authentication Fails");

            var LoginDetails = AuthenticUserDetails.GetCurrentUserDetails(identity);

            var result = await _uow.ContractRepository.projectestimationstatusupdate(Value, LoginDetails);
            return Ok(result);

        }
    }
}
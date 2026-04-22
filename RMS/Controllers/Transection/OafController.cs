using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RMS.Entity.DTO;
using RMS.Extensions;
using RMS.Service.Interfaces;
using System.Data;
using System.Security.Claims;

namespace RMS.Controllers.Master
{
    public class OafController : BaseApiController
    {
        private readonly IUnitOfWork _uow;
        public OafController(IUnitOfWork uow)
        {
            _uow = uow;
        }
        [HttpGet,Authorize]
        public async Task<IActionResult> Get()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity == null)
            {
                return BadRequest("Authentication Fails");
            }
            var LoginDetails = AuthenticUserDetails.GetCurrentUserDetails(identity);
            var result= await _uow.IOafRepository.GetList(LoginDetails);
            return Ok(result);
           
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<OafDto>> Get(int id)
        {
            var oafDetails = await _uow.IOafRepository.GetOafById(id);

            if (oafDetails == null)
                return NotFound($"Oaf Does not exists at given id : {id}");

            return Ok(oafDetails);

        }
        [HttpPost, Authorize]
        public async Task<IActionResult> Post([FromForm] JsonWithFilesAofDto DtoObject) //[FromBody] OafDto obj)
        {
            //JsonWithFilesAofDto DtoObject=new JsonWithFilesAofDto();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity == null)
            {
                return BadRequest("Authentication Fails");
            }

            var Value = JsonConvert.DeserializeObject<OafDto>(DtoObject.JasonData);
            if (Value == null)
            {
                return BadRequest("Invalid request data.");
            }

            // Helper function 
            IFormFile ValidateAndGetAttachment(IFormFile attachment)
            {
                if (attachment == null) return null;
                var validExtensions = new[] { ".doc", ".docx", ".pdf", ".jpeg", ".png", ".jpg", ".xls", ".xlsx" };
                var extension = Path.GetExtension(attachment.FileName).ToLowerInvariant();
                if (!validExtensions.Contains(extension))
                {
                    throw new InvalidOperationException("Invalid file format");
                }
                return attachment;
            }

            // Validate attachments
            IFormFile emailAttachmentToUpload, proposalAttachmentToUpload, poAttachmentToUpload, costAttachmentToUpload;
            try
            {
                emailAttachmentToUpload = ValidateAndGetAttachment(DtoObject.Emailattachment);
                proposalAttachmentToUpload = ValidateAndGetAttachment(DtoObject.Proposalattachment);
                poAttachmentToUpload = ValidateAndGetAttachment(DtoObject.Poattachment);
                costAttachmentToUpload = ValidateAndGetAttachment(DtoObject.Costattachment);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }

            var LoginDetails = AuthenticUserDetails.GetCurrentUserDetails(identity);
            var result = await _uow.IOafRepository.UpsertOaf(null, Value, emailAttachmentToUpload, proposalAttachmentToUpload, poAttachmentToUpload, costAttachmentToUpload, LoginDetails);
            return Ok(result);
            

        }
        [HttpPut("{id:int}"), Authorize]
        public async Task<IActionResult> Put(int id, [FromForm] JsonWithFilesAofDto DtoObject)
        {
            JwtLoginDetailDto Userdetails = new JwtLoginDetailDto();
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (id <= 0)
            {
                return BadRequest("OAF id can not be zero/less than zero");
            }
            if (identity == null)
            {
                return BadRequest("Authentication Fails");
            }

            var Value = JsonConvert.DeserializeObject<OafDto>(DtoObject.JasonData);
            if (Value == null)
            {
                return BadRequest("Invalid request data.");
            }
            if (id != Value.Oafid)
            {
                return BadRequest(" Both the IDs inside json data and querystring are not same ");
            }
            // Helper function 
            IFormFile ValidateAndGetAttachment(IFormFile attachment)
            {
                if (attachment == null) return null;
                var validExtensions = new[] { ".doc", ".docx", ".pdf", ".jpeg", ".png", ".jpg", ".xls", ".xlsx" };
                var extension = Path.GetExtension(attachment.FileName).ToLowerInvariant();
                if (!validExtensions.Contains(extension))
                {
                    throw new InvalidOperationException("Invalid file format");
                }

                return attachment;
            }

            // Validate attachments
            IFormFile emailAttachmentToUpload, proposalAttachmentToUpload, poAttachmentToUpload, costAttachmentToUpload;
            try
            {
                emailAttachmentToUpload = ValidateAndGetAttachment(DtoObject.Emailattachment);
                proposalAttachmentToUpload = ValidateAndGetAttachment(DtoObject.Proposalattachment);
                poAttachmentToUpload = ValidateAndGetAttachment(DtoObject.Poattachment);
                costAttachmentToUpload = ValidateAndGetAttachment(DtoObject.Costattachment);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }            

            var LoginDetails = AuthenticUserDetails.GetCurrentUserDetails(identity);
            var result = await _uow.IOafRepository.UpsertOaf(id, Value, emailAttachmentToUpload, proposalAttachmentToUpload, poAttachmentToUpload, costAttachmentToUpload, LoginDetails);
            return Ok(result);

        }
        [HttpDelete("{id}")]
        public async Task<bool> Delete(int id)
        {
            _uow.IOafRepository.Delete(id);
            return await _uow.Complete();
        }
        [HttpPost("DHAction"),Authorize]       
        public async Task<Response> ApproveRejectOaf([FromBody] OafDto Value)
        {
            if (Value == null)
            {
                throw new ArgumentNullException("Value cannot be null.");
            }

            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity == null)
            {
                throw new Exception("Authentication failed.");
            }

            var LoginDetails = AuthenticUserDetails.GetCurrentUserDetails(identity);
            return await _uow.IOafRepository.ApproverAction(Value, LoginDetails);
        }

        [HttpPost("OAFwiseResourceAllotment"),Authorize]
        public async Task<Response> OAFwiseResourceAllotment([FromBody] OAFwiseResourceAllotmentDto Value)
        {
            JwtLoginDetailDto Userdetails = new JwtLoginDetailDto();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var LoginDetails = AuthenticUserDetails.GetCurrentUserDetails(identity);
            if (Value == null)
            {
                throw new ArgumentNullException();   
            }
            var result = await _uow.IOafRepository.OAFwiseResourceAllotment(Value, LoginDetails);
            return result;  


        }
        [HttpGet("GetOafListByDAId/{DeliveryAnchorId:int}")]
        public async Task<IActionResult> GetOafListByDAId(int DeliveryAnchorId)
        {
            var result = await _uow.IOafRepository.GetOafListByDAId(DeliveryAnchorId);
            return Ok(result);

        }
        [HttpPost("ExtendOaf"),Authorize]
        public async Task<IActionResult>ExtendOaf(ExtendOafDto value)
        {
            var Identity = HttpContext.User.Identity as ClaimsIdentity;
            var LoginDetails =AuthenticUserDetails.GetCurrentUserDetails(Identity);
            if (value == null)
            {
                throw new ArgumentNullException("Input object can not be null");
            }
            var result = await _uow.IOafRepository.ExtendOaf(value, LoginDetails);
            return Ok(result);      

        }

    }
}
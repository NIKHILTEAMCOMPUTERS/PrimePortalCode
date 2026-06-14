using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.Entity.DTO;
using RMS.Extensions;
using RMS.Service.Interfaces;
using RMS.Utility;
using System.Security.Claims;

namespace RMS.Controllers.Transection
{
    public class TeamPendingProvisionController : BaseApiController
    {
        private readonly IUnitOfWork _uow;

        public TeamPendingProvisionController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpGet("list"), Authorize]
        public async Task<IActionResult> GetList()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity == null) return BadRequest("Authentication Fails");

            var loginDetails = AuthenticUserDetails.GetCurrentUserDetails(identity);
            var result = await _uow.TeamPendingProvisionRepository.GetPendingList(loginDetails);
            return Ok(result);
        }

        [HttpPost("update-closer-date"), Authorize]
        public async Task<IActionResult> UpdateCloserDate([FromBody] TeamUpdateCloserDateDto dto)
        {
            if (dto == null || dto.ProvisionId <= 0)
                return BadRequest("Invalid request.");

            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity == null) return BadRequest("Authentication Fails");

            var loginDetails = AuthenticUserDetails.GetCurrentUserDetails(identity);
            var result = await _uow.TeamPendingProvisionRepository.UpdateCloserDate(dto, loginDetails);
            return Ok(result);
        }

        [HttpPost("update-document-no"), Authorize]
        public async Task<IActionResult> UpdateDocumentNo([FromBody] TeamUpdateDocumentNoDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.MonthYear) || string.IsNullOrWhiteSpace(dto.PoNumber))
                return BadRequest("Invalid request.");

            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity == null) return BadRequest("Authentication Fails");

            var loginDetails = AuthenticUserDetails.GetCurrentUserDetails(identity);
            var result = await _uow.TeamPendingProvisionRepository.UpdateDocumentNo(dto, loginDetails);
            return Ok(result);
        }

        [HttpPost("record-billed"), Authorize]
        public async Task<IActionResult> RecordBilled([FromBody] TeamRecordBilledDto dto)
        {
            if (dto == null || dto.ProvisionId <= 0)
                return BadRequest("Invalid request.");

            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity == null) return BadRequest("Authentication Fails");

            var loginDetails = AuthenticUserDetails.GetCurrentUserDetails(identity);
            var result = await _uow.TeamPendingProvisionRepository.RecordBilled(dto, loginDetails);
            return Ok(result);
        }

        [HttpGet("history/{provisionId:int}"), Authorize]
        public async Task<IActionResult> GetHistory(int provisionId)
        {
            if (provisionId <= 0) return BadRequest("Invalid provision ID.");
            var result = await _uow.TeamPendingProvisionRepository.GetHistory(provisionId);
            return Ok(result);
        }
    }
}

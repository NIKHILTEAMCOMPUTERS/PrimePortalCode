using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.Entity.DTO;
using RMS.Extensions;
using RMS.Service.Interfaces;
using RMS.Utility;
using System.Security.Claims;

namespace RMS.Controllers.Transection
{
    public class ProvisionManagementController : BaseApiController
    {
        private readonly IUnitOfWork _uow;
        private readonly IConfiguration _config;

        public ProvisionManagementController(IUnitOfWork uow, IConfiguration config)
        {
            _uow = uow;
            _config = config;
        }

        [HttpGet("list"), Authorize]
        public async Task<IActionResult> GetList([FromQuery] string monthYear)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity == null) return BadRequest("Authentication Fails");

            var loginDetails = AuthenticUserDetails.GetCurrentUserDetails(identity);
            var result = await _uow.ProvisionManagementRepository.GetProvisionList(monthYear, loginDetails);
            return Ok(result);
        }

        [HttpPost("carryforward"), Authorize]
        public async Task<IActionResult> CarryForward([FromBody] ProvisionActionDto dto)
        {
            if (dto == null || dto.ProvisionId <= 0)
                return BadRequest("Invalid request.");

            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity == null) return BadRequest("Authentication Fails");

            var loginDetails = AuthenticUserDetails.GetCurrentUserDetails(identity);
            var result = await _uow.ProvisionManagementRepository.CarryForward(dto, loginDetails);
            return Ok(result);
        }

        [HttpPost("reverse"), Authorize]
        public async Task<IActionResult> Reverse([FromBody] ProvisionActionDto dto)
        {
            if (dto == null || dto.ProvisionId <= 0)
                return BadRequest("Invalid request.");

            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity == null) return BadRequest("Authentication Fails");

            var loginDetails = AuthenticUserDetails.GetCurrentUserDetails(identity);
            var result = await _uow.ProvisionManagementRepository.ReverseProvision(dto, loginDetails);

            if (result.responseCode == 200 && result.data != null)
                SendReversalEmail(result.data);

            return Ok(result);
        }

        [HttpPost("update-paid-amount"), Authorize]
        public async Task<IActionResult> UpdatePaidAmount([FromBody] UpdatePaidAmountDto dto)
        {
            if (dto == null || dto.ProvisionId <= 0)
                return BadRequest("Invalid request.");

            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity == null) return BadRequest("Authentication Fails");

            var loginDetails = AuthenticUserDetails.GetCurrentUserDetails(identity);
            var result = await _uow.ProvisionManagementRepository.UpdatePaidAmount(dto, loginDetails);
            return Ok(result);
        }

        [HttpPost("update-billing-date"), Authorize]
        public async Task<IActionResult> UpdateBillingDate([FromBody] UpdateEstimatedBillingDateDto dto)
        {
            if (dto == null || dto.ProvisionId <= 0)
                return BadRequest("Invalid request.");

            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity == null) return BadRequest("Authentication Fails");

            var loginDetails = AuthenticUserDetails.GetCurrentUserDetails(identity);
            var result = await _uow.ProvisionManagementRepository.UpdateEstimatedBillingDate(dto, loginDetails);
            return Ok(result);
        }

        [HttpGet("history/{provisionId:int}"), Authorize]
        public async Task<IActionResult> GetHistory(int provisionId)
        {
            if (provisionId <= 0) return BadRequest("Invalid provision ID.");
            var result = await _uow.ProvisionManagementRepository.GetProvisionHistory(provisionId);
            return Ok(result);
        }

        [HttpPost("update-document-no"), Authorize]
        public async Task<IActionResult> UpdateDocumentNo([FromBody] UpdateDocumentNoDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.MonthYear) || string.IsNullOrWhiteSpace(dto.PoNumber))
                return BadRequest("Invalid request.");

            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity == null) return BadRequest("Authentication Fails");

            var loginDetails = AuthenticUserDetails.GetCurrentUserDetails(identity);
            var result = await _uow.ProvisionManagementRepository.UpdateDocumentNo(dto, loginDetails);
            return Ok(result);
        }

        // Manual trigger for testing (admin only)
        [HttpPost("trigger-monthly-job"), Authorize]
        public async Task<IActionResult> TriggerMonthlyJob()
        {
            var autoReversed = await _uow.ProvisionManagementRepository.ProcessMonthlyCarryForward();
            foreach (var item in autoReversed)
                SendReversalEmail(new { provisionId = item.ProvisionId, billingMonthYear = item.BillingMonthYear, provisionAmount = item.ProvisionAmount, paidAmount = item.PaidAmount, carryForwardCount = item.CarryForwardCount });

            return Ok(new Response
            {
                responseCode = 200,
                responseMessage = $"Monthly carry-forward job executed. {autoReversed.Count} provision(s) auto-reversed.",
                data = autoReversed
            });
        }

        private void SendReversalEmail(object provisionData)
        {
            Task.Run(() =>
            {
                try
                {
                    string sender = _config["MailCredential:Email"] ?? "";
                    string password = _config["MailCredential:Password"] ?? "";
                    int port = int.TryParse(_config["MailCredential:Port"], out var p) ? p : 587;
                    string host = _config["MailCredential:Host"] ?? "";
                    bool ssl = bool.TryParse(_config["MailCredential:EnableSsl"], out var s) && s;

                    var d = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, System.Text.Json.JsonElement>>(
                        System.Text.Json.JsonSerializer.Serialize(provisionData));

                    string provisionId = d.ContainsKey("provisionId") ? d["provisionId"].ToString() : "-";
                    string monthYear = d.ContainsKey("billingMonthYear") ? d["billingMonthYear"].GetString() : "-";
                    string amount = d.ContainsKey("provisionAmount") ? d["provisionAmount"].ToString() : "0";
                    string paid = d.ContainsKey("paidAmount") ? d["paidAmount"].ToString() : "0";
                    string cfCount = d.ContainsKey("carryForwardCount") ? d["carryForwardCount"].ToString() : "0";

                    var mail = new Mail(sender, password, port, host, ssl);
                    string subject = $"Provision Manually Reversed – {monthYear} (ID: {provisionId})";
                    string body = $@"
<html><body style='font-family:Arial,sans-serif;font-size:14px;'>
<p>Dear Team,</p>
<p>The following provision has been <strong>manually reversed</strong> via the Prime Portal:</p>
<table border='1' cellpadding='8' cellspacing='0' style='border-collapse:collapse;'>
  <tr style='background:#f2f2f2'><th>Field</th><th>Value</th></tr>
  <tr><td>Provision ID</td><td>{provisionId}</td></tr>
  <tr><td>Billing Month</td><td>{monthYear}</td></tr>
  <tr><td>Provision Amount</td><td>₹{amount}</td></tr>
  <tr><td>Paid Amount</td><td>₹{paid}</td></tr>
  <tr><td>Carry Forward Count</td><td>{cfCount}</td></tr>
  <tr><td>Reversed At</td><td>{DateTime.Now:dd/MM/yyyy HH:mm}</td></tr>
</table>
<p>This is an automated notification from the Prime Portal.</p>
</body></html>";

                    string outMsg = "";
                    mail.SendMail(new List<string> { "nikhil.vig@teamcomputers.com" }, subject, body, out outMsg);
                }
                catch { /* Silent */ }
            });
        }
    }
}

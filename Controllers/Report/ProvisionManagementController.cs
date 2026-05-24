using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using RMS.Client.Models.Master;
using RMS.Client.Utility;
using System.Globalization;
using System.Net;
using System.Text;

namespace RMS.Client.Controllers.Reporting
{
    public class ProvisionManagementController : BaseController
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        private ApiManager _apiManager;
        private readonly IsoDateTimeConverter _dateConverter;

        public ProvisionManagementController(IConfiguration configuration, IWebHostEnvironment env)
            : base(configuration, env)
        {
            _configuration = configuration;
            _env = env;
            _dateConverter = new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" };
        }

        public async Task<IActionResult> Index(string date = null)
        {
            if (!ReadPermission)
                return RedirectToAction("NotAuthorized", "Home");

            ViewData["controller"] = ControllerContext.RouteData.Values["controller"].ToString();
            ViewData["action"] = ControllerContext.RouteData.Values["action"].ToString();

            string monthYear = string.IsNullOrEmpty(date)
                ? DateTime.Now.ToString("MMM-yy", CultureInfo.InvariantCulture)
                : DateTime.Parse(date).ToString("MMM-yy", CultureInfo.InvariantCulture);

            ViewBag.SelectedMonth = DateTime.ParseExact(monthYear, "MMM-yy", CultureInfo.InvariantCulture)
                                             .ToString("yyyy-MM");

            var items = await FetchProvisionList(monthYear);
            return View("~/Views/Report/ProvisionManagement.cshtml", items);
        }

        [HttpGet]
        public async Task<IActionResult> GetHistory(int provisionId)
        {
            if (Session == null) return Json(new { success = false, message = "Session expired." });

            string url = $"{_configuration["ServiceUrl"].Trim()}/api/ProvisionManagement/history/{provisionId}";
            _apiManager = new ApiManager(url, Session.Token);
            var (code, content) = await _apiManager.Get();

            if (code == HttpStatusCode.OK)
            {
                var history = JsonConvert.DeserializeObject<List<ProvisionHistoryItem>>(content, _dateConverter);
                return Json(new { success = true, data = history });
            }
            return Json(new { success = false, message = "Failed to load history." });
        }

        [HttpPost]
        public async Task<IActionResult> CarryForward(
            [FromForm] int provisionId,
            [FromForm] string? remark,
            [FromForm] string? targetMonth,
            IFormFile? attachment)
        {
            if (!WritePermission)
                return Json(new { success = false, message = "You do not have permission to carry forward provisions." });

            if (provisionId <= 0)
                return Json(new { success = false, message = "Invalid request." });

            string? attachmentPath = null;
            if (attachment != null && attachment.Length > 0)
            {
                var allowedExtensions = new[] { ".pdf", ".doc", ".docx", ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
                var ext = Path.GetExtension(attachment.FileName).ToLowerInvariant();
                if (!allowedExtensions.Contains(ext))
                    return Json(new { success = false, message = "Invalid file type. Allowed: PDF, Word, Images." });

                if (attachment.Length > 10 * 1024 * 1024)
                    return Json(new { success = false, message = "File size exceeds 10 MB limit." });

                var uploadsDir = Path.Combine(_env.WebRootPath, "uploads", "provisions");
                Directory.CreateDirectory(uploadsDir);
                var safeFileName = $"cf_{provisionId}_{DateTime.Now:yyyyMMddHHmmss}_{Path.GetFileName(attachment.FileName)}";
                var filePath = Path.Combine(uploadsDir, safeFileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                    await attachment.CopyToAsync(stream);
                attachmentPath = $"/uploads/provisions/{safeFileName}";
            }

            var request = new ProvisionActionRequest
            {
                ProvisionId = provisionId,
                Remark = remark,
                TargetMonth = targetMonth,
                AttachmentPath = attachmentPath
            };

            string url = $"{_configuration["ServiceUrl"].Trim()}/api/ProvisionManagement/carryforward";
            _apiManager = new ApiManager(url, Session.Token);
            var (code, content) = await _apiManager.PostJson(JsonConvert.SerializeObject(request));

            var response = ParseResponse(code, content);
            return Json(response);
        }

        public async Task<IActionResult> MonthWiseProvisionReport(string date = null)
        {
            if (!ReadPermission)
                return RedirectToAction("NotAuthorized", "Home");

            ViewData["controller"] = ControllerContext.RouteData.Values["controller"].ToString();
            ViewData["action"] = ControllerContext.RouteData.Values["action"].ToString();

            string monthYear = string.IsNullOrEmpty(date)
                ? DateTime.Now.ToString("MMM-yy", CultureInfo.InvariantCulture)
                : DateTime.Parse(date).ToString("MMM-yy", CultureInfo.InvariantCulture);

            ViewBag.SelectedMonth = DateTime.ParseExact(monthYear, "MMM-yy", CultureInfo.InvariantCulture).ToString("yyyy-MM");
            ViewBag.MonthYear = monthYear;

            var items = await FetchProvisionList(monthYear);
            return View("~/Views/Report/ProvisionMonthWiseReport.cshtml", items);
        }

        public async Task<IActionResult> DAWiseProvisionReport(string date = null)
        {
            if (!ReadPermission)
                return RedirectToAction("NotAuthorized", "Home");

            ViewData["controller"] = ControllerContext.RouteData.Values["controller"].ToString();
            ViewData["action"] = ControllerContext.RouteData.Values["action"].ToString();

            string monthYear = string.IsNullOrEmpty(date)
                ? DateTime.Now.ToString("MMM-yy", CultureInfo.InvariantCulture)
                : DateTime.Parse(date).ToString("MMM-yy", CultureInfo.InvariantCulture);

            ViewBag.SelectedMonth = DateTime.ParseExact(monthYear, "MMM-yy", CultureInfo.InvariantCulture).ToString("yyyy-MM");
            ViewBag.MonthYear = monthYear;

            var items = await FetchProvisionList(monthYear);
            return View("~/Views/Report/ProvisionDAWiseReport.cshtml", items);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateBillingDate([FromBody] ProvisionEditRequest request)
        {
            if (!WritePermission)
                return Json(new { success = false, message = "You do not have permission to edit provisions." });

            if (request == null || request.ProvisionId <= 0)
                return Json(new { success = false, message = "Invalid request." });

            string url = $"{_configuration["ServiceUrl"].Trim()}/api/ProvisionManagement/update-billing-date";
            _apiManager = new ApiManager(url, Session.Token);
            var (code, content) = await _apiManager.PostJson(JsonConvert.SerializeObject(request));

            var response = ParseResponse(code, content);
            return Json(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePaidAmount([FromBody] UpdatePaidAmountRequest request)
        {
            if (!WritePermission)
                return Json(new { success = false, message = "You do not have permission to update paid amounts." });

            if (request == null || request.ProvisionId <= 0)
                return Json(new { success = false, message = "Invalid request." });

            string url = $"{_configuration["ServiceUrl"].Trim()}/api/ProvisionManagement/update-paid-amount";
            _apiManager = new ApiManager(url, Session.Token);
            var (code, content) = await _apiManager.PostJson(JsonConvert.SerializeObject(request));

            var response = ParseResponse(code, content);
            return Json(response);
        }

        [HttpPost]
        public async Task<IActionResult> Reverse([FromBody] ProvisionActionRequest request)
        {
            if (!WritePermission)
                return Json(new { success = false, message = "You do not have permission to reverse provisions." });

            if (request == null || request.ProvisionId <= 0)
                return Json(new { success = false, message = "Invalid request." });

            string url = $"{_configuration["ServiceUrl"].Trim()}/api/ProvisionManagement/reverse";
            _apiManager = new ApiManager(url, Session.Token);
            var (code, content) = await _apiManager.PostJson(JsonConvert.SerializeObject(request));

            var response = ParseResponse(code, content);
            return Json(response);
        }

        private async Task<List<ProvisionManagementItem>> FetchProvisionList(string monthYear)
        {
            string url = $"{_configuration["ServiceUrl"].Trim()}/api/ProvisionManagement/list?monthYear={Uri.EscapeDataString(monthYear)}";
            _apiManager = new ApiManager(url, Session?.Token ?? "");
            var (code, content) = await _apiManager.Get();

            if (code == HttpStatusCode.OK && !string.IsNullOrEmpty(content))
                return JsonConvert.DeserializeObject<List<ProvisionManagementItem>>(content, _dateConverter)
                       ?? new List<ProvisionManagementItem>();

            return new List<ProvisionManagementItem>();
        }

        private static object ParseResponse(HttpStatusCode code, string content)
        {
            if (code == HttpStatusCode.OK)
            {
                try
                {
                    var obj = JsonConvert.DeserializeObject<dynamic>(content);
                    bool isSuccess = (int)obj.responseCode == 200;
                    return new { success = isSuccess, message = (string)obj.responseMessage, data = obj.data };
                }
                catch
                {
                    return new { success = true, message = "Action completed." };
                }
            }
            return new { success = false, message = $"Server error ({(int)code})." };
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using RMS.Client.Models.Master;
using RMS.Client.Utility;
using System.Net;

namespace RMS.Client.Controllers.Reporting
{
    public class TeamPendingProvisionController : BaseController
    {
        private readonly IConfiguration _configuration;
        private ApiManager _apiManager;
        private readonly IsoDateTimeConverter _dateConverter;

        public TeamPendingProvisionController(IConfiguration configuration, IWebHostEnvironment env)
            : base(configuration, env)
        {
            _configuration = configuration;
            _dateConverter = new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" };
        }

        public async Task<IActionResult> Index()
        {
            if (!ReadPermission)
                return RedirectToAction("NotAuthorized", "Home");

            ViewData["controller"] = ControllerContext.RouteData.Values["controller"].ToString();
            ViewData["action"] = ControllerContext.RouteData.Values["action"].ToString();

            var items = await FetchList();
            return View("~/Views/Report/TeamPendingProvision.cshtml", items);
        }

        [HttpGet]
        public async Task<IActionResult> GetHistory(int provisionId)
        {
            if (Session == null) return Json(new { success = false, message = "Session expired." });

            string url = $"{_configuration["ServiceUrl"].Trim()}/api/TeamPendingProvision/history/{provisionId}";
            _apiManager = new ApiManager(url, Session.Token);
            var (code, content) = await _apiManager.Get();

            if (code == HttpStatusCode.OK)
            {
                var history = JsonConvert.DeserializeObject<List<TeamProvisionHistoryItem>>(content, _dateConverter);
                return Json(new { success = true, data = history });
            }
            return Json(new { success = false, message = "Failed to load history." });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCloserDate([FromBody] TeamUpdateCloserDateRequest request)
        {
            if (!WritePermission)
                return Json(new { success = false, message = "You do not have permission to update closer dates." });
            if (request == null || request.ProvisionId <= 0)
                return Json(new { success = false, message = "Invalid request." });

            string url = $"{_configuration["ServiceUrl"].Trim()}/api/TeamPendingProvision/update-closer-date";
            _apiManager = new ApiManager(url, Session.Token);
            var (code, content) = await _apiManager.PostJson(JsonConvert.SerializeObject(request));
            return Json(ParseResponse(code, content));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateDocumentNo([FromBody] TeamUpdateDocumentNoRequest request)
        {
            if (!WritePermission)
                return Json(new { success = false, message = "You do not have permission to update document numbers." });
            if (request == null || string.IsNullOrWhiteSpace(request.MonthYear) || string.IsNullOrWhiteSpace(request.PoNumber))
                return Json(new { success = false, message = "Month/Year and PO Number are required." });

            string url = $"{_configuration["ServiceUrl"].Trim()}/api/TeamPendingProvision/update-document-no";
            _apiManager = new ApiManager(url, Session.Token);
            var (code, content) = await _apiManager.PostJson(JsonConvert.SerializeObject(request));
            return Json(ParseResponse(code, content));
        }

        [HttpPost]
        public async Task<IActionResult> RecordBilled([FromBody] TeamRecordBilledRequest request)
        {
            if (!WritePermission)
                return Json(new { success = false, message = "You do not have permission to record billing." });
            if (request == null || request.ProvisionId <= 0)
                return Json(new { success = false, message = "Invalid request." });

            string url = $"{_configuration["ServiceUrl"].Trim()}/api/TeamPendingProvision/record-billed";
            _apiManager = new ApiManager(url, Session.Token);
            var (code, content) = await _apiManager.PostJson(JsonConvert.SerializeObject(request));
            return Json(ParseResponse(code, content));
        }

        private async Task<List<TeamPendingProvisionItem>> FetchList()
        {
            string url = $"{_configuration["ServiceUrl"].Trim()}/api/TeamPendingProvision/list";
            _apiManager = new ApiManager(url, Session?.Token ?? "");
            var (code, content) = await _apiManager.Get();

            if (code == HttpStatusCode.OK && !string.IsNullOrEmpty(content))
                return JsonConvert.DeserializeObject<List<TeamPendingProvisionItem>>(content, _dateConverter)
                       ?? new List<TeamPendingProvisionItem>();

            return new List<TeamPendingProvisionItem>();
        }

        private static object ParseResponse(HttpStatusCode code, string content)
        {
            if (code == HttpStatusCode.OK)
            {
                try
                {
                    var obj = JsonConvert.DeserializeObject<dynamic>(content);
                    bool isSuccess = (int)obj.responseCode == 200;
                    return new { success = isSuccess, message = (string)obj.responseMessage };
                }
                catch { return new { success = true, message = "Action completed." }; }
            }
            return new { success = false, message = $"Server error ({(int)code})." };
        }
    }
}

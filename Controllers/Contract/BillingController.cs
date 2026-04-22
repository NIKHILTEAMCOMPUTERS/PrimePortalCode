using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using RMS.Client.Models.Contract;
using RMS.Client.Models.Customer;
using RMS.Client.Models.Dashboard;
using RMS.Client.Models.Vendor;
using RMS.Client.Utility;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Net;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Security.Policy;
using System.Text;
using static RMS.Client.Controllers.Contract.BillingController;

namespace RMS.Client.Controllers.Contract
{
    public class BillingController : BaseController
    {
        private readonly IWebHostEnvironment _env;
        private ApiManager apiManager;
        private IConfiguration _configuration;
        private IsoDateTimeConverter dateTimeConverter;
        public BillingController(IConfiguration configuration, IWebHostEnvironment env, IWebHostEnvironment environment)
              : base(configuration, env)
        {
            _env = environment ?? throw new ArgumentNullException(nameof(environment));
            _configuration = configuration;
            dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" };
        }
        public async Task<IActionResult> Index(string m = null, string fd = null, string td = null)
        {
            if (!ReadPermission)
                return RedirectToAction("NotAuthorized", "Home");

            List<Billing> billingList = new List<Billing>();

            DateTime currentDate = DateTime.Now;

            // Check if fd and td are provided, and parse them if necessary
            DateTime financialYearStart = string.IsNullOrEmpty(fd)
                ? currentDate.AddMonths(-1) // Set to the previous month if not provided
                : DateTime.Parse(fd);

            DateTime financialYearEnd = string.IsNullOrEmpty(td)
                ? currentDate.AddMonths(5)  // Set to 6 months ahead if not provided
                : DateTime.Parse(td);

            var objPram = new
            {
                mode = string.IsNullOrEmpty(m) ? "employeewise" : m.ToLower().Trim(),
                fromDate = financialYearStart.ToString("yyyy-MM-dd"),
                toDate = financialYearEnd.ToString("yyyy-MM-dd")
            };
            var httpContent = new StringContent(JsonConvert.SerializeObject(objPram), Encoding.UTF8, "application/json");

            apiManager = new ApiManager(_configuration["ServiceUrl"].ToString().Trim() + "/api/contractbilling/getbillings/");
            
            try
            {
                var (statusCode, responseContent) = await apiManager.Get(httpContent);
                if (statusCode == System.Net.HttpStatusCode.OK)
                {
                    billingList = JsonConvert.DeserializeObject<List<Billing>>(responseContent, dateTimeConverter);
                    if (billingList != null)
                    {
                        foreach (var item in billingList)
                        {
                            if (!string.IsNullOrEmpty(item.monthYear))
                            {
                                item.monthYear = (item.monthYear);
                            }
                        }
                    }
                }
                else
                {
                    TempData["error"] = $"Unable to fetch billing data. Status: {statusCode}. Message: {responseContent}";
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = $"An error occurred while fetching billing data: {ex.Message}";
            }

            ViewBag.mode = objPram.mode;
            ViewBag.fDate = financialYearStart.ToString("yyyy-MM");
            ViewBag.tDate = financialYearEnd.ToString("yyyy-MM");
            ViewData["controller"] = this.ControllerContext.RouteData.Values["controller"].ToString();
            var jasonyfy = JsonConvert.SerializeObject(billingList);
            return View(billingList);
        }

        private string ConvertToValidDate(string billingMonthYear)
        {
            // Replace "Sept" with "Sep" if present
            return billingMonthYear?.Replace("Sept", "Sep");
        }
        // all billing actions starts here

        #region Provisions actions
        [HttpPost]
        public async Task<IActionResult> BillingApproval([FromBody] List<ApprovalRequestItem> selectedItems)

        {
            try
            {



                if (!WritePermission)
                {
                    return RedirectToAction("NotAuthorized", "Home");
                }

                if (selectedItems.Count <= 0)
                {
                    TempData["error"] = "No items selected for approval.";
                    return RedirectToAction("ContractBillingProvisionReport", "Billing");
                }

                List<string> convertedDates = new List<string>();


                foreach (var item in selectedItems)
                {
                    if (!string.IsNullOrEmpty(item.exptectedbillingdate))
                    {
                        DateTime parsedDate = DateTime.ParseExact(item.exptectedbillingdate, "dd/mm/yyyy", CultureInfo.InvariantCulture);


                        string formattedDate = parsedDate.ToString("yyyy-MM-dd");


                        item.exptectedbillingdate = formattedDate;


                        convertedDates.Add(formattedDate);
                    }
                }
                string endpointUrl = _configuration["ServiceUrl"].ToString().Trim() + "/api/ContractBilling/SendForApproval";
                apiManager = new ApiManager(endpointUrl, Session.Token);

                string serializedData = JsonConvert.SerializeObject(selectedItems);

                var (statusCode, responseContent) = await apiManager.PostJson(serializedData);

                if (statusCode == HttpStatusCode.OK)
                {

                    dynamic apiResponse = JsonConvert.DeserializeObject<dynamic>(responseContent);
                    string practiceheademail = apiResponse.data.practiceheademail;
                    string daemail = apiResponse.data.daemail;
                    string daname = apiResponse.data.daname;
                    string practiceheadname = apiResponse.data.practiceheadname;
                    string projectname = apiResponse.data.projectname;



                    string mailTemplatePath = Path.Combine(_env.WebRootPath, "Resource", "Mailer", "genericmailer.html");
                    string mailBody = System.IO.File.Exists(mailTemplatePath) ? System.IO.File.ReadAllText(mailTemplatePath) : string.Empty;

                    string sMailBody = string.Format(
                                        mailBody,
                                        DateTime.Now.ToString("yyyy"),
                                        "Billing Request Received For Project" + projectname,
                                        "Request Received For Approval",
                                        practiceheadname,
                                        "Provision Billing Request Received from " + daname + " for Project: " + projectname
                                    );



                  //  SendMail(practiceheademail, $"Provision Request Received From {daname}", sMailBody, new List<string> { daemail }, out string sMailOutput, null);

                    ViewData["success"] = "Send For Approval Succeeded.";
                    return Json(new { success = true, msg = "Submitted" });
                }

                else
                {
                    ViewData["error"] = "Send For Approval Failed. Please try again.";
                    return Json(new { success = false, msg = "Failed" });
                }
            }
            catch (Exception ex)
            {
                ViewData["error"] = $"An error occurred: {ex.Message}";
                return Json(new { success = false, msg = $"An error occurred: {ex.Message}" });
            }
        }

        public async Task<IActionResult> DeleteProBilling(int id)
        {
            if (id <= 0)
            {
                TempData["error"] = "Invalid Provision billing ID";
                return RedirectToAction("ContractBillingProvisionReport", "Billing");
            }

            try
            {
                apiManager = new ApiManager(_configuration["ServiceUrl"].ToString().Trim() + $"/api/ContractBilling/{id}", Session.Token); //--TOKEN


                var (statusCode, responseCode) = await apiManager.Delete();
                if (statusCode == System.Net.HttpStatusCode.OK)
                {
                    TempData["success"] = "Provision Billling deleted successfully!";
                    return RedirectToAction("ContractBillingProvisionReport", "Report");
                }
                else
                {
                    ViewData["error"] = "Failed to delete Provision Billing";
                    return RedirectToAction("ContractBillingProvisionReport", "Report");
                }


            }
            catch (Exception ex)
            {
                ViewData["error"] = $"An error has occurred: {ex.Message}";
            }

            return RedirectToAction("ContractBillingProvisionReport", "Report");
        }
        [HttpPost]
        public async Task<IActionResult> BillingApprovalDH(string selectedItems)

        {
            try
            {

                if (selectedItems == null)
                {
                    TempData["error"] = "No items selected for approval.";
                    return RedirectToAction("ContractBillingProvisionReportDH", "Report");
                }

                string endpointUrl = _configuration["ServiceUrl"].ToString().Trim() + "/api/ContractBilling/ApproverAction";
                apiManager = new ApiManager(endpointUrl, Session.Token);


                var (statusCode, responseContent) = await apiManager.PostJson(selectedItems);

                if (statusCode == HttpStatusCode.OK)
                {
                    dynamic apiResponse = JsonConvert.DeserializeObject<dynamic>(responseContent);
                    string practiceheademail = apiResponse.data.practiceheademail;
                    string daemail = apiResponse.data.daemail;
                    string daname = apiResponse.data.daname;
                    string practiceheadname = apiResponse.data.practiceheadname;
                    string projectname = apiResponse.data.projectname;
                    string flag = apiResponse.data.flag;

                    string mailTemplatePath = Path.Combine(_env.WebRootPath, "Resource", "Mailer", "genericmailer.html");
                    string mailBody = System.IO.File.Exists(mailTemplatePath) ? System.IO.File.ReadAllText(mailTemplatePath) : string.Empty;

                    string subject = flag == "Approved" ? "Provision Billing Request Approved For Project" : "Provision Billing Request Rejected For Project";
                    string mailMessage = flag == "Approved"
                        ? $"Provision Billing Request Approved by {practiceheadname} for Project: {projectname}"
                        : $"Provision Billing Request Rejected by {practiceheadname} for Project: {projectname}";

                    string sMailBody = string.Format(
                        mailBody,
                        DateTime.Now.ToString("yyyy"),
                        subject + projectname,
                        flag == "Approved" ? "Request Approved" : "Request Rejected",
                    daname,
                    mailMessage
                    );
                   

                    SendMail(daemail, $" The provision billing request for the {projectname} project has been {flag} by {practiceheadname}", sMailBody, new List<string> { practiceheademail }, out string sMailOutput, null);

                    return Json(new { success = true, msg = "Acted to request successfully!!" });
                }
                else
                {
                    ViewData["error"] = "Something went wrong. Please try again.";
                    return Json(new { success = false, msg = "Failed" });
                }
            }
            catch (Exception ex)
            {
                ViewData["error"] = $"An error occurred: {ex.Message}";
                return Json(new { success = false, msg = $"An error occurred: {ex.Message}" });
            }
        }
        #endregion


        #region actual billing actions
        [HttpPost]
        public async Task<IActionResult> RevisionApproval([FromBody] List<ApprovalRequestItem> selectedItems) // this method for actual billing revisions

        {
            try
            {
                bool isactualrevised = selectedItems.Select(a => a.IsRevised == true).Any();
                List<string> convertedDates = new List<string>();
                if (!WritePermission)
                {
                    return RedirectToAction("NotAuthorized", "Home");
                }

                if (isactualrevised)
                {
                    foreach (var item in selectedItems)
                    {
                        if (!string.IsNullOrEmpty(item.exptectedbillingdate))
                        {
                            DateTime parsedDate = DateTime.ParseExact(item.exptectedbillingdate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            string formattedDate = parsedDate.ToString("yyyy-MM-dd");


                            item.exptectedbillingdate = formattedDate; // required for api
                            item.statusId = "2"; // 2 for pending
                            item.IsRevised = true; // mandatory as per api

                            convertedDates.Add(formattedDate);
                        }
                    }
                    string endpointUrls = _configuration["ServiceUrl"].ToString().Trim() + "/api/ContractBilling/UpdateActualBilling";
                    apiManager = new ApiManager(endpointUrls, Session.Token);

                    string serializedDatas = JsonConvert.SerializeObject(selectedItems);

                    var (statusCodes, responseContents) = await apiManager.PostJson(serializedDatas);

                    if (statusCodes == HttpStatusCode.OK)
                    {
                        ViewData["success"] = "Send For Revision Succeeded.";
                        return Json(new { success = true, msg = "Submitted" });
                    }
                    else
                    {
                        ViewData["error"] = "Send For Approval Failed. Please try again.";
                        return Json(new { success = false, msg = "Failed" });
                    }
                }

                else
                {
                    if (selectedItems.Count <= 0)
                    {
                        TempData["error"] = "No items selected for approval.";
                        return RedirectToAction("ActualBillingReports", "Report");
                    }


                    foreach (var item in selectedItems)
                    {
                        if (!string.IsNullOrEmpty(item.exptectedbillingdate))
                        {
                            DateTime parsedDate = DateTime.ParseExact(item.exptectedbillingdate, "dd/MM/yyyy", CultureInfo.InvariantCulture);


                            string formattedDate = parsedDate.ToString("yyyy-MM-dd");


                            item.exptectedbillingdate = formattedDate;


                            convertedDates.Add(formattedDate);
                        }
                    }
                    string endpointUrl = _configuration["ServiceUrl"].ToString().Trim() + "/api/ContractBilling/SendForApproval";
                    apiManager = new ApiManager(endpointUrl, Session.Token);

                    string serializedData = JsonConvert.SerializeObject(selectedItems);

                    var (statusCode, responseContent) = await apiManager.PostJson(serializedData);

                    if (statusCode == HttpStatusCode.OK)
                    {
                        ViewData["success"] = "Send For Approval Succeeded.";
                        return Json(new { success = true, msg = "Submitted" });
                    }
                    else
                    {
                        ViewData["error"] = "Send For Approval Failed. Please try again.";
                        return Json(new { success = false, msg = "Failed" });
                    }
                }
            }
            catch (Exception ex)
            {
                ViewData["error"] = $"An error occurred: {ex.Message}";
                return Json(new { success = false, msg = $"An error occurred: {ex.Message}" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateBillingStatus() // this method for mark billing as billed/tobebilled
        {
            if (!WritePermission)
                return RedirectToAction("NotAuthorized", "Home");

            const string redirectAction = "ActualBillingReport";
            const string redirectController = "Report";

            try
            {
                var requestData = await Request.ReadFormAsync();
                var jsonData = requestData["data"]; // Extracting JSON data from form
                var data = JsonConvert.DeserializeObject<List<BilledStatus>>(jsonData);

                if (data == null || data.Count == 0)
                {
                    ViewData["error"] = "Billing data is empty.";
                    return RedirectToAction(redirectAction, redirectController);
                }

                IFormFileCollection Files = Request.Form.Files;
                var baseApiUrl = _configuration["ServiceUrl"].ToString().Trim();
                var apiURL = $"{baseApiUrl}/api/ContractBilling/UpdateBillingStatus";

                apiManager = new ApiManager(apiURL, Session.Token);

                System.Net.HttpStatusCode statusCode;
                string responseContent;

                (statusCode, responseContent) = await apiManager.PostWithFiles(JsonConvert.SerializeObject(data), Files);

                if (statusCode == HttpStatusCode.OK)
                {

                    return Json(new { success = true, msg = "Billing updated to Billed " });
                }
                else
                {
                    ViewData["error"] = "Error occurred while processing your request.";
                    return Json(new { success = false, msg = "Failed" });
                }
            }
            catch (Exception ex)
            {
                ViewData["error"] = $"An unexpected error occurred: {ex.Message}";
            }

            return RedirectToAction(redirectAction, redirectController);
        }

        public class BilledStatus
        {
            public int contractbillingid { get; set; }
            public bool isTobeBilled { get; set; } = false;
            public bool isBilled { get; set; } = false;
            public DateTime? Exptectedbillingdate { get; set; }
        }
        [HttpPost]
        public async Task<IActionResult> RevisionActionDh([FromBody] List<ApprovalRequestItem> selectedItems) // method for dh approval/rejection on revisions

        {
            try
            {

                if (!WritePermission)
                {
                    return RedirectToAction("NotAuthorized", "Home");
                }

                string endpointUrls = _configuration["ServiceUrl"].ToString().Trim() + "/api/ContractBilling/SendApprovalActualBilling";
                apiManager = new ApiManager(endpointUrls, Session.Token);

                string serializedDatas = JsonConvert.SerializeObject(selectedItems);

                var (statusCodes, responseContents) = await apiManager.PostJson(serializedDatas);

                if (statusCodes == HttpStatusCode.OK)
                {

                    return Json(new { success = true, msg = "Submitted" });
                }
                else
                {
                    return Json(new { success = false, msg = "Failed" });
                }

            }
            catch (Exception ex)
            {
                ViewData["error"] = $"An error occurred: {ex.Message}";
                return Json(new { success = false, msg = $"An error occurred: {ex.Message}" });
            }
        }

        #endregion



        #region swapbilling

        [HttpPost]
        public async Task<JsonResult> SwapBilling(string swapItem)
        {
            try
            {
                string serviceUrl = _configuration["ServiceUrl"].ToString().Trim();
                string endpointUrl = $"{serviceUrl}/api/ContractBilling/SwapFromProvisionToActual";
                var data = JsonConvert.DeserializeObject<List<swapbilling>>(swapItem);
                var apiManager = new ApiManager(endpointUrl, Session.Token);
                var requestData = JsonConvert.SerializeObject(data);

                var (statusCode, responsecontent) = await apiManager.PostJson(requestData);

                return Json(new { success = statusCode == HttpStatusCode.OK, msg = statusCode == HttpStatusCode.OK ? "Provisional billing has been move to actual billing" : "Failed" });
            }
            catch (Exception ex)
            {
                var errorMessage = $"An error occurred: {ex.Message}";
                ViewData["error"] = errorMessage;
                return Json(new { success = false, msg = errorMessage });
            }
        }

        public async Task<JsonResult> Actualbillingcheck(string data)
        {

            try
            {
                string serviceUrl = _configuration["ServiceUrl"].ToString().Trim();
                string endpointUrl = $"{serviceUrl}/api/ContractBilling/GetEmployeeActualBillins";
                 var checkbilling = JsonConvert.DeserializeObject<GetEmployeeActualBillinDto>(data);
                var apiManager = new ApiManager(endpointUrl, Session.Token);
                var requestData = JsonConvert.SerializeObject(checkbilling);

                var (statusCode, responsecontent) = await apiManager.PostJson(requestData);


                var employeecosting = JsonConvert.DeserializeObject<swapbilling>(responsecontent);


                return Json(new { success = statusCode == HttpStatusCode.OK, msg = employeecosting.costing <=0 ? "allowed" : employeecosting.costing.ToString() });
            }
            catch (Exception ex)
            {
                var errorMessage = $"An error occurred: {ex.Message}";
                ViewData["error"] = errorMessage;
                return Json(new { success = false, msg = errorMessage });
            }

        }

        public class swapbilling
        {
          
            public int contrtactbillingprovesionid { get; set; }
            public string BillingMonthyear { get; set; }
            public decimal? costing { get; set; }
            public DateTime? billingdate { get; set; }
        }
        public class GetEmployeeActualBillinDto
        {
            public int? contractEmployeeId { get; set; }
            public string? monthyear { get; set; }

        }
        #endregion
    }
}

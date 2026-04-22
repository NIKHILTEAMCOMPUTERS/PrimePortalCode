using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using RMS.Client.Models.Contract;
using RMS.Client.Models.Master;
using RMS.Client.Models.Vendor;
using RMS.Client.Utility;
using System.ComponentModel;
using System.Globalization;
using System.Net;
using System.Text;
namespace RMS.Client.Controllers.Contract
{
    public class ReportController : BaseController
    {
        private ApiManager apiManager;
        private IConfiguration _configuration;
        private IsoDateTimeConverter dateTimeConverter;
        public ReportController(IConfiguration configuration, IWebHostEnvironment env)
              : base(configuration, env)
        {
            _configuration = configuration;
            dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" };
        }


        public async Task<IActionResult> Index(string date)
        {
            ViewData["controller"] = this.ControllerContext.RouteData.Values["controller"].ToString();
            ViewData["action"] = this.ControllerContext.RouteData.Values["action"].ToString();

            if (!ReadPermission)
                return RedirectToAction("NotAuthorized", "Home");
            try
            {
                date = string.IsNullOrEmpty(date) ? DateTime.Now.ToString("MMM-yy", CultureInfo.InvariantCulture) : DateTime.Parse(date).ToString("MMM-yy", CultureInfo.InvariantCulture);

                var httpContent = new StringContent(JsonConvert.SerializeObject(new { date }), Encoding.UTF8, "application/json");

                // Use string interpolation to include the 'date' variable in the URL
                apiManager = new ApiManager($"{_configuration["ServiceUrl"].ToString().Trim()}/api/Report/GetBillingRptMonth?monthyear={date}");
                var (statusCode, responseContent) = await apiManager.Get(httpContent);

                if (statusCode == System.Net.HttpStatusCode.OK)
                {
                    var billingsreport = JsonConvert.DeserializeObject<List<Report>>(responseContent, dateTimeConverter);

                    // Filter the data based on the date parameter
                    billingsreport = billingsreport.Where(report => report.billingmonth == date && ((report.provesionbilling != null && report.provesionbilling > 0) || (report.actualbilling != null && report.actualbilling > 0))).ToList();
                    ViewBag.fDate = DateTime.Parse(date).ToString("yyyy-MM");
                    // Format fDate as "yyyyMM"
                    return View(billingsreport);
                }
                else
                {
                    // Handle error or return an appropriate view here
                }
            }
            catch (Exception ex)
            {
                // Handle exception, log it, and return an appropriate view or error response
            }

            // Return a default view or error response if something goes wrong
            return View();
        }

        public async Task<IActionResult> MonthWiseReport(string date)
        {
            if (!ReadPermission)
                return RedirectToAction("NotAuthorized", "Home");
            date = string.IsNullOrEmpty(date) ? DateTime.Now.ToString("MMM-yy", CultureInfo.InvariantCulture) : DateTime.Parse(date).ToString("MMM-yy", CultureInfo.InvariantCulture);
            ViewData["controller"] = this.ControllerContext.RouteData.Values["controller"].ToString();
            ViewData["action"] = this.ControllerContext.RouteData.Values["action"].ToString();
            List<MonthWisePOReport> poReports = new List<MonthWisePOReport>();
            apiManager = new ApiManager($"{_configuration["ServiceUrl"].ToString().Trim()}/api/Report/GetBillingRptMonthDA?monthyear={date}");
            var (statusCode, responseContent) = await apiManager.Get();
            if (statusCode == System.Net.HttpStatusCode.OK)
            {
                poReports = JsonConvert.DeserializeObject<List<MonthWisePOReport>>(responseContent);
            }

            ViewBag.fDate = DateTime.Parse(date).ToString("yyyy-MM");
            return View(poReports);
        }

        //public async Task<IActionResult> ContractBillingProvisionReport(string date, string mode = "empwise")

        //{
        //    if (Session.RoleName == "DH")
        //    {
        //        return RedirectToAction("ContractBillingProvisionReportDH", "Report");

        //    }
        //    ViewData["controller"] = this.ControllerContext.RouteData.Values["controller"].ToString();
        //    if (!ReadPermission)
        //        return RedirectToAction("NotAuthorized", "Home");
        //    if()
        //    string endpointUrl = $"{_configuration["ServiceUrl"].ToString().Trim()}/api/ContractBilling/approvallist";
        //    List<ContractBillingProvision> data = new List<ContractBillingProvision>();

        //    try
        //    {
        //        date = string.IsNullOrEmpty(date) ? DateTime.Now.ToString("MMM-yy", CultureInfo.InvariantCulture) : DateTime.Parse(date).ToString("MMM-yy", CultureInfo.InvariantCulture);

        //        var apiManager = new ApiManager(endpointUrl, Session.Token);
        //        {
        //            var (statusCode, responseContent) = await apiManager.Get();

        //            if (statusCode == System.Net.HttpStatusCode.OK)
        //            {
        //                var allData = JsonConvert.DeserializeObject<List<ContractBillingProvision>>(responseContent, dateTimeConverter);
        //                data = allData.Where(a =>a.MonthYear == date && (a.ProvisionAmount > 0 || a.ProvisionAmount == 0 && a.Status!= "Draft")).ToList();
        //            }
        //            else
        //            {

        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    ViewBag.fDate = DateTime.Parse(date).ToString("yyyy-MM");
        //    ViewBag.ApprovedCount = data.Count(a => a.Status == "Approved");
        //    ViewBag.PendingCount = data.Count(a => a.Status == "Pending");
        //    ViewBag.RejectedCount = data.Count(a => a.Status == "Rejected");
        //    ViewBag.Role = Session.RoleName;
        //    ViewData["controller"] = this.ControllerContext.RouteData.Values["controller"].ToString();
        //    return View(data);
        //}


        #region Provision request page
        public async Task<IActionResult> ContractBillingProvisionReport(string date, ProvisionViewModal data)
        {
            if (!ReadPermission)
                return RedirectToAction("NotAuthorized", "Home");
            try
            {
                if (Session.RoleName == "DH")
                {
                    return RedirectToAction("ContractBillingProvisionReportDH", "Report");
                }

                ViewData["controller"] = this.ControllerContext.RouteData.Values["controller"].ToString();

                if (!ReadPermission)
                {
                    return RedirectToAction("NotAuthorized", "Home");
                }

                date = string.IsNullOrEmpty(date) ? DateTime.Now.ToString("MMM-yy", CultureInfo.InvariantCulture) : DateTime.Parse(date).ToString("MMM-yy", CultureInfo.InvariantCulture);

                var projectData = await GetProjectData(date);
                var empData = await GetEmployeeData(date);

                data.Projectwiseprovisiondata = projectData;
                data.Empwiseprovisiondata = empData;

                ViewBag.fDate = DateTime.Parse(date).ToString("yyyy-MM");
                ViewBag.ApprovedCount = data.Empwiseprovisiondata.Count(a => a.Status == "Approved");
                ViewBag.PendingCount = data.Empwiseprovisiondata.Count(a => a.Status == "Pending");
                ViewBag.RejectedCount = data.Empwiseprovisiondata.Count(a => a.Status == "Rejected");
                ViewBag.TotalCount = data.Empwiseprovisiondata.Select(a => a.contractbillingprovesionid).Count();
             
                return View(new List<ProvisionViewModal> { data });
            }
            catch (Exception ex)
            {

                return RedirectToAction("Error", "Home");
            }
        }

        private async Task<List<ContractData>> GetProjectData(string date)
        {
            // Ensure the date is in the correct format ("MMM-yy")
            date = EnsureCorrectDateFormat(date);

            string endpointUrl = $"{_configuration["ServiceUrl"].ToString().Trim()}/api/ContractBilling/GetListProvisionRequestProjectWise";
            var apiManager = new ApiManager(endpointUrl, Session.Token);
            var (statusCode, responseContent) = await apiManager.Get();
            var allProjectData = JsonConvert.DeserializeObject<List<ContractData>>(responseContent, dateTimeConverter);

            foreach (var item in allProjectData)
            {
                // Ensure each item has the correct MonthYear format
                foreach (var valueItem in item.Value)
                {
                    valueItem.MonthYear = EnsureCorrectDateFormat(valueItem.MonthYear);
                }

                // Filter data with corrected MonthYear
                item.Value = item.Value.Where(a => a.MonthYear == date && (a.ProvisionAmount > 0)).ToList();
            }

            return allProjectData;
        }

        private async Task<List<ContractBillingProvision>> GetEmployeeData(string date)
        {
            // Ensure the date is in the correct format ("MMM-yy")
            date = EnsureCorrectDateFormat(date);

            string endpointUrl = $"{_configuration["ServiceUrl"].ToString().Trim()}/api/ContractBilling/approvallist";
            var apiManager = new ApiManager(endpointUrl, Session.Token);
            var (statusCode, responseContent) = await apiManager.Get();
            var allData = JsonConvert.DeserializeObject<List<ContractBillingProvision>>(responseContent, dateTimeConverter);

            // Ensure each item has the correct MonthYear format
            foreach (var dataItem in allData)
            {
                dataItem.MonthYear = EnsureCorrectDateFormat(dataItem.MonthYear);
            }

            // Filter data with corrected MonthYear
            return allData
                .Where(a => a.MonthYear == date && (a.ProvisionAmount > 0))
                .ToList();
        }

        // Helper method to ensure the date format is correct
        private string EnsureCorrectDateFormat(string monthYear)
        {
            if (monthYear.StartsWith("Sept"))
            {
                monthYear = monthYear.Replace("Sept", "Sep");
            }
            return monthYear;
        }


        #endregion



        [HttpGet]
        public async Task<IActionResult> ContractBillingProvisionReportDH()
        {
            ViewData["controller"] = this.ControllerContext.RouteData.Values["controller"].ToString();
            if (!ReadPermission && Session.RoleName != "DH" && Session.RoleName != "PracticeHead")
            return RedirectToAction("NotAuthorized", "Home");
            

            string endpointUrl = $"{_configuration["ServiceUrl"].ToString().Trim()}/api/ContractBilling/approvallist";

            try
            {
                var apiManager = new ApiManager(endpointUrl, Session.Token);
                var (statusCode, responseContent) = await apiManager.Get();

                if (statusCode == System.Net.HttpStatusCode.OK)
                {
                    var dataList = JsonConvert.DeserializeObject<List<ContractData>>(responseContent, dateTimeConverter);
                    ViewBag.Role = Session.RoleName;
                    ViewData["controller"] = this.ControllerContext.RouteData.Values["controller"].ToString();
                    ViewBag.TotalRequest = dataList.Count;
                    HttpContext.Session.SetInt32("ProvisionsRequestCount", dataList.Count);
                    ViewBag.PendingCount = dataList != null ? dataList.Count(a => a.Key.Status == "Pending") : 0;
                    ViewBag.RejectedCount = dataList != null ? dataList.Count(a => a.Key.Status == "Rejected") : 0;
                    return View(dataList); // Pass the list of data to the view
                }
                else
                {
                    // Handle the case where the API response is not OK (e.g., display an error message)
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during the API call
            }

            // Handle the case where the API call failed, and you need to return something appropriate

            return View(); // You might want to create an Error action in your Home controller
        }
        //[HttpGet]


        #region Billed report
        public async Task<IActionResult> BilledReport(string date)
        {
            if (!ReadPermission)
                return RedirectToAction("NotAuthorized", "Home");
            ViewData["controller"] = this.ControllerContext.RouteData.Values["controller"].ToString();
            ViewData["action"] = this.ControllerContext.RouteData.Values["action"].ToString();
            try
            {
                date = string.IsNullOrEmpty(date) ? DateTime.Now.ToString("MMM-yy", CultureInfo.InvariantCulture) : DateTime.Parse(date).ToString("MMM-yy", CultureInfo.InvariantCulture);

                var httpContent = new StringContent(JsonConvert.SerializeObject(new { date }), Encoding.UTF8, "application/json");

                // Use string interpolation to include the 'date' variable in the URL
                apiManager = new ApiManager($"{_configuration["ServiceUrl"].ToString().Trim()}/api/Report/GetRptMonthForBilled?monthyear={date}");
                var (statusCode, responseContent) = await apiManager.Get(httpContent);

                if (statusCode == System.Net.HttpStatusCode.OK)
                {
                    var billingsreport = JsonConvert.DeserializeObject<List<Report>>(responseContent, dateTimeConverter);

                    // Filter the data based on the date parameter
                    billingsreport = billingsreport.Where(report => report.billingmonth == date).ToList();

                    ViewBag.fDate = DateTime.Parse(date).ToString("yyyy-MM");
                    // Format fDate as "yyyyMM"
                    return View(billingsreport);
                }
                else
                {
                    // Handle error or return an appropriate view here
                }
            }
            catch (Exception ex)
            {
                // Handle exception, log it, and return an appropriate view or error response
            }

            // Return a default view or error response if something goes wrong
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> UpdateBilledData([FromBody] List<BilledDataItem> billedData)
        {

            try
            {
                var apiUrl = $"{_configuration["ServiceUrl"].ToString().Trim()}/api/Accountmanager/BilledAmountSubmission";
                if (billedData != null)
                {
                    var apiManager = new ApiManager(apiUrl, Session.Token);
                    var jsondata = JsonConvert.SerializeObject(billedData);
                    var (StatusCode, ResponseContent) = await apiManager.PostJson(jsondata);
                    if (StatusCode == HttpStatusCode.OK)
                    {
                        return Json(new { success = true, msg = "Billing Updated!!" });
                    }
                    else
                    {
                        return Json(new { success = false, msg = ResponseContent });
                    }
                }
                else
                {
                    return Json(new { success = false, msg = "posting data is empty" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, msg = ex.Message });
            }
        }

        public class BilledDataItem
        {
            public string ProvisionBillingId { get; set; }
            public string BilledAmount { get; set; }
        }
        #endregion



        #region [ActualBilllingReport]

        public async Task<IActionResult> ActualBillingReports(string date, string tabdata)
        {
            if (!ReadPermission)
                return RedirectToAction("NotAuthorized", "Home");
            var data = new ViewModalReport();
            ViewData["controller"] = this.ControllerContext.RouteData.Values["controller"].ToString();
            ViewData["action"] = this.ControllerContext.RouteData.Values["action"].ToString();
            try
            {
                var da = Session.RoleName == "DA" ? Session.EmployeeId : 0;
                var Daemployeeid = Convert.ToInt64(da);
                date = string.IsNullOrEmpty(date) ? DateTime.Now.ToString("MMM-yy", CultureInfo.InvariantCulture) : DateTime.Parse(date).ToString("MMM-yy", CultureInfo.InvariantCulture);
                var httpContent = new StringContent(JsonConvert.SerializeObject(new { date }), Encoding.UTF8, "application/json");

                var tasks = new List<Task>();

                // Concurrent HTTP requests
                tasks.Add(GetActualReport(Daemployeeid, date, httpContent, data));
                tasks.Add(GetProvisionReport(Daemployeeid, date, httpContent, data));

                await Task.WhenAll(tasks);
                ViewBag.Role = Session.RoleName;
                ViewBag.activetabdata = tabdata;
                return View(data);
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.ToString(); // Consider logging the error instead
                return View(data);
            }
        }

        private async Task GetActualReport(long? da, string date, HttpContent httpContent, ViewModalReport data)
        {
            string baseUrl = _configuration["ServiceUrl"].ToString().Trim();
            string apiUrl;

            if (da == 0)
            {
                apiUrl = $"{baseUrl}/api/Report/GetRptMonthForBilled-TobeBilled?monthyear={date}";
            }
            else
            {
                apiUrl = $"{baseUrl}/api/Report/GetRptMonthForBilled-TobeBilled?monthyear={date}&DeliveryAnchorId={da}";
            }

            var apiManager = new ApiManager(apiUrl);
            var (statusCode, responseContent) = await apiManager.Get(httpContent);

            if (statusCode == System.Net.HttpStatusCode.OK)
            {
                data.ActualReportData = JsonConvert.DeserializeObject<List<Report>>(responseContent, dateTimeConverter)
                    .Where(report => report.billingmonth == date && report.billing > 0).ToList();
                ViewBag.fDate = DateTime.Parse(date).ToString("yyyy-MM");
            }
        }


        private async Task GetProvisionReport(long? da, string date, HttpContent httpContent, ViewModalReport data)
        {
            string baseUrl = _configuration["ServiceUrl"].ToString().Trim();
            string apiUrl;

            if (da == 0)
            {
                apiUrl = $"{baseUrl}/api/Report/GetRptMonthForBilled?monthyear={date}";
            }
            else
            {
                apiUrl = $"{baseUrl}/api/Report/GetRptMonthForBilled?monthyear={date}&DeliveryAnchorId={da}";
            }

            var apiManager = new ApiManager(apiUrl);
            var (statusCode, responseContent) = await apiManager.Get(httpContent);

            if (statusCode == System.Net.HttpStatusCode.OK)
            {
                data.ProvisionReportData = JsonConvert.DeserializeObject<List<Report>>(responseContent, dateTimeConverter)
                    .Where(report => report.billingmonth == date).ToList();
            }
        }
        [HttpGet]
        public async Task<IActionResult> ContractBillingReportDH()
        {
            ViewData["controller"] = this.ControllerContext.RouteData.Values["controller"].ToString();
            if (!ReadPermission && Session.RoleName != "DH")
                return RedirectToAction("NotAuthorized", "Home");

            ViewData["controller"] = this.ControllerContext.RouteData.Values["controller"].ToString();
            if (!ReadPermission && Session.RoleName != "DH")
                return RedirectToAction("NotAuthorized", "Home");

            string endpointUrl = $"{_configuration["ServiceUrl"].ToString().Trim()}/api/ContractBilling/GetActualBillingApprovalList";

            try
            {
                var apiManager = new ApiManager(endpointUrl, Session.Token);
                var (statusCode, responseContent) = await apiManager.Get();

                if (statusCode == System.Net.HttpStatusCode.OK)
                {
                    var dataList = JsonConvert.DeserializeObject<List<ActualBillingRevisions>>(responseContent, dateTimeConverter);
                    ViewBag.Role = Session.RoleName;
                    ViewData["controller"] = this.ControllerContext.RouteData.Values["controller"].ToString();
                    ViewBag.TotalRequest = dataList.Count;
                    HttpContext.Session.SetInt32("RevisionsRequestCount", dataList.Count);
                    ViewBag.PendingCount = dataList != null ? dataList.SelectMany(a => a.value).Count(v => v.Status == "Pending") : 0;
                    ViewBag.RejectedCount = dataList != null ? dataList.SelectMany(a => a.value).Count(v => v.Status == "Rejected") : 0;
                    return View(dataList);
                }

            }
            catch (Exception ex)
            {

            }
            return View();
        }


        [HttpGet]
        public async Task <IActionResult> ResourceReport()
        {
            if (!ReadPermission)
                return RedirectToAction("NotAuthorized", "Home");
            string endpointUrl = $"{_configuration["ServiceUrl"].ToString().Trim()}/api/Report/GetResourcesInfoForHR";
            var   dataList=new List<ResourceInfoForHRDto>();    
            try
            {
                ViewBag.Practices = await GetPractice();
                ViewBag.Subpractices = await GetSubPractice();
                var apiManager = new ApiManager(endpointUrl, Session.Token);
                var (statusCode, responseContent) = await apiManager.Get();

                if (statusCode == System.Net.HttpStatusCode.OK)
                {
                     dataList = JsonConvert.DeserializeObject<List<ResourceInfoForHRDto>>(responseContent, dateTimeConverter);
                   
                }
                return View(dataList);
               
            }
            catch (Exception ex)
            {
                ViewData["error"] = ex.ToString();  
               return View(dataList);
            }
            
        }

       
        public async Task<IActionResult> GetAllBillingDataReourceWise()
        {
            if (!ReadPermission)
                return RedirectToAction("NotAuthorized", "Home");
            string endpointUrl = $"{_configuration["ServiceUrl"].ToString().Trim()}/api/Employee/GetAllBillingDataReourceWise";
            var dataList = new List<ResourceInfoForHRDto>();
            try
            {
                ViewBag.Practices = await GetPractice();
                ViewBag.Subpractices = await GetSubPractice();
                var apiManager = new ApiManager(endpointUrl, Session.Token);
                var (statusCode, responseContent) = await apiManager.Get();

                if (statusCode == System.Net.HttpStatusCode.OK)
                {
                    dataList = JsonConvert.DeserializeObject<List<ResourceInfoForHRDto>>(responseContent, dateTimeConverter);

                }
                return View(dataList);

            }
            catch (Exception ex)
            {
                ViewData["error"] = ex.ToString();
                return View(dataList);
            }

        }

        #endregion
    }
}

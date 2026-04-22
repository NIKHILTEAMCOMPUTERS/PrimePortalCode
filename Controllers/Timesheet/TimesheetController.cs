using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RMS.Client.Models.Common;
using RMS.Client.Models.Master;
using RMS.Client.Models.Timesheet;
using RMS.Client.Utility;
using System.Globalization;
using System.Security.Policy;
using System.Text;

namespace RMS.Client.Controllers.Timesheet
{
    public class TimesheetController : BaseController
    {
        private ApiManager apiManager;
        private readonly IWebHostEnvironment _env;

        private IConfiguration _configuration;
        public TimesheetController(IConfiguration configuration, IWebHostEnvironment env)
              : base(configuration, env)
        {
            _configuration = configuration;
            _env = env;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
{
    GenericResponse<List<TimeSheetHeader>> data = new GenericResponse<List<TimeSheetHeader>>();
    List<MonthlyEntryCount> monthlyEntryCounts = new List<MonthlyEntryCount>();
    int currentYear = DateTime.Now.Year; // Get the current year

    try
    {
        ViewBag.name = Session.Name;
        ViewData["controller"] = this.ControllerContext.RouteData.Values["controller"].ToString();

        var url = _configuration["ServiceUrl"] + "/api/Timesheet/GetHeaderItems";
        if (!string.IsNullOrEmpty(url))
        {
            apiManager = new ApiManager(url, Session.Token);
            var res = await apiManager.Get();
            if (res.Item1 == System.Net.HttpStatusCode.OK)
            {
                // Deserialize the main data object
                data = JsonConvert.DeserializeObject<GenericResponse<List<TimeSheetHeader>>>(res.Item2);

                // Use Newtonsoft.Json to parse the response for additional data
                var additionalData = JObject.Parse(res.Item2)["data"];
                if (additionalData != null)
                {
                    var monthlyEntryCountsToken = additionalData["monthlyEntryCounts"];
                    if (monthlyEntryCountsToken != null)
                        monthlyEntryCounts = monthlyEntryCountsToken.ToObject<List<MonthlyEntryCount>>();

                    ViewBag.leaveCount = additionalData["leaveCount"]?.ToObject<int>() ?? 0;
                }

                if (data.DataObject != null && data.DataObject.Any())
                {
                    //// Filter for the current year
                    //data.DataObject = data.DataObject
                    //    .Where(item => DateTime.Parse(item.Monthyear).Year == currentYear)
                    //    .ToList();

                    // Set TotalWorkingDay for each item
                    foreach (var item in data.DataObject)
                    {
                        item.TotalWorkigDay = GetWorkingDays(item.Monthyear);
                    }

                    // Extract the grouped count from the response message (if exists)
                    var groupedCountMatch = System.Text.RegularExpressions.Regex.Match(data.responseMessage, @"Grouped count of unique dates: (\d+)");
                    if (groupedCountMatch.Success)
                    {
                        ViewBag.groupedCount = int.Parse(groupedCountMatch.Groups[1].Value);
                    }

                    // Count total months for the current year
                    ViewBag.totalmonth = data.DataObject.Count(a => a.Timesheetid > 0);
                }

                // Store monthly entry counts in ViewBag for access in the view
                ViewBag.MonthlyEntryCounts = monthlyEntryCounts;
            }
        }

        if (data.DataObject != null)
        {
            return View(data.DataObject.OrderByDescending(item => Convert.ToDateTime(item.Monthyear)));
        }
        else
        {
            return View(new List<TimeSheetHeader>());
        }
    }
    catch (Exception ex)
    {
        ViewBag.ErrorMessage = "An error occurred: " + ex.Message;
        return View(new List<TimeSheetHeader>());
    }
}




        public int GetWorkingDays(string monthYear)
        {
            var date = DateTime.ParseExact(monthYear, "MMM-yy", System.Globalization.CultureInfo.InvariantCulture);
            return Enumerable.Range(1, DateTime.DaysInMonth(date.Year, date.Month))
                             .Count(d => !new[] { DayOfWeek.Saturday, DayOfWeek.Sunday }
                             .Contains(new DateTime(date.Year, date.Month, d).DayOfWeek));
        }


        [HttpPost]
        public async Task<JsonResult> CreateMonth(string date)
        {
            try
            {
                try
                {
                    date = string.IsNullOrEmpty(date) ? DateTime.Now.ToString("MMM-yy", CultureInfo.InvariantCulture) : DateTime.Parse(date).ToString("MMM-yy", CultureInfo.InvariantCulture);

                }
                catch (FormatException)
                {
                    return Json(new { success = false, msg = "Select atleast one month" });
                }

                var url = _configuration["ServiceUrl"] + "/api/Timesheet/CreateHeader/" + date;
                if (!string.IsNullOrEmpty(url))
                {
                    apiManager = new ApiManager(url, Session.Token);
                    var res = await apiManager.SendRequestAsync(HttpMethod.Post);
                    if (res.Item1 == System.Net.HttpStatusCode.OK)
                    {

                        return Json(new { success = true, msg = "Month has been Submitted" });

                    }
                    else
                    {
                        return Json(new { success = false, msg = "Something went wrong" });
                    }
                }
                return Json(new { success = false, msg = "Something went wrong" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, msg = $"An error occured : {ex.Message}" }); ;
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetProjectAgainstUser()
        {
            try
            {
                var data = await GetProjectForTimesheet();
                return Json(new Tuple<bool, Object>(true, data));
            }
            catch (Exception e)
            {
                return Json(new Tuple<bool, string>(false, e.Message));
            }

        }


        [HttpGet]
        public async Task<IActionResult> Add(int id, string month)
        {
            List<TimesheetsDetailDTO> details = new List<TimesheetsDetailDTO>();
            try
            {

                ViewBag.month = month;

                ViewBag.GetCategoryOfActivity = await GetCategoryOfActivityForTimesheet();
                ViewBag.cust = Customerslist().Result;
                ViewBag.activeCustomer = CustomerActiveList().Result;
                ViewData["controller"] = this.ControllerContext.RouteData.Values["controller"].ToString();
                ViewBag.project = await GetProjectForTimesheet();
                ViewBag.department = await GetDepartment();
                List<TimesheetMasterDTO> timesheets = new List<TimesheetMasterDTO>();
                apiManager = new ApiManager(_configuration["ServiceUrl"].ToString().Trim() + $"/api/TimeSheet/GetDetailListByHeaderId/{id}");
                var (statusCode, ResponseContent) = await apiManager.Get();
                if (statusCode == System.Net.HttpStatusCode.OK)
                {

                    var timesheet = JsonConvert.DeserializeObject<GenericResponse<List<TimesheetsDetailDTO>>>(ResponseContent);
                    if (timesheet.responseCode == 200)
                    {


                        details = timesheet.DataObject;
                        ViewBag.LeaveCount = details.Where(a => a.categoryofactivityid == 19).Count();
                        return await Task.Run(() => View("Add", timesheet.DataObject.OrderBy(a => a.Timesheetdate)));
                    }
                }
                return View(details);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred: " + ex.Message;
                return View(details);
            }

        }
        [HttpPost]
        public async Task<IActionResult> Add(string timesheetData)
        {
            try
            {
                var datecheck = JsonConvert.DeserializeObject<TimesheetsDetailDTO>(timesheetData);
                if (datecheck != null)
                {
                    DateTime givenDate = datecheck.Timesheetdate; // Use directly since it's already a DateTime
                    DateTime today = DateTime.Today;
                    DateTime allowedDate = today.AddDays(-2); // 2 days back is allowed
                    if (givenDate < allowedDate)
                    {
                        return Json(new { success = false, msg = "Error: Timesheet Date is more than 2 days old." });

                    }
                }
                var url = _configuration["ServiceUrl"] + "/api/TimeSheet/PostDetailItems";
                apiManager = new ApiManager(url, Session.Token);
                var res = await apiManager.PostJson(timesheetData);
                if (res.Item1 == System.Net.HttpStatusCode.OK)
                {
                    var data = JsonConvert.DeserializeObject<GenericResponse<TimesheetsDetailDTO>>(res.Item2);
                    if (data.responseCode != 200)
                    {
                        return Json(new { success = false, msg = data.responseMessage });
                    }
                    else
                    {
                        return Json(new { success = true, msg = data.DataObject.Timesheetdetailid });
                    }
                }
                else
                {
                    return Json(new { success = false, msg = res.Item2 });
                }
            }
            catch (Exception e)
            {
                return Json(new { success = false, msg = "Something went wrong!" });
            }
        }

        [HttpPost]
        public async Task<JsonResult> Delete(int id)
        {
            try
            {
                var url = _configuration["ServiceUrl"].ToString().Trim() + $"/api/Timesheet/DeleteTimeSheetDetailItemAsync/{id}";
                apiManager = new ApiManager(url, Session.Token);
                var data = await apiManager.SendRequestAsync(HttpMethod.Post);
                if (data.Item1 == System.Net.HttpStatusCode.OK)
                {
                    var response = JsonConvert.DeserializeObject<GenericResponse<Object>>(data.Item2);
                    if (response.responseCode != 200)
                    {
                        return Json(new { success = false, msg = response.DataObject });

                    }
                    return Json(new { success = true, msg = "Sucesfully deleted timesheet detail" });
                }
                else
                {
                    return Json(new { success = false, msg = "Something went wrong" });

                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, msg = $"An error has occurred: {ex.Message}" });
            }
        }
        public async Task<JsonResult> SubmitTimeSheetDetails(List<int> ids)
        {
            try
            {
                var url = _configuration["ServiceUrl"] + "/api/Timesheet/submitTimeSheetDetailItemAsync";
                if (!string.IsNullOrEmpty(url))
                {
                    apiManager = new ApiManager(url, Session.Token);

                    var res = await apiManager.PostJson(JsonConvert.SerializeObject(ids));
                    if (res.Item1 == System.Net.HttpStatusCode.OK)
                    {
                        var data = JsonConvert.DeserializeObject<GenericResponse<Object>>(res.Item2);
                        if (data.responseCode != 200)
                        {
                            return Json(new { success = false, msg = data.responseMessage });
                        }
                        else
                        {
                            return Json(new { success = true, msg = data.responseMessage });
                        }
                    }
                    else
                    {
                        return Json(new { success = false, msg = res.Item2 });
                    }
                }
                return Json(new { success = false, msg = "Something went wrong" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, msg = $"An error occured : {ex.Message}" }); ;
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetMemberTimesheet(bool isSummary = true)
        {
            ViewBag.Month = DateTime.Now.ToString("yyyy-MM");
            ViewBag.MonthName = DateTime.Now.ToString("MMM");
            try
            {
                ViewBag.Projects = GetProjectsByDA(Session.EmployeeId).Result;
                ViewBag.Resource = GetResourceByDA(Session.EmployeeId).Result;
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred: " + ex.Message;
                return View();
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetManagerTimesheet(string date, int projectId = 0, int employeeId = 0)
        {
            try
            {
                date = string.IsNullOrEmpty(date) ? DateTime.Now.ToString("MMM-yy", CultureInfo.InvariantCulture) : DateTime.Parse(date).ToString("MMM-yy", CultureInfo.InvariantCulture);
                var url = _configuration["ServiceUrl"] + "/api/Timesheet/GetTimesheetsByManagerId/" + date + "/" + projectId + "/" + employeeId;
                if (!string.IsNullOrEmpty(url))
                {
                    apiManager = new ApiManager(url, Session.Token);
                    var res = await apiManager.SendRequestAsync(HttpMethod.Get);
                    if (res.Item1 == System.Net.HttpStatusCode.OK)
                    {
                        var data = JsonConvert.DeserializeObject<GenericResponse<Object>>(res.Item2);
                        if (data.responseCode == 200)
                        {
                            return Json(new { success = true, msg = data.DataObject.ToString() });
                        }
                        else
                        {
                            return Json(new { success = false, msg = "no data found" });
                        }
                    }
                }
                return Json(new { success = false, msg = "Something went wrong" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, msg = $"An error occurred : {ex.Message}" }); ;
            }
        }

        [HttpGet]
        public async Task<IActionResult> TimesheetHistory(bool isSummary = true)
        {
            try
            {
                ViewData["controller"] = this.ControllerContext.RouteData.Values["controller"].ToString();

                ViewBag.isSummaryHistory = isSummary;
                var temp = new List<Timesheets>();
                apiManager = new ApiManager(_configuration["ServiceUrl"].ToString().Trim() + $"/api/TimeSheet/GetTimesheetHistory/{Session.EmployeeId}");
                var (statusCode, responseContent) = await apiManager.Get();

                if (statusCode == System.Net.HttpStatusCode.OK)
                {
                    if (responseContent != null)
                    {
                        temp = JsonConvert.DeserializeObject<List<Timesheets>>(responseContent);
                    }
                }
                return View(temp);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred: " + ex.Message;
                return View();
            }
        }
        [HttpPost]
        public async Task<JsonResult> ManagerAction(string data)
        {
            try
            {
                var url = $"{_configuration["ServiceUrl"].Trim()}/api/Timesheet/ManagerAction";
                var (statusCode, responseContent) = await new ApiManager(url, Session.Token).PostJson(data);
                var response = JsonConvert.DeserializeObject<GenericResponse<object>>(responseContent);
                bool isSuccess = statusCode == System.Net.HttpStatusCode.OK && response.responseCode == 200;
                return Json(new { success = isSuccess, msg = isSuccess ? "Timesheet Declined" : response?.DataObject?.ToString() ?? "something went wrong" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, msg = $"An error has occurred: {ex.Message}" });
            }
        }

        internal async Task<List<SelectListItem>> CustomerActiveList()
        {
            var items = new List<SelectListItem>();

            apiManager = new ApiManager(_configuration["ServiceUrl"].ToString().Trim() + $"/api/Customer/EmployeeActiveProjects/{Session.EmployeeId}"); //--TOKEN
            var (statusCode, responseContent) = await apiManager.Get();

            bool isTeamComputersPresent = false;
            if (statusCode == System.Net.HttpStatusCode.OK)
            {

                if (responseContent != null)
                {
                    dynamic apiResponse = JsonConvert.DeserializeObject<dynamic>(responseContent);

                    foreach (var item in apiResponse)
                    {
                        string customerId = item.customerId;
                        string customerName = item.customerCompanyName;

                        if (customerId == "749") isTeamComputersPresent = true;

                        items.Add(new SelectListItem
                        {
                            Value = customerId.ToString(),
                            Text = customerName.ToString()
                        });
                    }
                }
            }
            if (!isTeamComputersPresent)
            {
                items.Add(new SelectListItem
                {
                    Value = "749",
                    Text = "TEAM COMPUTERS PVT LTD"
                });
            }

            return items;
        }
        
        [HttpGet]
        public async Task<IActionResult> TimesheetExcel(string date, int projectId = 0, int employeeId = 0, int headerId = 0)
        {
            try
            {
                date = string.IsNullOrEmpty(date) ? DateTime.Now.ToString("MMM-yy", CultureInfo.InvariantCulture) : DateTime.Parse(date).ToString("MMM-yy", CultureInfo.InvariantCulture);
                string fileName = "TimesheetReport_" + Guid.NewGuid() + ".csv";
                string filePath = "";

                if (headerId == 0)
                {
                    var url = _configuration["ServiceUrl"] + "/api/Timesheet/GetTimesheetsByManagerId/" + date + "/" + projectId + "/" + employeeId;
                    apiManager = new ApiManager(url, Session.Token);
                    var res = await apiManager.SendRequestAsync(HttpMethod.Get);

                    if (res.Item1 == System.Net.HttpStatusCode.OK)
                    {
                        var data = JsonConvert.DeserializeObject<GenericResponse<List<dynamic>>>(res.Item2);

                        string directoryPath = Path.Combine(_env.WebRootPath, "timesheet", "timecsv");
                        Directory.CreateDirectory(directoryPath);
                        filePath = Path.Combine(directoryPath, fileName);

                        WriteToCSV(data.DataObject, filePath);
                        var fileUrl = $"/timesheet/timecsv/{fileName}";

                        // Prepare the JSON response
                        var jsonResult = Json(new { success = true, fileUrl });

                        // Schedule file deletion asynchronously
                        _ = Task.Run(() => DeleteFile(filePath));

                        // Return the JSON response
                        return jsonResult;
                    }
                }
                else
                {
                    var url = _configuration["ServiceUrl"] + $"/api/TimeSheet/GetDetailListByHeaderId/{headerId}";
                    apiManager = new ApiManager(url, Session.Token);
                    var res = await apiManager.Get();

                    if (res.Item1 == System.Net.HttpStatusCode.OK)
                    {
                        var data = JsonConvert.DeserializeObject<GenericResponse<List<dynamic>>>(res.Item2);

                        string directoryPath = Path.Combine(_env.WebRootPath, "empTimesheet", "timecsv");
                        Directory.CreateDirectory(directoryPath);
                        filePath = Path.Combine(directoryPath, fileName);

                        ViewBag.GetCategoryOfActivity = await GetCategoryOfActivityForTimesheet();
                        var activityCategories = ViewBag.GetCategoryOfActivity as List<SelectListItem>;

                        WriteToCSVEmp(data.DataObject, filePath, activityCategories);
                        var fileUrl = $"/empTimesheet/timecsv/{fileName}";

                        // Prepare the JSON response
                        var jsonResult = Json(new { success = true, fileUrl });

                        // Schedule file deletion asynchronously
                        _ = Task.Run(() => DeleteFile(filePath));

                        // Return the JSON response
                        return jsonResult;
                    }
                }

                return Json(new { success = false, msg = "Something went wrong" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }


        private void DeleteFile(string filePath)
        {
            Task.Delay(30000).Wait();

            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
        }

        static void WriteToCSV(IEnumerable<dynamic> list, string filePath)
        {
            var csv = new StringBuilder();

            // Add headers in the first row
            csv.AppendLine("EmpId,Employeename,Companyemail,Projectname,Timesheetdate,Dayhours,CategoryOfActivity,Activity");

            // Append data rows starting from the second row
            foreach (var item in list)
            {
                var row = new StringBuilder();

                string activity = ((string)item.activity ?? string.Empty).Replace("\"", "\"\"");
                string projectName = ((string)item.projectname ?? string.Empty).Replace("\"", "\"\"");

                // Collect values for each column
                row.Append($"{item.userid},");
                row.Append($"{item.employeename},");
                row.Append($"{item.companyemail},");
                //row.Append($"{item.projectname},");
                row.Append($"\"{projectName}\",");

                row.Append($"{item.timesheetdate:yyyy-MM-dd},");
                row.Append($"{item.dayhours},");
                row.Append($"{item.categoryofactivityname},");
                //row.Append($"{item.activity}");
                row.Append($"\"{activity}\",");

                csv.AppendLine(row.ToString());
            }

            // Write content to file
            System.IO.File.WriteAllText(filePath, csv.ToString());
        }

        static void WriteToCSVEmp(IEnumerable<dynamic> list, string filePath, List<SelectListItem> activityCategories)
        {
            var csv = new StringBuilder();

            // Add headers in the first row
            csv.AppendLine("Timesheetdate,Dayhours,CategoryfActivity,Activity,Projectname,Status");

            // Append data rows starting from the second row
            foreach (var item in list)
            {
                var row = new StringBuilder();

                // Collect values for each column
                row.Append($"{item.timesheetdate:yyyy-MM-dd},");
                row.Append($"{item.dayhours},");
                var categoryName = "Other";
                var categoryItem = activityCategories.FirstOrDefault(c => c.Value == item.categoryofactivityid.ToString());

                if (categoryItem != null)
                {
                    categoryName = categoryItem.Text;
                }
                string activity = ((string)item.activity ?? string.Empty).Replace("\"", "\"\"");
                string projectName = ((string)item.companyname ?? string.Empty).Replace("\"", "\"\"");

                row.Append($"\"{categoryName}\",");
                row.Append($"\"{activity}\",");
                row.Append($"\"{projectName}\",");

                row.Append(item.isdrafted == true ? "Drafted" : "Submitted");
                csv.AppendLine(row.ToString());
            }

            // Write content to file
            System.IO.File.WriteAllText(filePath, csv.ToString());
        }


        public async Task <IActionResult>UserMannual()
        {
            return View();
        }
        [HttpGet]
        //public async Task<IActionResult> TeamTimesheet(DateTime? monthyear)
        //{
        //    try
        //    {
        //        if (Session.RoleName != "HR" && Session.RoleName != "DA")
        //        {
        //            return RedirectToAction("NotAuthorized", "Home");
        //        }

        //        DateTime date;

        //        if (!monthyear.HasValue || monthyear.Value == DateTime.MinValue || monthyear.Value == new DateTime(1, 1, 1))
        //        {
        //            date = DateTime.Now;
        //        }
        //        else
        //        {
        //            date = monthyear.Value;
        //        }

        //        string monthName = date.ToString("yyyy-MM"); // Correct format for year and month
        //        ViewBag.monthname = monthName;

        //        // Format the date
        //        string formattedDate = date.ToString("MMM-yy");

        //        List<TimesheetHRDto> details = new List<TimesheetHRDto>();
        //        string monthYearFilter = (monthyear.HasValue && monthyear.Value != DateTime.MinValue && monthyear.Value != new DateTime(1, 1, 1))
        //   ? monthyear.Value.ToString("MMM-yy") // Use provided monthyear
        //   : DateTime.Now.ToString("MMM-yy");
        //        apiManager = new ApiManager(_configuration["ServiceUrl"].ToString().Trim() + $"/api/TimeSheet/GetTdeTimesheetData/{formattedDate}");

        //        // Call the API
        //        var (statusCode, responseContent) = await apiManager.Get();

        //        if (statusCode == System.Net.HttpStatusCode.OK && !string.IsNullOrEmpty(responseContent))
        //        {
        //            // Deserialize the API response dynamically
        //            dynamic apiResponse = JsonConvert.DeserializeObject<dynamic>(responseContent);

        //            foreach (var item in apiResponse)
        //            {
        //                // Safely extract properties from the dynamic object
        //                string monthYear = item.monthYear;
        //                string reportingHead = item.reportingHead;
        //                string employeeName = item.employeeName;
        //                string employeeUserId = item.employeeUserId;
        //                int detailCount = item.detailCount;

        //                // Populate TimesheetHRDto object
        //                TimesheetHRDto timesheetHR = new TimesheetHRDto
        //                {
        //                    MonthYear = monthYear,
        //                    ReportingHead = reportingHead,
        //                    EmployeeName = employeeName,
        //                    EmployeeUserId = employeeUserId,
        //                    DetailCount = detailCount
        //                };

        //                details.Add(timesheetHR);
        //            }
        //        }

        //        // Return the view with the details
        //        return View(details.Where(a => a.MonthYear == formattedDate));
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the exception (not shown here for simplicity)
        //        return View("Error", ex.Message); // Optionally return an error view
        //    }
        //}
        public async Task<IActionResult> TeamTimesheet(DateTime? monthyear)
        {
            try
            {
                if (Session.RoleName != "HR" && Session.RoleName != "DA")
                {
                    return RedirectToAction("NotAuthorized", "Home");
                }

                DateTime date;

                if (!monthyear.HasValue || monthyear.Value == DateTime.MinValue || monthyear.Value == new DateTime(1, 1, 1))
                {
                    date = DateTime.Now;
                }
                else
                {
                    date = monthyear.Value;
                }

                string monthName = date.ToString("yyyy-MM"); // Correct format for year and month
                ViewBag.monthname = monthName;

                // Format the date
                string formattedDate = date.ToString("MMM-yy");

                List<TimesheetHRDto> details = new List<TimesheetHRDto>();

                // Calculate total working days
                int totalWorkingDays = GetTotalWorkingDays(date.Year, date.Month);
                ViewBag.TotalWorkingDays = totalWorkingDays;

                // API call to fetch timesheet data
                apiManager = new ApiManager(_configuration["ServiceUrl"].ToString().Trim() + $"/api/TimeSheet/GetTdeTimesheetData/{formattedDate}");
                var (statusCode, responseContent) = await apiManager.Get();

                if (statusCode == System.Net.HttpStatusCode.OK && !string.IsNullOrEmpty(responseContent))
                {
                    dynamic apiResponse = JsonConvert.DeserializeObject<dynamic>(responseContent);

                    foreach (var item in apiResponse)
                    {
                        string monthYear = item.monthYear;
                        string reportingHead = item.reportingHead;
                        string employeeName = item.employeeName;
                        string employeeUserId = item.employeeUserId;
                        int detailCount = item.detailCount;

                        TimesheetHRDto timesheetHR = new TimesheetHRDto
                        {
                            MonthYear = monthYear,
                            ReportingHead = reportingHead,
                            EmployeeName = employeeName,
                            EmployeeUserId = employeeUserId,
                            DetailCount = detailCount
                        };

                        details.Add(timesheetHR);
                    }
                }

                // Calculate total timesheets left
                int totalTimesheetsLeft = CalculateTotalTimesheetsLeft(details, totalWorkingDays);
                ViewBag.TotalTimesheetsLeft = totalTimesheetsLeft;

                return View(details.Where(a => a.MonthYear == formattedDate));
            }
            catch (Exception ex)
            {
                // Log the exception (not shown here for simplicity)
                return View("Error", ex.Message); // Optionally return an error view
            }
        }

        // Helper method to calculate total working days
        private int GetTotalWorkingDays(int year, int month)
        {
            var allDays = Enumerable.Range(1, DateTime.DaysInMonth(year, month))
                                    .Select(day => new DateTime(year, month, day));
            return allDays.Count(date => date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday);
        }

        // Helper method to calculate total timesheets left
        private int CalculateTotalTimesheetsLeft(List<TimesheetHRDto> timesheetDetails, int totalWorkingDays)
        {
            // Assuming detailCount represents the submitted timesheets for each employee
            int totalEmployees = timesheetDetails.Count;
            int totalSubmittedTimesheets = timesheetDetails.Sum(d => d.DetailCount);

            // Timesheets left = (Total employees * Total working days) - Total submitted timesheets
            return (totalEmployees * totalWorkingDays) - totalSubmittedTimesheets;
        }

        static void WriteToHr(IEnumerable<dynamic> list, string filePath)
        {
            var csv = new StringBuilder();

            // Add headers in the first row
            csv.AppendLine("ReportingHead,EmployeeName,TMC,MonthYear,DetailCount,TotalWorkingDays,TotalTimesheetsLeft");

            // Append data rows starting from the second row
            foreach (var item in list)
            {
                var row = new StringBuilder();

                // Collect values for each column
                string reportingHead = item.ReportingHead ?? string.Empty;
                string employeeName = item.EmployeeName ?? string.Empty;
                string tmc = item.EmployeeUserId ?? string.Empty;
                string monthYear = item.MonthYear ?? string.Empty;
                int detailCount = item.DetailCount ?? 0;
                int totalWorkingDays = item.TotalWorkingDays;
                int totalTimesheetsLeft = item.TotalTimesheetsLeft;

                // Build the row
                row.Append($"{reportingHead},");
                row.Append($"{employeeName},");
                row.Append($"{tmc},");
                row.Append($"{monthYear},");
                row.Append($"{detailCount},");
                row.Append($"{totalWorkingDays},");
                row.Append($"{totalTimesheetsLeft}");

                // Add the row to the CSV
                csv.AppendLine(row.ToString());
            }

            // Write content to file
            System.IO.File.WriteAllText(filePath, csv.ToString());
        }


        [HttpGet]
        public async Task<IActionResult> HrDataExcel(DateTime? monthyear)
        {
            try
            {
                if (Session.RoleName != "HR")
                {
                    return Unauthorized();
                }

                string fileName = "TimesheetHrReport_" + Guid.NewGuid() + ".csv";
                string filePath = "";
                string monthYearFilter = (monthyear.HasValue && monthyear.Value != DateTime.MinValue && monthyear.Value != new DateTime(1, 1, 1))
                    ? monthyear.Value.ToString("MMM-yy") // Use provided monthyear
                    : DateTime.Now.ToString("MMM-yy");

                var url = _configuration["ServiceUrl"] + $"/api/Timesheet/GetTdeTimesheetData/{monthYearFilter}";
                apiManager = new ApiManager(url, Session.Token);
                var res = await apiManager.SendRequestAsync(HttpMethod.Get);

                if (res.Item1 == System.Net.HttpStatusCode.OK)
                {
                    var data = JsonConvert.DeserializeObject<List<TimesheetHRDto>>(res.Item2);

                    // Calculate total working days for the selected month
                    DateTime monthDate = monthyear ?? DateTime.Now;
                    int totalWorkingDays = GetTotalWorkingDaysInMonth(monthDate);

                    // Prepare data with additional properties dynamically
                    var enrichedData = data.Select(item => new
                    {
                        item.ReportingHead,
                        item.EmployeeName,
                        item.EmployeeUserId,
                        item.MonthYear,
                        item.DetailCount,
                        TotalWorkingDays = totalWorkingDays, // Add total working days
                        TotalTimesheetsLeft = totalWorkingDays - item.DetailCount // Calculate timesheets left
                    }).ToList();

                    string directoryPath = Path.Combine(_env.WebRootPath, "timesheet", "timehr");
                    Directory.CreateDirectory(directoryPath);
                    filePath = Path.Combine(directoryPath, fileName);

                    // Pass the enriched data to the WriteToHr method
                    WriteToHr(enrichedData, filePath);

                    var fileUrl = $"/timesheet/timehr/{fileName}";

                    // Prepare the JSON response
                    var jsonResult = Json(new { success = true, fileUrl });

                    // Schedule file deletion asynchronously
                    _ = Task.Run(() => DeleteFile(filePath));

                    // Return the JSON response
                    return jsonResult;
                }


                return Json(new { success = false, msg = "Something went wrong" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }
        private int GetTotalWorkingDaysInMonth(DateTime date)
        {
            int totalDays = DateTime.DaysInMonth(date.Year, date.Month);
            int workingDays = 0;

            for (int i = 1; i <= totalDays; i++)
            {
                DateTime currentDay = new DateTime(date.Year, date.Month, i);
                if (currentDay.DayOfWeek != DayOfWeek.Saturday && currentDay.DayOfWeek != DayOfWeek.Sunday)
                {
                    workingDays++;
                }
            }

            return workingDays;
        }

    }
}

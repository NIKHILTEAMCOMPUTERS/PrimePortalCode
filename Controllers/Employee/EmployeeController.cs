using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RMS.Client.Models.Dashboard;
using RMS.Client.Models.Employee;
using RMS.Client.Models.Oaf;
using RMS.Client.Models.Timesheet;
using RMS.Client.Models.Vendor;
using RMS.Client.Utility;
using RMS.Client.ViewModels;
using System.ComponentModel;
using System.Dynamic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json.Nodes;

namespace RMS.Client.Controllers.Employee
{
    public class EmployeeController : BaseController
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly HttpClient _httpClient;
        private ApiManager apiManager;
         

        public EmployeeController(IConfiguration configuration, IWebHostEnvironment env, IWebHostEnvironment webHostEnvironment)
              : base(configuration, env)
        {
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
            _httpClient = new HttpClient();
        }
        public async Task<IActionResult> EmployeeDashboard()
        {
            ViewBag.name = Session.Name;
            int employeeid = Session.EmployeeId;
            var emp = new List<Employeedashboard>();

            apiManager = new ApiManager(_configuration["ServiceUrl"].ToString().Trim() + $"/api/Project/GetProjetsByEmployeeId/{employeeid}"); //--TOKEN
            var (statusCode, responseContent) = await apiManager.Get();

            if (statusCode == System.Net.HttpStatusCode.OK && responseContent != null)
            {
                emp = JsonConvert.DeserializeObject<List<Employeedashboard>>(responseContent);

                var employeestatus = "Bench";

                if (emp.Any())
                {
                    employeestatus = emp.Any(a => a.CategorySubStatusName.ToLower() == "deployed")
                        ? "Deployed"
                        : emp.Any(a => a.CategorySubStatusName.ToLower() == "allocation")
                            ? "Allocation"
                            : emp.Any(a => a.CategorySubStatusName.ToLower() == "shadow")
                                ? "Shadow"
                                : emp.Any(a => a.CategorySubStatusName.ToLower() == "projection")
                                    ? "Projection"
                                    : "Bench";
                }

                ViewBag.employeestatus = employeestatus;
            }

            return View(emp);
        }
        public async Task<IActionResult> Index()
        {
            return RedirectToAction("EmployeeList");
            if (!ReadPermission)
                return RedirectToAction("NotAuthorized", "Home");

            var employees = RowLevelSecurity ? await GetEmployeeByDa(Session.EmployeeId) : await GetEmployee();


            var overrunCounts = employees.Where(e => e.CategorySubStatusName.ToLower().Trim() == "over run").Count();
            var OnbenchCounts = employees.Where(e => e.CategorySubStatusName.ToLower().Trim() == "onbench").Count();
            var DeployedCount = employees.Where(e => e.CategorySubStatusName.ToLower().Trim() == "deployed").Count();
            var Freshercount = employees.Where(e => e.CategorySubStatusName.ToLower().Trim() == "Bench" && e.Experience == "Fresher").Count();
            var Expcount = employees.Where(e => e.CategorySubStatusName.ToLower().Trim() == "Bench" && e.Experience == "Experienced").Count();

            var objResponse = new HRDashboard
            {
                EmployeeCount = employees.Count(),
                EmployeeOverrun = overrunCounts,
                EmployeeOnbench = OnbenchCounts,
                EmployeeDeployed = DeployedCount,
                BenchFreshersCount = Freshercount,
                BenchExpCount = Expcount,
            };

            ViewBag.RowLevel = RowLevelSecurity;

            ViewData["controller"] = this.ControllerContext.RouteData.Values["controller"].ToString();
            return View(objResponse);
        }

        public async Task<IActionResult> EmployeeList()
        {
            if (!ReadPermission)
                return RedirectToAction("NotAuthorized", "Home");
             var employeesData = new List<DataForEmployeeReport>();

            //var employees = RowLevelSecurity ? await GetEmployeeByDa(Session.EmployeeId) : await GetEmployee();
            apiManager = RowLevelSecurity ? new ApiManager(_configuration["ServiceUrl"] + $"/api/Employee/EmployeeDetailsView/{Session.EmployeeId}", Session.Token)
                                           : new ApiManager(_configuration["ServiceUrl"] + "/api/Employee/EmployeeDetailsView/", Session.Token);
            
            var (statusCodes, responseContents) = await apiManager.Get();
            if (statusCodes==HttpStatusCode.OK)
            {
                employeesData = JsonConvert.DeserializeObject<List<DataForEmployeeReport>>(responseContents);

            }      


            //var overrunCounts = employees.Where(e => e.CategorySubStatusName.ToLower().Trim() == "over run").Count();
            var overrunCounts = employeesData.Where(e => e.Flag.ToLower().Trim() == "over run").Count();
            var OnbenchCounts = employeesData.Where(e => e.Flag.ToLower().Trim() == "on bench").Count();
            var DeployedCount = employeesData.Where(e => e.Flag.ToLower().Trim() == "deployed").Count();
            var Freshercount = employeesData.Where(e => e.Flag.ToLower().Trim() == "on bench" && e.Exe?.ToLower().Trim() == "fresher").Count();
            var Expcount = employeesData.Where(e => e.Flag.ToLower().Trim() == "on bench" && e.Exe?.ToLower().Trim() == "experienced").Count();

            var objResponse = new HRDashboard
            {
                EmployeeCount = employeesData.Count(),
                EmployeeOverrun = overrunCounts,
                EmployeeOnbench = OnbenchCounts,
                EmployeeDeployed = DeployedCount,
                BenchFreshersCount = Freshercount,
                BenchExpCount = Expcount,
            };

            ViewBag.RowLevel = RowLevelSecurity;

            ViewData["controller"] = this.ControllerContext.RouteData.Values["controller"].ToString();
            return View(objResponse);
        }
        [HttpPost]
        public async Task<IActionResult> EmployeeDataMethod(int mode, bool all = false)
        {
            var employeesData = new List<DataForEmployeeReport>();
            // apiManager = new ApiManager(_configuration["ServiceUrl"] + "/api/Employee/EmployeeDetailsView/", Session.Token);
            if (all)
                apiManager = new ApiManager(_configuration["ServiceUrl"] + $"/api/Employee/EmployeeDetailsView/", Session.Token);
            else
            {
                apiManager = RowLevelSecurity ? new ApiManager(_configuration["ServiceUrl"] + $"/api/Employee/EmployeeDetailsView/{Session.EmployeeId}", Session.Token)
                                                : new ApiManager(_configuration["ServiceUrl"] + "/api/Employee/EmployeeDetailsView/", Session.Token);
            }
            var (statusCodes, responseContents) = await apiManager.Get();
            if (statusCodes == HttpStatusCode.OK)
            {
                employeesData = JsonConvert.DeserializeObject<List<DataForEmployeeReport>>(responseContents);

            }
            if (mode==1)
            {
                return Json(new
                {
                    Employees = employeesData,
                    Overrun = employeesData.Where(e => e.Flag.ToLower().Trim() == "over run").Count(),
                    OnBench = employeesData.Where(e => e.Flag.ToLower().Trim() == "on bench").Count(),
                    Deployed = employeesData.Where(e => e.Flag.ToLower().Trim() == "deployed").Count(),
                    Total = employeesData.Count(),
                    benchFreshersCount = employeesData.Where(e => e.Flag.ToLower().Trim() == "on bench" && e.Exe?.ToLower().Trim() == "fresher").Count(),
                    benchExpCount = employeesData.Where(e => e.Flag.ToLower().Trim() == "on bench" && e.Exe?.ToLower().Trim() == "experienced").Count(),
                });
            }
           else if (mode == 2)
            {
                return Json(new
                {
                    Employees = employeesData.Where(e => e.Flag.ToLower().Trim() == "over run").ToList(),
                    Overrun = employeesData.Where(e => e.Flag.ToLower().Trim() == "over run").Count(),
                    OnBench = employeesData.Where(e => e.Flag.ToLower().Trim() == "on bench").Count(),
                    Deployed = employeesData.Where(e => e.Flag.ToLower().Trim() == "deployed").Count(),
                    Total = employeesData.Count(),
                    benchFreshersCount = employeesData.Where(e => e.Flag.ToLower().Trim() == "on bench" && e.Exe?.ToLower().Trim() == "fresher").Count(),
                    benchExpCount = employeesData.Where(e => e.Flag.ToLower().Trim() == "on bench" && e.Exe?.ToLower().Trim() == "experienced").Count(),
                });
            }
            if (mode == 3)
            {
                return Json(new
                {
                    Employees = employeesData.Where(e => e.Flag.ToLower().Trim() == "on bench").ToList(),
                    Overrun = employeesData.Where(e => e.Flag.ToLower().Trim() == "over run").Count(),
                    OnBench = employeesData.Where(e => e.Flag.ToLower().Trim() == "on bench").Count(),
                    Deployed = employeesData.Where(e => e.Flag.ToLower().Trim() == "deployed").Count(),
                    Total = employeesData.Count(),
                    benchFreshersCount = employeesData.Where(e => e.Flag.ToLower().Trim() == "on bench" && e.Exe?.ToLower().Trim() == "fresher").Count(),
                    benchExpCount = employeesData.Where(e => e.Flag.ToLower().Trim() == "on bench" && e.Exe?.ToLower().Trim() == "experienced").Count(),
                });
            }
            else if (mode == 4)
            {
                return Json(new
                {
                    Employees = employeesData.Where(e => e.Flag.ToLower().Trim() == "deployed").ToList(),
                    Overrun = employeesData.Where(e => e.Flag.ToLower().Trim() == "over run").Count(),
                    OnBench = employeesData.Where(e => e.Flag.ToLower().Trim() == "on bench").Count(),
                    Deployed = employeesData.Where(e => e.Flag.ToLower().Trim() == "deployed").Count(),
                    Total = employeesData.Count(),
                    benchFreshersCount = employeesData.Where(e => e.Flag.ToLower().Trim() == "on bench" && e.Exe?.ToLower().Trim() == "fresher").Count(),
                    benchExpCount = employeesData.Where(e => e.Flag.ToLower().Trim() == "on bench" && e.Exe?.ToLower().Trim() == "experienced").Count(),
                });
            }
            else
            {
                return Json(new
                {
                    Employees = employeesData,
                    Overrun = employeesData.Where(e => e.Flag.ToLower().Trim() == "over run").Count(),
                    OnBench = employeesData.Where(e => e.Flag.ToLower().Trim() == "on bench").Count(),
                    Deployed = employeesData.Where(e => e.Flag.ToLower().Trim() == "deployed").Count(),
                    Total = employeesData.Count(),
                    benchFreshersCount = employeesData.Where(e => e.Flag.ToLower().Trim() == "on bench" && e.Exe?.ToLower().Trim() == "fresher").Count(),
                    benchExpCount = employeesData.Where(e => e.Flag.ToLower().Trim() == "on bench" && e.Exe?.ToLower().Trim() == "experienced").Count(),
                });
            }


        }

        public async Task<IActionResult> Update(int id)
        {
            if (!WritePermission)
                return RedirectToAction("NotAuthorized", "Home");

            var emp = await GetEmployee(id);
            var projList = await GetProjects();

            ViewBag.ProjectList = projList.Select(c => new
            {
                Value = c.ProjectId.ToString(),
                Text = c.ProjectName,
                ContractNo = c.ContractNo // Add ContractNo to SelectListItem
            }).ToList();


            var catsubList = await GetCategorySubStatus();
            ViewBag.SubCategoryStatusList = catsubList;

            var objModel = new ProjectAssign
            {
                employeeid = id,
                employees = emp
            };
            ViewBag.skilllist = await GetSkillList();
            ViewData["controller"] = this.ControllerContext.RouteData.Values["controller"].ToString();
            return View(objModel);
        }

        [HttpPost]
        public async Task<IActionResult> Update(string jobject)
        {
            if (!WritePermission)
                return RedirectToAction("NotAuthorized", "Home");

            if (string.IsNullOrEmpty(jobject))
                return View();

            try
            {
                var oModel = JsonConvert.DeserializeObject<ProjectAssign>(jobject);
                string jsonBody = JsonConvert.SerializeObject(oModel);

                apiManager = new ApiManager(_configuration["ServiceUrl"] + "/api/ProjectAssignment/", Session.Token);
                var (statusCodes, responseContents) = await apiManager.PostJson(jsonBody);

                if (statusCodes == HttpStatusCode.OK)
                {
                    ViewData["success"] = "Employee Updated Successfully !!";
                    return Json(new { data = responseContents });
                }
                else
                {
                    ViewData["error"] = "Something Went Wrong";
                    return Json(new { data = responseContents });
                }
            }
            catch (Exception ex)
            {
                // Log or handle exceptions as needed.
                ViewData["error"] = "An error occurred: " + ex.Message;

                // Return a JSON response with the error message
                return Json(new { error = ex.Message });
            }

            ViewData["controller"] = this.ControllerContext.RouteData.Values["controller"].ToString();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AssignEmployees(string jobject)
        {
            if (!WritePermission)
                return RedirectToAction("NotAuthorized", "Home");

            if (string.IsNullOrEmpty(jobject))
                return View();

            try
            {
                var oModel = JsonConvert.DeserializeObject<EmployeeAssignDto>(jobject);
                string jsonBody = JsonConvert.SerializeObject(oModel);

                apiManager = new ApiManager(_configuration["ServiceUrl"] + "/api/ProjectAssignment/AssignEmployeeProject", Session.Token);
                var (statusCodes, responseContents) = await apiManager.PostJson(jsonBody);

                if (statusCodes == HttpStatusCode.OK)
                {

                    return Json(new { data = responseContents });
                }
                else
                {

                    return Json(new { data = responseContents });
                }
            }
            catch (Exception ex)
            {
                // Log or handle exceptions as needed.
                ViewData["error"] = "An error occurred: " + ex.Message;

                // Return a JSON response with the error message
                return Json(new { error = ex.Message });
            }

            ViewData["controller"] = this.ControllerContext.RouteData.Values["controller"].ToString();
            return View();
        }

        public async Task<JsonResult> GetProjectInfo(int id)
        {
            return Json(await GetProjects(id));
        }

        [HttpPost]
        public async Task<IActionResult> EmployeeResign(string empid, int isresigned)
        {
            if (!WritePermission)
                return RedirectToAction("NotAuthorized", "Home");

            try
            {
                int resign = 0;
                if (isresigned != 0)
                    resign = 1;
                else
                    resign = isresigned;
                int employeeId = Convert.ToInt32(empid);
                Employees employee = new Employees
                {
                    Employeeid = employeeId,
                    Udf3 = resign.ToString(), // 1 for mark as resign 2 for exit from company
                };

                apiManager = new ApiManager(_configuration["ServiceUrl"] + "/api/Employee/ResignationUpdateAPI/", Session.Token);
                IFormFileCollection images = HttpContext.Request.Form.Files;
                // Assuming PostJson returns a tuple (HttpStatusCode, string) or similar structure.
                var apiResult = await apiManager.PostWithFiles(System.Text.Json.JsonSerializer.Serialize(employee), images);

                HttpStatusCode statusCodes = apiResult.Item1;
                string responseContents = apiResult.Item2;

                if (statusCodes == HttpStatusCode.OK)
                {
                    return Json(new { data = responseContents });
                }
                else
                {
                    return Json(new { data = responseContents });
                }
            }
            catch (Exception ex)
            {
                // Log or handle exceptions as needed.
                ViewData["error"] = "An error occurred: " + ex.Message;

                // Return a JSON response with the error message
                return Json(new { error = ex.Message });
            }
        }
        [HttpPost]
        public async Task<IActionResult> DeleteEmployeeProject(string jobject)
        {
            if (!WritePermission)
                return RedirectToAction("NotAuthorized", "Home");

            if (string.IsNullOrEmpty(jobject))
                return View();

            try
            {
                var oModel = JsonConvert.DeserializeObject<ProjectAssign>(jobject);
                string jsonBody = JsonConvert.SerializeObject(oModel);

                apiManager = new ApiManager(_configuration["ServiceUrl"] + "/api/ProjectAssignment/DeleteEmployeeProject/", Session.Token);
                var (statusCodes, responseContents) = await apiManager.PostJson(jsonBody);

                if (statusCodes == HttpStatusCode.OK)
                {
                    ViewData["success"] = "Employee Project Deleted Successfully !!";
                    return Json(new { data = responseContents });
                }
                else
                {
                    ViewData["error"] = "Something Went Wrong";
                    return Json(new { data = responseContents });
                }
            }
            catch (Exception ex)
            {
                // Log or handle exceptions as needed.
                ViewData["error"] = "An error occurred: " + ex.Message;

                // Return a JSON response with the error message
                return Json(new { error = ex.Message });
            }

        }

        [HttpPost]
        public async Task<IActionResult> Changeemployeeprojectstatus(string jobject)
        {
            if (!WritePermission)
                return RedirectToAction("NotAuthorized", "Home");

            if (string.IsNullOrEmpty(jobject))
                return View();

            try
            {
                var oModel = JsonConvert.DeserializeObject<ProjectAssign>(jobject);
                string jsonBody = JsonConvert.SerializeObject(oModel);

                apiManager = new ApiManager(_configuration["ServiceUrl"] + "/api/ProjectAssignment/Changeemployeeprojectstatus/", Session.Token);
                var (statusCodes, responseContents) = await apiManager.PostJson(jsonBody);

                if (statusCodes == HttpStatusCode.OK)
                {
                    ViewData["success"] = "Employee Project Deleted Successfully !!";
                    return Json(new { data = responseContents });
                }
                else
                {
                    ViewData["error"] = "Something Went Wrong";
                    return Json(new { data = responseContents });
                }
            }
            catch (Exception ex)
            {
                // Log or handle exceptions as needed.
                ViewData["error"] = "An error occurred: " + ex.Message;

                // Return a JSON response with the error message
                return Json(new { error = ex.Message });
            }

        }
        //   [HttpPost]
        //public async Task<IActionResult> Employeetraineebatch(string empid, bool isade)
        //{
        //    if (!WritePermission)
        //        return RedirectToAction("NotAuthorized", "Home");

        //    try
        //    {
        //        bool traineemark = false;
        //        if (isade != false)
        //            traineemark = true;
        //        else
        //            traineemark = isade;
        //        int employeeId = Convert.ToInt32(empid);
        //        Employees employee = new Employees
        //        {
        //            Employeeid = employeeId,
        //            isAde = traineemark,
        //        };

        //        apiManager = new ApiManager(_configuration["ServiceUrl"] + "/api/Employee/AdeUpdation/", Session.Token);
        //        // Assuming PostJson returns a tuple (HttpStatusCode, string) or similar structure.
        //        var apiResult = await apiManager.PostJson(System.Text.Json.JsonSerializer.Serialize(employee));

        //        HttpStatusCode statusCodes = apiResult.Item1;
        //        string responseContents = apiResult.Item2;

        //        if (statusCodes == HttpStatusCode.OK)
        //        {
        //            return Json(new { data = responseContents });
        //        }
        //        else
        //        {
        //            return Json(new { data = responseContents });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log or handle exceptions as needed.
        //        ViewData["error"] = "An error occurred: " + ex.Message;

        //        // Return a JSON response with the error message
        //        return Json(new { error = ex.Message });
        //    }
        //}

        public async Task<IActionResult> Employeetraineebatch(string empid, bool isade)
        {
            if (!WritePermission)
                return RedirectToAction("NotAuthorized", "Home");

            try
            {
                bool isAde = isade;
                if (!int.TryParse(empid, out int employeeId))
                    return Json(new { error = "Invalid employee ID" });

                var employee = new { EmployeeId = employeeId, IsAde = isAde };

                var serializedEmployee = System.Text.Json.JsonSerializer.Serialize(employee);

                apiManager = new ApiManager(_configuration["ServiceUrl"] + "/api/Employee/AdeUpdation/", Session.Token);
                var apiResult = await apiManager.PostJson(serializedEmployee);

                HttpStatusCode statusCodes = apiResult.Item1;
                string responseContents = apiResult.Item2;

                if (statusCodes == HttpStatusCode.OK)
                {
                    return Json(new { data = responseContents });
                }
                else
                {
                    return Json(new { data = responseContents });
                }
            }
            catch (Exception ex)
            {
                return Json(new { error = "An error occurred: " + ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateEmployeePracticeSubPractice(string jobject)
        {

            if (string.IsNullOrEmpty(jobject))
                return Json(new { error = "Something Went Wrong!" });

            try
            {
                var oModel = JsonConvert.DeserializeObject<RequestSubpracticeUpdationDto>(jobject);
                string jsonBody = JsonConvert.SerializeObject(oModel);

                apiManager = new ApiManager(_configuration["ServiceUrl"] + "/api/Employee/UpdateEmployeeSubpractice", Session.Token);
                var (statusCodes, responseContents) = await apiManager.PostJson(jsonBody);

                if (statusCodes == HttpStatusCode.OK)
                {

                    return Json(new { data = responseContents });
                }
                else
                {

                    return Json(new { data = responseContents });
                }
            }
            catch (Exception ex)
            {
                // Log or handle exceptions as needed.
                ViewData["error"] = "An error occurred: " + ex.Message;

                // Return a JSON response with the error message
                return Json(new { error = ex.Message });
            }

            ViewData["controller"] = this.ControllerContext.RouteData.Values["controller"].ToString();
            return View();
        }

        public async Task<IActionResult> GetAllBillingDataResourceWise()
        {
            string serviceUrl = _configuration["ServiceUrl"].ToString().Trim() + "/api/Employee/GetAllBillingDataReourceWise";
            var response = await _httpClient.GetAsync(serviceUrl);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                if (responseContent != null)
                {
                    var billingData = JsonConvert.DeserializeObject<List<ExpandoObject>>(responseContent);

                    // Generate CSV file
                    string fileName = "ResourceReport_" + Guid.NewGuid() + ".csv";
                    string directoryPath = Path.Combine(_webHostEnvironment.WebRootPath, "temp", "tempcsv");
                    Directory.CreateDirectory(directoryPath);

                    string filePath = Path.Combine(directoryPath, fileName);
                    WriteToCSV(billingData, filePath);

                    byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
                    return File(fileBytes, "application/octet-stream", fileName);
                }
            }

            return StatusCode((int)response.StatusCode);
        }

        private static void WriteToCSV(IEnumerable<ExpandoObject> list, string filePath)
        {
            var csv = new StringBuilder();

            var firstItem = list.FirstOrDefault();
            if (firstItem != null)
            {
                var headers = ((IDictionary<string, object>)firstItem).Keys;
                csv.AppendLine(string.Join(",", headers));

               
                foreach (var item in list)
                {
                    var values = ((IDictionary<string, object>)item).Values;
                    var line = string.Join(",", values.Select(value => value != null ? value.ToString() : string.Empty));
                    csv.AppendLine(line);
                }
            }

            
            System.IO.File.WriteAllText(filePath, csv.ToString());
        }


        [HttpPost]
        public async Task<IActionResult> UpdateSkill(string jobject)
        {
          
            if (string.IsNullOrEmpty(jobject))
                return RedirectToAction("Update","Employee");

            try
            {
                //var oModel = JsonConvert.DeserializeObject<EmployeeSkill>(jobject);
                //string jsonBody = JsonConvert.SerializeObject(oModel);

                apiManager = new ApiManager(_configuration["ServiceUrl"] + "/api/Skill/AddEmployeeSkills", Session.Token);
                var (statusCodes, responseContents) = await apiManager.PostJson(jobject);

                if (statusCodes == HttpStatusCode.OK)
                {

                    return Json(new { data = responseContents, success = true });
                }
                else
                {

                    return Json(new { data = responseContents, success = false });
                }
            }
            catch (Exception ex)
            {
                // Log or handle exceptions as needed.
                ViewData["error"] = "An error occurred: " + ex.Message;

                // Return a JSON response with the error message
                return Json(new { error = ex.Message });
            }

            ViewData["controller"] = this.ControllerContext.RouteData.Values["controller"].ToString();
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> DeleteEmployeeSkill(int employeeskillid)
        {
            //if (!WritePermission)
            //    return RedirectToAction("NotAuthorized", "Home");

            try
            {
                apiManager = new ApiManager(_configuration["ServiceUrl"] + "/api/Skill/DeleteEmployeeSkill/" + employeeskillid, Session.Token);
                var apiResult = await apiManager.Delete();

                HttpStatusCode statusCode = apiResult.Item1;
                string responseContents = apiResult.Item2;
                return Json(new { data = responseContents, status = statusCode });
            }
            catch (Exception ex)
            {
                return Json(new { error = "An error occurred while processing your request." });
            }
        }
        public async Task<IActionResult> Profile()
        {
            
            var emp = new Profile();

            apiManager = new ApiManager(_configuration["ServiceUrl"].ToString().Trim() + $"/api/Employee/GetProfile",Session.Token); //--TOKEN
            var (statusCode, responseContent) = await apiManager.Get();

            if (statusCode == System.Net.HttpStatusCode.OK && responseContent != null)
            {
                emp = JsonConvert.DeserializeObject<Profile>(responseContent);

            }
            ViewBag.skilllist = await GetSkillList();
            ViewBag.Employeeid = Session.EmployeeId;
            return View(emp);
        }
        [HttpGet]
        public async Task<IActionResult> emplist()
        {
            var emp = new List<emplist>();

            apiManager = new ApiManager(_configuration["ServiceUrl"].ToString().Trim() + "/api/Employee/EmployeeDetailsView"); //--TOKEN
            var (statusCode, responseContent) = await apiManager.Get();

            if (statusCode == System.Net.HttpStatusCode.OK)
            {
                if (responseContent != null)
                {
                    emp = JsonConvert.DeserializeObject<List<emplist>>(responseContent);

                }
            }
            return View(emp);
        }
        public async Task<JsonResult> GetBenchResources()
        {
            try
            {
                // Call EmployeeDataMethod with mode 3 to get "on bench" employees
                var result = await EmployeeDataMethod(3, false);

                // Extract JSON result as a string
                var jsonResult = result as JsonResult;
                if (jsonResult?.Value == null)
                {
                    return Json(new { error = "No data found" });
                }

                // Convert the JSON result back to an object
                var jsonString = JsonConvert.SerializeObject(jsonResult.Value);
                var response = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonString);

                // Deserialize only the Employees part as a list of emplist objects
                if (response.ContainsKey("Employees") && response["Employees"] is Newtonsoft.Json.Linq.JArray employeesArray)
                {
                    var benchResources = employeesArray.ToObject<List<emplist>>();

                    if (benchResources != null && benchResources.Count > 0)
                    {
                        // Format data for dropdown with EmployeeId and ResourceName
                        return Json(benchResources.Select(br => new
                        {
                            Value = br.Employeeid,
                            Text = $"{br.Resourcename} ({br.Userid})"
                        }));

                    }
                }

                return Json(new { error = "No data found" });
            }
            catch (Exception ex)
            {
                // Log exception here if needed
                return Json(new { error = "Failed to fetch data", details = ex.Message });
            }
        }


    }
}

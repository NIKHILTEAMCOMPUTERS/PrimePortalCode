using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RMS.Client.Models;
using RMS.Client.Models.Customer;
using RMS.Client.Models.Employee;
using RMS.Client.Models.Master;
using RMS.Client.Models.Oaf;
using RMS.Client.Models.Projection;
using RMS.Client.Utility;
using System;
using System.Text;
using System.Text.Json.Serialization;

namespace RMS.Client.Controllers.Customer
{
    public class ProjectProjectionController : BaseController
    {
        private ApiManager apiManager;
        private IConfiguration _configuration;
        public ProjectProjectionController(IConfiguration configuration, IWebHostEnvironment env)
              : base(configuration, env)
        {
            _configuration = configuration;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // Retrieve data from the first API (CRM Deals)
            ViewBag.customerList = await GetCustomer();
            ViewBag.skilllist = await GetSkillList();
            ViewBag.resource = await GetEmployeedropdown();
            ViewBag.Practices = await GetPractice();
            ViewBag.Subpractices = await GetSubPractice();
            ViewBag.Currency = GetCurrency().Result;
            ViewBag.CountryList = await GetCountry();
            ViewBag.PaymentTermList = await GetPaymentTerms();
            ViewBag.TodayDate = DateTime.Now.ToString("yyyy-MM-dd");
            ViewBag.YearGapDate = DateTime.Now.AddYears(-1).ToString("yyyy-MM-dd");
            ViewBag.ProjectTypeList = await GetProjecType();
            ViewBag.costsheet = await GetExistingCostingSheet();

            // Fetch data from CRM API
            var apiResult = new List<CrmDeal>();
            apiManager = new ApiManager(_configuration["ServiceUrl"].ToString().Trim() + "/api/City/GetAllCRMdata");
            var (statusCode, responseContent) = await apiManager.Get();

            if (statusCode == System.Net.HttpStatusCode.OK && !string.IsNullOrEmpty(responseContent))
            {
                if (Session.RoleName != "HR" || Session.RoleName != "DA") {
                    apiResult = JsonConvert.DeserializeObject<List<CrmDeal>>(responseContent)
                        .OrderBy(deal => deal.CreatedAt)
                        .ToList(); }
                else
                {
                    apiResult = JsonConvert.DeserializeObject<List<CrmDeal>>(responseContent)
                        .Where(a => a.Projectionid !=null)
                        .OrderBy(deal => deal.CreatedAt)
                        .ToList();
                }
            }

            
            return View(apiResult);
        }



        [HttpGet]
        public async Task<IActionResult> GetListofCustomers()
        {
            var listOfCustomers = new List<customer>();
            apiManager = new ApiManager(_configuration["ServiceUrl"].ToString().Trim() + "/api/Customer"); 
            var (statusCode, responseContent) = await apiManager.Get();

            if (statusCode == System.Net.HttpStatusCode.OK)
            {
                listOfCustomers = JsonConvert.DeserializeObject<List<customer>>(responseContent);
            }

            return Json(listOfCustomers); 
        }
        [HttpGet]
        public async Task<IActionResult> GetListofSkillsWithExp()
        {
            var Skill= new List<SkillData>();
            apiManager = new ApiManager(_configuration["ServiceUrl"].ToString().Trim() + "/api/Projection/GetSkillNameExe");
            var (statusCode, responseContent) = await apiManager.Get();

            if (statusCode == System.Net.HttpStatusCode.OK)
            {
                if (responseContent != null)
                {
                    var apiResult = JsonConvert.DeserializeObject<ModelSkillExp>(responseContent);
                    Skill = apiResult.Data;
                }
            }
            return Json(Skill);
        }
        [HttpGet]
        public async Task<IActionResult> GetListOfResources(int skillId, string experience)
        {
            var listofReources = new List<EmployeeExpData>();
            var objPram = new
            {
                skillId = skillId,
                Experince = experience

            };
            var httpContent = new StringContent(JsonConvert.SerializeObject(objPram), Encoding.UTF8, "application/json");
          
            apiManager = new ApiManager(_configuration["ServiceUrl"].ToString().Trim() + "/api/Projection/GetEmpBySkillExe");
            var (statusCode, responseContent) = await apiManager.Get(httpContent);

            if (statusCode == System.Net.HttpStatusCode.OK)
            {
                if (responseContent != null)
                {
                    var apiResult = JsonConvert.DeserializeObject<ModelEmpExpList>(responseContent);
                    listofReources = apiResult.Data;
                }
            }
            return Json(listofReources);
        }

        [HttpGet]
        public async Task<IActionResult> GetListOfProjectDetilas(int employeeid)
        {
            var listofReources = new List<EmployeeProjectDetails>();            

            apiManager = new ApiManager(_configuration["ServiceUrl"].ToString().Trim() + "/api/Projection/GetEmployeeProjectDetails/"+ employeeid+"");
            var (statusCode, responseContent) = await apiManager.Get();

            if (statusCode == System.Net.HttpStatusCode.OK)
            {
                if (responseContent != null)
                {
                    var apiResult = JsonConvert.DeserializeObject<ModelEmployeeProjectDetail>(responseContent);
                    listofReources = apiResult.Data;
                }
            }
            return Json(listofReources);
        }
        [HttpPost]
        public async Task<IActionResult> PostProjection(string data)
        {
            try
            {
                var apiUrl = $"{_configuration["ServiceUrl"].ToString().Trim()}/api/Projection/";
                apiManager = new ApiManager(apiUrl, Session.Token);

                var (statusCode, responseContent) = await apiManager.PostJson(data);

                return statusCode == System.Net.HttpStatusCode.OK
                    ? Json(new { statusCode, responseContent })
                    : Json(new { statusCode });
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetProjectionList()
        {

            try
            {
                var projection = new List<Projection>();

                apiManager = new ApiManager(_configuration["ServiceUrl"].ToString().Trim() + "/api/Projection");
                var (statusCode, responseContent) = await apiManager.Get();

                if (statusCode == System.Net.HttpStatusCode.OK && responseContent != null)
                {
                    var apiResponse = JsonConvert.DeserializeObject<ResponseModelProjection>(responseContent);

                    if (apiResponse != null && apiResponse.Data != null)
                    {
                        return View(apiResponse.Data);
                    }
                }
                return View(new List<Projection>());

            }
            catch (Exception ex)
            {
                return View(new List<Projection>());
            }
        }



        [HttpPost]
        public async Task<JsonResult> CreateCustomer(Customers customer)
        {
            if (Session.RoleName != "DA")
            {
                if (!WritePermission)
                    return Json(new { success = false, msg = "Not Authorized" });
            }

            try
            {
                if (customer == null)
                {
                    ViewData["error"] = "Customer data is empty.";
                    return Json(new { success = false, msg = "Not Found" });
                }

                customer.Customertypeid = 1;
                IFormFileCollection images = HttpContext.Request.Form.Files;
                var baseApiUrl = _configuration["ServiceUrl"].ToString().Trim();
                var apiURL = (customer.Customerid != 0) ? $"{baseApiUrl}/api/Customer/{customer.Customerid}" : $"{baseApiUrl}/api/Customer";

                apiManager = new ApiManager(apiURL, Session.Token);

                System.Net.HttpStatusCode statusCode;
                string responseContent;

                if (customer.Customerid != 0)
                {
                    (statusCode, responseContent) = await apiManager.PutWithFiles(System.Text.Json.JsonSerializer.Serialize(customer), images);
                }
                else
                {
                    (statusCode, responseContent) = await apiManager.PostWithFiles(System.Text.Json.JsonSerializer.Serialize(customer), images);

                    return Json(new { success = true, msg = "Customer created Successfully", data = responseContent });

                }


            }
            catch (Exception ex)
            {
                return Json(new { success = false, msg = "An Error occured" });

            }

            return Json(new { success = true, msg = "Something wrong happen" });
        }

        [HttpGet]
        public async Task<IActionResult> DetailProjection(int Id)
        {
            //if (!ReadPermission)
            //    return RedirectToAction("NotAuthorized", "Home");
            ViewBag.skilllist = await GetSkillList();
            ViewBag.YearGapDate = DateTime.Now.AddYears(-1).ToString("yyyy-MM-dd");

            ViewData["controller"] = this.ControllerContext.RouteData.Values["controller"].ToString();
            Projection projection = null;
            var msg = "";
            var viewModel = new ProjectionViewModel
            {
                Projections = new Projection()
            };
            try
            {
                if (Id != 0)
                {
                    apiManager = new ApiManager(_configuration["ServiceUrl"].ToString().Trim() + "/api/Projection/" + Id);
                    var (statusCode, responseContent) = await apiManager.Get();

                    if (statusCode == System.Net.HttpStatusCode.OK)
                    {
                        var jsonResponse = JsonConvert.DeserializeObject<dynamic>(responseContent);
                        var data = jsonResponse.data.ToString();

                        // Deserialize the 'data' into Projection object
                        // projection = JsonConvert.DeserializeObject<Projection>(data);
                        viewModel.Projections = JsonConvert.DeserializeObject<Projection>(data);
                        viewModel.Projections.Costsheetdata = await GetCostSheet((int)viewModel.Projections.costsheetId);

                    }
                    else
                    {
                        msg = responseContent.ToString();
                        ViewData["error"] = $"An unexpected error occurred: {msg}";
                    }
                }
            }
            catch (Exception ex)
            {

                ViewData["error"] = $"An unexpected error occurred: {ex.Message}";
            }
            return View(viewModel);
        }

        [HttpPost]
        public async Task<JsonResult> ActiveEmployeeProject(string data)
        {
            try
            {
                if (string.IsNullOrEmpty(data))
                {
                    ViewData["error"] = "Data is empty.";
                    return Json(new { success = false, msg = "No data provided" });
                }

                string endpointUrl = _configuration["ServiceUrl"].ToString().Trim() + "/api/Project/CheckEmployeeActiveProjects";
                apiManager = new ApiManager(endpointUrl);

                var (statuscode, responseContent) = await apiManager.PostJson(data);

                if (statuscode == System.Net.HttpStatusCode.OK)
                {
                    if (!string.IsNullOrEmpty(responseContent))
                    {
                        dynamic apiResponse = JsonConvert.DeserializeObject<dynamic>(responseContent);

                        var projectList = new List<object>();

                        foreach (var item in apiResponse)
                        {
                            var project = new
                            {
                                projectName = (string)item.projectName,
                                contractStartDate = ((DateTime)item.contractStartDate).ToString("yyyy-MM-dd"),
                                contractEndDate = ((DateTime)item.contractEndDate).ToString("yyyy-MM-dd"),
                                deliveryAnchorId = (string)item.deliveryAnchorId,
                                deliveryAnchorName = (string)item.deliveryAnchorName,
                                employeeStatus = (string)item.employeeStatus
                            };

                            projectList.Add(project);
                        }

                        return Json(new { success = true, msg = "Employee Active Project List", data = projectList });
                    }

                    return Json(new { success = true, msg = "No active projects found", data = new List<object>() });
                }

                return Json(new { success = false, msg = $"Failed to retrieve active projects. Status code: {statuscode}" });
            }
            catch (Exception ex)
            {
                ViewData["error"] = $"An unexpected error occurred: {ex.Message}";
                return Json(new { success = false, msg = "An error occurred", error = ex.Message });
            }
        }



        [HttpPost]
        public async Task<JsonResult> EmployeeReleaseRequest(string data)
        {
            try
            {
                if (string.IsNullOrEmpty(data))
                {
                    ViewData["error"] = "Data is empty.";
                    return Json(new { success = false, msg = "No data provided" });
                }

                string endpointUrl = _configuration["ServiceUrl"].ToString().Trim() + "/api/Project/EmployeeReleaseRequest";
                apiManager = new ApiManager(endpointUrl, Session.Token);

                var (statuscode, responseContent) = await apiManager.PostJson(data);

                if (statuscode == System.Net.HttpStatusCode.OK)
                {
                    if (!string.IsNullOrEmpty(responseContent))
                    {
                        dynamic apiResponse = JsonConvert.DeserializeObject<dynamic>(responseContent);

                        //var projectList = new List<object>();

                        //foreach (var item in apiResponse)
                        //{
                        //    var project = new
                        //    {
                        //        Requestsentby = (string)item.Requestsentby,
                        //        Requestsentto = (string)item.Requestsentby,
                        //        Status = (string)item.Status

                        //    };

                        //    projectList.Add(project);
                        //}

                        return Json(new { success = true, msg = "Request has been sent", data = apiResponse });
                    }

                    return Json(new { success = true, msg = "No active projects found", data = new List<object>() });
                }

                return Json(new { success = false, msg = $"Failed to retrieve active projects. Status code: {statuscode}" });
            }
            catch (Exception ex)
            {
                ViewData["error"] = $"An unexpected error occurred: {ex.Message}";
                return Json(new { success = false, msg = "An error occurred", error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ProjectionRequest()
        {
            try
            {
                apiManager = new ApiManager(_configuration["ServiceUrl"].ToString().Trim() + "/api/Projection/ProjectionRequestList/" + Session.EmployeeId);

                var (statusCode, responseContent) = await apiManager.Get();

                if (statusCode == System.Net.HttpStatusCode.OK && responseContent != null)
                {
                    List<Projectionrequest> projectionRequests = JsonConvert.DeserializeObject<List<Projectionrequest>>(responseContent);

                    if (projectionRequests != null)
                    {
                        return View(projectionRequests.Where(_ => _.Status.ToLower() == "pending"));
                    }
                }
                return View(new List<Projectionrequest>());
            }
            catch (Exception ex)
            {
                return View(new List<Projectionrequest>());
            }
        }
    }
}

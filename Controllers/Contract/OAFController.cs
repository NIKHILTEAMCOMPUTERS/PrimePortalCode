using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using RMS.Client.Models.Contract;
using RMS.Client.Models.Master;
using RMS.Client.Models.Oaf;
using RMS.Client.Utility;
using RMS.Client.ViewModels;
using System;
using System.Net;
using System.Reflection;

namespace RMS.Client.Controllers.Contract
{
    public class OAFController : BaseController
    {
        private readonly IWebHostEnvironment _env;
        private ApiManager apiManager;
        private IConfiguration _configuration;
        private IsoDateTimeConverter dateTimeConverter;
        public OAFController(IConfiguration configuration, IWebHostEnvironment env, IWebHostEnvironment environment)
              : base(configuration, env)
        {
            _configuration = configuration;
            _env = environment ?? throw new ArgumentNullException(nameof(environment));
            dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" };
        }
        [HttpGet]
        public async Task<IActionResult> Index(string status = "Total")
        
        {

            //if (Session.RoleName == "DH")
            //{
            //    return RedirectToAction("OAFList", "OAF");
            //}

            int DAId = Session.EmployeeId;
            // Filter data based on the "status" parameter if provided
            if (string.IsNullOrEmpty(status))
                status = "Total";

            ViewData["controller"] = this.ControllerContext.RouteData.Values["controller"].ToString();
            var data = new List<Oaf>();


            if (Session.RoleName == "DA")
            {
                
                string endpointUrl = (_configuration["ServiceUrl"].ToString().Trim() + "/api/Oaf/GetOafListByDAId/" + DAId); //--TOKEN
                apiManager = new ApiManager(endpointUrl, Session.Token);

            }
            else
            {
                string url = (_configuration["ServiceUrl"].ToString().Trim() + "/api/Oaf"); //--TOKEN
                apiManager = new ApiManager(url, Session.Token);
            }
            var (statusCode, responseContent) = await apiManager.Get();

            if (statusCode == System.Net.HttpStatusCode.OK)
            {
                data = JsonConvert.DeserializeObject<List<Oaf>>(responseContent, dateTimeConverter);

                ViewBag.totalOAFCount = data != null ? data.Count() : 0;
                ViewBag.approvedOAFCount = data != null ? data.Count(o => o.status == "Approved") : 0;
                ViewBag.pendingOAFCount = data != null ? data.Count(o => o.status == "Pending") : 0;
                ViewBag.rejectedOAFCount = data != null ? data.Count(o => o.status == "Rejected") : 0;
                ViewBag.extendOAFCount = data != null ? data.Count(o => o.isextended == true) : 0;
                // Filter data based on the "status" parameter if provided
                data = FilterDataByStatus(data, status);
            }

            ViewBag.status = status;
            return View(data.OrderByDescending(c => c.oafid));
        }

        //[HttpGet]
        //public async Task<IActionResult> Upsert(int id)
        //{
        //    ViewData["controller"] = this.ControllerContext.RouteData.Values["controller"].ToString();
        //    var apiManager = new ApiManager($"{_configuration["ServiceUrl"]}/api/Oaf/{id}");

        //    var (statusCode, responseContent) = await apiManager.Get();
        //    Oaf data = new Oaf();
        //    if (statusCode == System.Net.HttpStatusCode.OK)
        //    {
        //        data = JsonConvert.DeserializeObject<Oaf>(responseContent, dateTimeConverter);
        //        ViewBag.costsheet = await GetCostSheet(data.costsheetid);
        //    }

        //    wVieBag.cust = Customerslist().Result;
        //    ViewBag.ContractType = ContractTypeList().Result;
        //    ViewBag.ProjectTypeList = await GetProjecType();
        //    ViewBag.PracticeList = await GetPractice();
        //    ViewBag.SubpracticeList = await GetSubPractice();
        //    ViewBag.AccountMnager = await GetAccountManager();

        //    return View(data);
        //}
        [HttpGet]
        public async Task<IActionResult> Upsert(int id)
        {
            var viewModel = new UpsertOafViewModel
            {
                OafData = new Oaf()
            };
            viewModel.OafData.contractenddate = viewModel.OafData.contractenddate == null ? viewModel.OafData.contractenddate : DateTime.Now.AddMonths(12);
            viewModel.OafData.contractstartdate = viewModel.OafData.contractenddate == null ? viewModel.OafData.contractenddate : DateTime.Now;
            var apiManager = new ApiManager($"{_configuration["ServiceUrl"]}/api/Oaf/{id}");

            var (statusCode, responseContent) = await apiManager.Get();
            if (statusCode == System.Net.HttpStatusCode.OK)
            {
                viewModel.OafData = JsonConvert.DeserializeObject<Oaf>(responseContent, dateTimeConverter);
                
            }
            viewModel.Customers = await Customerslist();
            viewModel.ContractTypes = await ContractTypeList();
            viewModel.ProjectTypes = await GetProjecType();
            viewModel.Practices = await GetPractice();
            viewModel.Subpractices = await GetSubPractice();
            viewModel.AccountManagers = await GetAccountManager();

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Upsert(string contracts, IFormFile Emailattachment, IFormFile Proposalattachment,
            IFormFile Poattachment, IFormFile Costattachment)
        {
            try

            {
                if (contracts == null)
                {
                    ViewData["error"] = "Something Went Wrong !!";
                    return RedirectToAction("Index", "OAF");
                }
                var Jsondata = JsonConvert.DeserializeObject<Oaf>(contracts);

                string endpointUrl = $"{_configuration["ServiceUrl"].ToString().Trim()}/api/Oaf/{(Jsondata.oafid != 0 ? Jsondata.oafid.ToString() : "")}";
                apiManager = new ApiManager(endpointUrl, Session.Token);
               
                var (statusCode, responseContent) = Jsondata.oafid != 0
            ? await apiManager.CustomPut((contracts), Emailattachment, Proposalattachment, Poattachment, Costattachment)
            : await apiManager.CustomPost((contracts), Emailattachment, Proposalattachment, Poattachment, Costattachment);

                if (statusCode == HttpStatusCode.OK)
                {
                    dynamic apiResponse = JsonConvert.DeserializeObject<dynamic>(responseContent);
                    string practiceheademail = apiResponse.data.practiceheademail;
                    string salespersonemail = apiResponse.data.salespersonemail;
                    string salespersonname = apiResponse.data.salespersonname;
                    string practiceheadname = apiResponse.data.practiceheadname;
                    string projectname = apiResponse.data.projectname;
                    string customername = apiResponse.data.customername;
                   
                    
                    string mailTemplatePath = Path.Combine(_env.WebRootPath, "Resource", "Mailer", "genericmailer.html");
                    string mailBody = System.IO.File.Exists(mailTemplatePath) ? System.IO.File.ReadAllText(mailTemplatePath) : string.Empty;

                    string sMailBody = string.Format(
                                         mailBody,
                                         DateTime.Now.ToString("yyyy"),
                                         "New Order Acceptance Form Request Received for Project: " + projectname,
                                         "Order Acceptance Form Approval Request Received",
                                         practiceheadname,
                                         "A new Order Acceptance Form request has been received from " + salespersonname + " for Customer: " + customername + " on Project: " + projectname
                                     );




                    SendMail(practiceheademail, $"OAF Request Received From {salespersonname}", sMailBody, new List<string> { salespersonemail }, out string sMailOutput, null);





                    ViewData["success"] = Jsondata.oafid != 0
                        ? "OAF Updated Successfully !!"
                        : "OAF Created Successfully !!";
                    return Json(new { success = true, msg = "Submitted" });
                }
                else
                {
                    return Json(new { success = false, msg = "Submitted" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, msg = $"An error occurred: {ex.Message}" });
            }
        }


        public async Task<IActionResult> Detail(int id)
        {
            var viewModel = new UpsertOafViewModel
            {
                OafData = new Oaf()
            };

            var apiManager = new ApiManager($"{_configuration["ServiceUrl"]}/api/Oaf/{id}");

            var (statusCode, responseContent) = await apiManager.Get();

            if (statusCode == System.Net.HttpStatusCode.OK)
            {
                viewModel.OafData = JsonConvert.DeserializeObject<Oaf>(responseContent, dateTimeConverter);
                viewModel.OafData.Costsheetdata = await GetCostSheet((int)viewModel.OafData.costsheetid);
                ViewBag.resource = await GetEmployeedropdown();
                ViewBag.status = await GetCategorySubStatusoaf();
                // var days = (DateTime.Now.AddDays(30)-viewModel.OafData.contractenddate).Days;
                int? test = (viewModel.OafData.contractenddate - DateTime.Now.AddDays(30))?.Days;
                viewModel.OafData.IsExtendable = viewModel.OafData.contractenddate.HasValue &&
                                 (DateTime.Now >= viewModel.OafData.contractenddate.Value.AddMonths(-3) &&
                                  DateTime.Now <= viewModel.OafData.contractenddate.Value);
                return View(viewModel);
            }
            return View();
        }

        internal async Task<CostSheet> GetCostSheet(int id)
        {
            var costSheet = new CostSheet();

            apiManager = new ApiManager(_configuration["ServiceUrl"].ToString().Trim() + "/api/CostSheet/" + id); //--TOKEN
            var (statusCode, responseContent) = await apiManager.Get();

            if (statusCode == System.Net.HttpStatusCode.OK)
            {
                if (responseContent != null)
                {
                    costSheet = JsonConvert.DeserializeObject<CostSheet>(responseContent);
                }
            }
            return costSheet;
        }

        internal List<Oaf> FilterDataByStatus(List<Oaf> data, string status)
        {
            if (status != "Approved" && status != "Pending" && status != "Rejected")
            {
                return data;
            }
             return data.Where(o => o.status == status).ToList();
        }



        // Delivery Head Code

        [HttpGet]
        public async Task<IActionResult> OAFList(string status = "Total")
        {
            // Filter data based on the "status" parameter if provided
            if (string.IsNullOrEmpty(status))
                status = "Total";

            ViewData["controller"] = this.ControllerContext.RouteData.Values["controller"].ToString();
            var data = new List<Oaf>();
            apiManager = new ApiManager(_configuration["ServiceUrl"].ToString().Trim() + "/api/Oaf"); //--TOKEN
            var (statusCode, responseContent) = await apiManager.Get();

            if (statusCode == System.Net.HttpStatusCode.OK)
            {
                data = JsonConvert.DeserializeObject<List<Oaf>>(responseContent, dateTimeConverter);

                ViewBag.totalOAFCount = data != null ? data.Count() : 0;
                ViewBag.approvedOAFCount = data != null ? data.Count(o => o.status == "Approved") : 0;
                ViewBag.pendingOAFCount = data != null ? data.Count(o => o.status == "Pending") : 0;
                ViewBag.rejectedOAFCount = data != null ? data.Count(o => o.status == "Rejected") : 0;
                ViewBag.extendOAFCount = data !=null ? data.Count(o => o.isextended == true) : 0;

                // Filter data based on the "status" parameter if provided
                data = FilterDataByStatus(data, status);
            }

            ViewBag.status = status;
            return View(data.OrderByDescending(c => c.oafid));
        }

        public async Task<IActionResult> OAFDetail(int id)
        {
            var viewModel = new UpsertOafViewModel
            {
                OafData = new Oaf()
            };

            var apiManager = new ApiManager($"{_configuration["ServiceUrl"]}/api/Oaf/{id}");

            var (statusCode, responseContent) = await apiManager.Get();

            if (statusCode == System.Net.HttpStatusCode.OK)
            {
                viewModel.OafData = JsonConvert.DeserializeObject<Oaf>(responseContent, dateTimeConverter);
                viewModel.OafData.Costsheetdata = await GetCostSheet((int)viewModel.OafData.costsheetid);
                ViewBag.DA = await GetDA();
                return View(viewModel);
            }
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> DHResponse(string contracts, string Status)
        {
            try

            {
                if (contracts == null)
                {
                    ViewData["error"] = "Something Went Wrong !!";
                    return RedirectToAction("Index", "OAFList");
                }
                var Jsondata = JsonConvert.DeserializeObject<Oaf>(contracts);
                apiManager = new ApiManager(_configuration["ServiceUrl"].ToString().Trim() + "/api/Oaf/DHAction", Session.Token);

                var (statusCode, responseContent) = await apiManager.PostJson(contracts);

                if (statusCode == HttpStatusCode.OK)
                {
                    ViewData["success"] = Status =="Approved" ? "OAF Approved Successfully !!"
                        : "OAF Rejected Successfully !!";
                    return Json(new { success = true, msg = Status.ToString() });
                }
                else
                {
                    return Json(new { success = false, msg = "Submitted" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, msg = $"An error occurred: {ex.Message}" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CostingSheetPost(string jsonObject)
        {
            try
            {
                if (!string.IsNullOrEmpty(jsonObject))
                {

                    string endpointUrl = $"{_configuration["ServiceUrl"].ToString().Trim()}/api/CostSheet";
                    apiManager = new ApiManager(endpointUrl, Session.Token);

                    var (statusCode, responseContent) = await apiManager.PostJson((jsonObject));
                    if (statusCode == HttpStatusCode.OK)
                    {
                        var response = JsonConvert.DeserializeObject<CostSheet>(responseContent);
                        return Json(new { success = true, response });
                    }
                    else
                    {
                        return Json(new { success = false, msg = responseContent });
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, msg = ex.Message });
            }

            return Json(new { success = false, msg = "" });
        }

        [HttpPost]
        public async Task<IActionResult> resourceallocation(string jsonObject)
        {
            try
            {
                if (!string.IsNullOrEmpty(jsonObject))
                {
                    
                    string endpointUrl = $"{_configuration["ServiceUrl"].ToString().Trim()}/api/Oaf/OAFwiseResourceAllotment";
                    apiManager = new ApiManager(endpointUrl, Session.Token);

                    var (statusCode, responseContent) = await apiManager.PostJson(jsonObject);
                    if (statusCode == HttpStatusCode.OK)
                    {
                        return Json(new { success = true, responseContent });
                    }
                    else
                    {
                        return Json(new { success = false, msg = responseContent });
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, msg = ex.Message });
            }

            return Json(new { success = false, msg = "" });
        }



        public async Task<List<SelectListItem>> GetCategorySubStatusoaf()
        {
            var items = new List<SelectListItem>();

            apiManager = new ApiManager(_configuration["ServiceUrl"].ToString().Trim() + "/api/CategorySubStatus"); //--TOKEN
            var (statusCode, responseContent) = await apiManager.Get();
            if (statusCode == System.Net.HttpStatusCode.OK)
            {
                if (responseContent != null)
                {
                    List<CategorySubStatus> practices = JsonConvert.DeserializeObject<List<CategorySubStatus>>(responseContent);
                    // Filter and select only the required category substatuses
                    var filteredCategorySubstatuses = practices.Where(c =>
                        c.categorysubstatusname == "Deployed" ||
                        c.categorysubstatusname == "Allocation" ||
                        c.categorysubstatusname == "Projection" ||
                        c.categorysubstatusname == "Shadow"
                    );

                    
                    items = filteredCategorySubstatuses.Select(c => new SelectListItem
                    {
                        Value = c.categorysubstatusid.ToString(),
                        Text = c.categorysubstatusname.ToString()
                    }).ToList();
                }
            }

            return items;
        }

        [HttpPost]
        public async Task<IActionResult> OAFExtend(string jsonObject)
        {
            try
            {
                if (!string.IsNullOrEmpty(jsonObject))
                {

                    string endpointUrl = $"{_configuration["ServiceUrl"].ToString().Trim()}/api/Oaf/ExtendOaf";
                    apiManager = new ApiManager(endpointUrl, Session.Token);

                    var (statusCode, responseContent) = await apiManager.PostJson((jsonObject));
                    if (statusCode == HttpStatusCode.OK)
                    {
                        
                        return Json(new { success = true, responseContent });
                    }
                    else
                    {
                        return Json(new { success = false, msg = responseContent });
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, msg = ex.Message });
            }

            return Json(new { success = false, msg = "" });
        }
    }
}

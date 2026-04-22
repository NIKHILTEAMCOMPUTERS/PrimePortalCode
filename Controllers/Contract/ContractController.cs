using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using RMS.Client.Models.Contract;
using RMS.Client.Models.Employee;
using RMS.Client.Utility;
using System;
using System.Linq;
using System.Net;
using System.Xml.Linq;

namespace RMS.Client.Controllers.Contract
{
    public class ContractController : BaseController
    {
        private readonly IWebHostEnvironment _env;
        private ApiManager apiManager;
        private IConfiguration _configuration;
        private IsoDateTimeConverter dateTimeConverter;
        public ContractController(IConfiguration configuration, IWebHostEnvironment env)
              : base(configuration, env)
        {
            _configuration = configuration;
            _env = env ?? throw new ArgumentNullException(nameof(env));
            dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" };
        }

        [HttpGet]
        public async Task<IActionResult>ContractList()

        {
            if (!ReadPermission)
                return RedirectToAction("NotAuthorized", "Home");

            List<ContractRequestDto> data = new List<ContractRequestDto>();
            var apiUrl = (RowLevelSecurity) ? $"/api/Contract/GetContractsByDeliveryAnchor/{Session.EmployeeId}" : "/api/Contract";

            apiManager = new ApiManager(_configuration["ServiceUrl"].ToString().Trim() + apiUrl); //--TOKEN
            var (statusCode, responseContent) = await apiManager.Get();
            if (statusCode == System.Net.HttpStatusCode.OK)
                data = JsonConvert.DeserializeObject<List<ContractRequestDto>>(responseContent, dateTimeConverter);

            ViewData["controller"] = this.ControllerContext.RouteData.Values["controller"].ToString();
            return View(data.OrderByDescending(c => c.Contractid));
        }

        [HttpGet]
        public async Task<IActionResult> Index()

        {
            if (!ReadPermission)
                return RedirectToAction("NotAuthorized", "Home");



         

            List<ContractRequestDto> data = new List<ContractRequestDto>();
            var apiUrl = (RowLevelSecurity) ? $"/api/Contract/GetContractsByProcedure/{Session.EmployeeId}" : "/api/Contract/GetContractsByProcedure";
            ViewData["controller"] = this.ControllerContext.RouteData.Values["controller"].ToString();


            apiManager = new ApiManager(_configuration["ServiceUrl"].ToString().Trim() + apiUrl); //--TOKEN
            var (statusCode, responseContent) = await apiManager.Get();
            if (statusCode == System.Net.HttpStatusCode.OK)
            {
                ViewBag.JsonData = responseContent;
            }
            else
            {
                return Json(new { success = false, msg = $"An error occurred: {responseContent}" });
            }
            return View();

        }


        [HttpGet]
        public async Task<IActionResult> Upsert(int id)
        {

            if (Session.RoleName != "DA")
            {
                if (!WritePermission)
                    return RedirectToAction("NotAuthorized", "Home");
            }
           
            ViewData["controller"] = this.ControllerContext.RouteData.Values["controller"].ToString();
            ContractRequestDto data = new ContractRequestDto();
            data.Contractenddate = data.Contractenddate == null ? data.Contractenddate : DateTime.Now.AddMonths(12);
            data.Contractstartdate = data.Contractstartdate == null ? data.Contractstartdate : DateTime.Now;
            if (id != 0)
            {
                apiManager = new ApiManager(_configuration["ServiceUrl"].ToString().Trim() + "/api/Contract/" + id.ToString()); //--TOKEN
                var (statusCode, responseContent) = await apiManager.Get();
                if (statusCode == System.Net.HttpStatusCode.OK)
                {
                    data = JsonConvert.DeserializeObject<ContractRequestDto>(responseContent, dateTimeConverter);
                }
            }

            ViewBag.cust = Customerslist().Result;
            ViewBag.GetCostSheet = GetExistingCostingSheet().Result;
            ViewBag.ContractType = ContractTypeList().Result;
            ViewBag.DA = await GetDA();

            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> Upsert(string contracts)
        {
            try
            {
                if (Session.RoleName != "DA")
                {
                    if (!WritePermission)
                        return RedirectToAction("NotAuthorized", "Home");
                }

                if (contracts == null)
                {
                    ViewData["error"] = "Something Went Wrong !!";
                    return RedirectToAction("Index", "Contract");
                }

                var Jsondata = JsonConvert.DeserializeObject<ContractRequestDto>(contracts);

                if (Jsondata.Statusid != 6)
                {
                    if (Jsondata.Ponumber == null || Jsondata.Ponumber == "")
                    {
                        long timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                        Jsondata.Ponumber = "PO Awaited/" + timestamp.ToString();

                    }
                }

                Jsondata.Ctype = string.Empty;
                Jsondata.Invoiceperiod = string.Empty;

                IFormFileCollection Files = HttpContext.Request.Form.Files;

                string endpointUrl = $"{_configuration["ServiceUrl"].ToString().Trim()}/api/Contract/{(Jsondata.Contractid != 0 ? Jsondata.Contractid.ToString() : "")}";
                apiManager = new ApiManager(endpointUrl, Session.Token);

                var (statusCode, responseContent) = Jsondata.Contractid != 0
                                                  ? await apiManager.PutWithFiles((JsonConvert.SerializeObject(Jsondata)), Files)
                                                   : await apiManager.PostWithFiles((JsonConvert.SerializeObject(Jsondata)), Files);

                dynamic apiResponse = JsonConvert.DeserializeObject<dynamic>(responseContent);
                string newcontractid = string.Empty;

                if (apiResponse != null && apiResponse.data != null)
                {
                    newcontractid = apiResponse.data.contractid ?? string.Empty;
                }

                //string newcontractid = apiResponse?.data?.contractid ?? string.Empty; // Safely handle null

                if (statusCode == HttpStatusCode.OK)
                {
                    ViewData["success"] = Jsondata.Contractid != 0
                        ? "Contract Updated Successfully !!"
                        : "Contract Created Successfully !!";
                    return Json(new { success = true, msg = "Submitted", newcontractid });
                }
                else
                {
                    return Json(new { success = false, msg = $"An error occurred: {responseContent}" });
                }

            }
            catch (Exception ex)
            {
                return Json(new { success = false, msg = $"An error occurred: {ex.Message}" });
            }
        }


        //public async Task<IActionResult> Detail(int id, string startDate, string endDate)
        //{
        //    if (!ReadPermission)
        //        return RedirectToAction("NotAuthorized", "Home");

        //    ViewData["controller"] = this.ControllerContext.RouteData.Values["controller"].ToString();
        //    ContractRequestDto data = new ContractRequestDto();
        //    apiManager = new ApiManager(_configuration["ServiceUrl"].ToString().Trim() + "/api/Contract/" + id.ToString()); //--TOKEN
        //    var (statusCode, responseContent) = await apiManager.Get();
        //    if (statusCode == System.Net.HttpStatusCode.OK)
        //    {
        //        data = JsonConvert.DeserializeObject<ContractRequestDto>(responseContent, dateTimeConverter);
        //    }

        //    //    var consumedamount = data.ContractEmployees?
        //    //.SelectMany(a => a.ContractBillings) // Flatten ContractBillings
        //    //.Sum(b => b.Costing.Value);

        //    //    data.unlocatedamount = (data.Amount == 0) ? 0 : data.Amount - consumedamount;
        //    var consumedAmount = data.ContractEmployees?
        //  .SelectMany(ce => ce.ContractBillings) // Flatten ContractBillings
        //  .Sum(cb => cb.Costing ?? 0); // Safely handle null Costing

        //    // Subtract consumed amount from total amount
        //    data.unlocatedamount = (data.Amount == 0) ? 0 : data.Amount - (consumedAmount ?? 0); // Safely handle null consumedAmount

        //    data.GetEmployee = await GetEmployeedropdown();



        //    int committedDay = (int)data?.CommittedClientBillingDate;

        //    int currentYear = DateTime.Now.Year;
        //    int currentMonth = DateTime.Now.Month;

        //    DateTime committedDate = new DateTime(currentYear, currentMonth, committedDay);
        //    DateTime nextMonthDate = DateTime.Now.AddMonths(1);
        //    if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
        //    {

        //        // Filter data based on the selected date range
        //        //data.ContractEmployees.ForEach(contract =>
        //        //{
        //        //    contract.ContractBillings = FilterData(startDate, endDate, contract.ContractBillings);
        //        //});
        //        //data.ContractEmployees.ForEach(contract =>
        //        //{
        //        //    contract.contractBillingsProvisional = FilterProData(startDate, endDate, contract.contractBillingsProvisional);
        //        //});

        //        return Json(data);
        //    }
        //    var catsubList = await GetCategorySubStatus();
        //    ViewBag.SubCategoryStatusList = catsubList;
        //    //ViewBag.DA = await GetEmployee();
        //    ViewBag.billingdate = committedDate.ToString("yyyy-MM-dd");
        //    ViewBag.nextmonthbillingdate = new DateTime(nextMonthDate.Year, nextMonthDate.Month, DateTime.DaysInMonth(nextMonthDate.Year, nextMonthDate.Month));
        //    ViewBag.BillingPermit = BillingPermission;
        //    return View(data);
        //}
        public async Task<IActionResult> Detail(int id, string startDate, string endDate)
        {
            if (!ReadPermission)
                return RedirectToAction("NotAuthorized", "Home");

            ViewData["controller"] = this.ControllerContext.RouteData.Values["controller"].ToString();
            ContractRequestDto data = new ContractRequestDto();
            apiManager = new ApiManager(_configuration["ServiceUrl"].ToString().Trim() + "/api/Contract/" + id.ToString());
            var (statusCode, responseContent) = await apiManager.Get();
            if (statusCode == System.Net.HttpStatusCode.OK)
            {
                data = JsonConvert.DeserializeObject<ContractRequestDto>(responseContent, dateTimeConverter);
            }


            if((data.ContractEmployees == null || data.ContractEmployees.Count == 0) && data.Oaf.Select(a => a.oafid !=0 || a.oafid !=null).FirstOrDefault())
            {
                ViewBag.costsheetdata = await GetCostSheet((int)data.Oaf.Select(a => a.costsheetid).FirstOrDefault());
                
            }

            var consumedAmount = (data.ContractEmployees?.SelectMany(ce => ce.ContractBillings).Sum(cb => cb.Costing ?? 0) ?? 0)
                                +(data.ContractEmployees?.SelectMany(ce => ce.contractBillingsProvisional).Where(x => x.statusId == 5).Sum(cb => cb.Costing ?? 0) ?? 0);

            data.unlocatedamount = (data.Amount == 0) ? 0 : data.Amount - consumedAmount;
            data.GetEmployee = await GetEmployeedropdown();

            if (data != null && data.ProjectInfo != null && (data.ProjectInfo.committedClientBillingDate.HasValue && data.ProjectInfo.committedClientBillingDate !=0))
            {
                int committedDay = data.ProjectInfo.committedClientBillingDate.Value;
                DateTime now = DateTime.Now;

                DateTime committedDate = new DateTime(now.Year, now.Month, Math.Min(committedDay, DateTime.DaysInMonth(now.Year, now.Month)));

                DateTime nextMonthDate = now.AddMonths(1);
                DateTime nextCommittedDate = new DateTime(nextMonthDate.Year, nextMonthDate.Month, Math.Min(committedDay, DateTime.DaysInMonth(nextMonthDate.Year, nextMonthDate.Month)));

                ViewBag.billingdate = committedDate.ToString("yyyy-MM-dd");
                ViewBag.nextmonthbillingdate = nextCommittedDate.ToString("yyyy-MM-dd");
            }
            if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
            {
              
                return Json(data);
            }
            var catsubList = await GetCategorySubStatus();
            ViewBag.SubCategoryStatusList = catsubList;
            ViewBag.BillingPermit = BillingPermission;
            return View(data);
        }

        public List<ContractBillinginfo> FilterData(string startDate, string endDate, List<ContractBillinginfo> billingData)
        {
            // Filter your data based on the selected date range
           // billingData.Reverse();
            //var flag = true;
            var startInd = billingData.FindIndex(item => item.BillingMonthYear == startDate);
            var endInd = billingData.FindIndex(item => item.BillingMonthYear == endDate);

            List<ContractBillinginfo> filterbilling = new List<ContractBillinginfo>();
            for(int i=startInd; i<=endInd; i++)
            {
                filterbilling.Add(billingData[i]);
            }
          
            return filterbilling;
        }
        public List<ContractBillingProvisionals> FilterProData(string startDate, string endDate, List<ContractBillingProvisionals> billingData)
        {
            List<ContractBillingProvisionals> filterbilling = new List<ContractBillingProvisionals>();
            if (billingData.Count > 0)
            { // Filter your data based on the selected date range
              // billingData.Reverse();
              //var flag = true;
                var startInd = billingData.FindIndex(item => item.BillingMonthYear == startDate);
                var endInd = billingData.FindIndex(item => item.BillingMonthYear == endDate);

               
                for (int i = startInd; i <= endInd; i++)
                {
                    filterbilling.Add(billingData[i]);
                }

                return filterbilling;
            }
            else { return filterbilling; }
            
        }

        #region na
        [HttpPost]
        //public async Task<IActionResult> Billing(string jsonObject, bool IsProvision = false)
        //{
        //    try
        //    {
        //        if (!string.IsNullOrEmpty(jsonObject))
        //        {
        //            string endpointUrl;

        //            // Check if IsProvision is false and the current date is greater than the 25th of the month


        //            var Jsondata = JsonConvert.DeserializeObject<List<ContractBillinginfo>>(jsonObject);

        //            var obj = Jsondata.Select(c => new
        //            {
        //                conractEmployeeId = c.ContractEmployeeId,
        //                billingMonthYear = c.BillingMonthYear,
        //                costing = c.Costing
        //            }).ToList();
        //            if (!IsProvision && DateTime.Now.Day > 25)
        //            {
        //                return Json(new { success = false, msg = "You can only update billing before 25th of the month!!" });
        //            }
        //            if (IsProvision)
        //            {
        //                endpointUrl = $"{_configuration["ServiceUrl"].ToString().Trim()}/api/ContractBilling/BilligProvision";
        //            }
        //            else
        //            {
        //                endpointUrl = $"{_configuration["ServiceUrl"].ToString().Trim()}/api/ContractBilling";
        //            }

        //            apiManager = new ApiManager(endpointUrl, Session.Token);
        //            var (statusCode, responseContent) = await apiManager.PostJson(JsonConvert.SerializeObject(obj));

        //            if (statusCode == HttpStatusCode.OK)
        //            {
        //                return Json(new { success = true, msg = "Submitted" });
        //            }
        //            else
        //            {
        //                return Json(new { success = false, msg = responseContent });
        //            }
        //        }
        //        else
        //        {
        //            return Json(new { success = false, msg = "JSON data is empty" });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { success = false, msg = ex.Message });
        //    }
        //}
        #endregion

        [HttpPost]
        public async Task<IActionResult> Billing(string jsonObject, bool IsProvision = false)
        {   
            try
            {
                object objToSerialize;
                if (!string.IsNullOrEmpty(jsonObject))
                {
                    string endpointUrl;

                    // Get the current date
                    var currentDate = DateTime.Now;

                    if (IsProvision)
                    {
                        var Jsondatapro = JsonConvert.DeserializeObject<List<ContractBillingProvisionals>>(jsonObject);
                        var objpro = Jsondatapro.Select(c =>
                        {
                            return c.estimatedDate != null
                                ? (object)new
                                {
                                    Contractemployeeid = c.Contractemployeeid,
                                    billingMonthYear = c.BillingMonthYear,
                                    costing = c.Costing,
                                    isRevised = c.isRevised,
                                    Exptectedbillingdate = c.estimatedDate
                                }
                                : new
                                {
                                    Contractemployeeid = c.Contractemployeeid,
                                    billingMonthYear = c.BillingMonthYear,
                                    costing = c.Costing,
                                    isRevised = c.isRevised
                                };
                        }).ToList();
                        objToSerialize = objpro;
                    }
                    else
                    {
                        var Jsondata = JsonConvert.DeserializeObject<List<ContractBillinginfo>>(jsonObject);

                        var obj = Jsondata.Select(c => new
                        {
                            Contractemployeeid = c.Contractemployeeid,
                            billingMonthYear = c.BillingMonthYear,
                            costing = c.Costing,
                            Exptectedbillingdate = c.estimatedDate
                        }).ToList();
                        objToSerialize = obj;
                    }
                    //foreach (var item in obj)
                    //{
                    //    // Parse the billingMonthYear to a DateTime object
                    //  //  var billingDate = DateTime.ParseExact(item.billingMonthYear, "MMM-yy", CultureInfo.InvariantCulture);

                    //    // Check if billingDate is before the current date
                    //    //if (billingDate < currentDate)
                    //    //{
                    //    //    return Json(new { success = false, msg = "You can only update billing for the current month and subsequent months!!" });
                    //    //}
                    //}

                    if (IsProvision)
                    {
                        endpointUrl = $"{_configuration["ServiceUrl"].ToString().Trim()}/api/ContractBilling/BilligProvision";
                    }
                    else
                    {
                        endpointUrl = $"{_configuration["ServiceUrl"].ToString().Trim()}/api/ContractBilling";
                    }

                    apiManager = new ApiManager(endpointUrl, Session.Token);

                    var data = JsonConvert.SerializeObject(objToSerialize);

                    var (statusCode, responseContent) = await apiManager.PostJson(data);


                    if (statusCode == HttpStatusCode.OK)
                    {
                        return Json(new { success = true, msg = "Billing Updated!!" });
                    }
                    else
                    {
                        return Json(new { success = false, msg = responseContent });
                    }
                }
                else
                {
                    return Json(new { success = false, msg = "JSON data is empty" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, msg = ex.Message });
            }
        }

        public async Task<IActionResult> GetCheckList(int practiceId)
        {
            apiManager = new ApiManager(_configuration["ServiceUrl"].ToString().Trim() + "/api/PreSalesQuestionRepository/GetQuesByPracticeId?PracticeId=" + practiceId.ToString(), Session.Token);
            var (statusCode, responseContent) = await apiManager.Get();

            if (statusCode == HttpStatusCode.OK && responseContent != null)
            {
                return Json(responseContent);
            }

            return Json(responseContent);
        }

       

        //FOR REVISION DATA
        public async Task<IActionResult> GetRevisionHistory(string obj)
        {
            try
            {
                if (string.IsNullOrEmpty(obj))
                {
                    return Json(new { success = false, msg = "Please provide valid data!" });
                }

                string endpointUrl = _configuration["ServiceUrl"] + "/api/Report/GetProvisionRptMonth";
                apiManager = new ApiManager(endpointUrl, Session.Token);

                var (statusCode, responseContent) = await apiManager.PostJson(obj);

                if (statusCode == HttpStatusCode.OK)
                {
                    return Json(new { success = true, responseContent });
                }
                else
                {
                    return Json(new { success = false, msg = "Something went wrong. Please try again." });
                }
            }
            catch (Exception ex)
            {
                ViewData["error"] = $"An error occurred: {ex.Message}";
                return Json(new { success = false, msg = $"An error occurred: {ex.Message}" });
            }
        }

        public async Task<IActionResult> CheckContractno(string contractno, string contractid)
        {
            try
            {
                if (!string.IsNullOrEmpty(contractno))
                {
                    string endpointUrl;

                    var contractData = new
                    {
                        Contractno = contractno,
                        Contractid = Convert.ToInt32(contractid)
                    };

                    // Serialize the anonymous object into JSON
                    string jsonData = JsonConvert.SerializeObject(contractData);
                    endpointUrl = $"{_configuration["ServiceUrl"].ToString().Trim()}/api/Contract/CheckContractNo";
                    apiManager = new ApiManager(endpointUrl, Session.Token);

                    var (statusCode, responseContent) = await apiManager.PostJson(jsonData);

                    if (statusCode == HttpStatusCode.OK)
                    {
                        return Json(new { success = true, responseContent });
                    }
                    else
                    {
                        return Json(new { success = false, msg = responseContent });
                    }
                }
                else
                {
                    return Json(new { success = false, msg = "Something Went Wrong!!" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, msg = ex.Message });
            }
        }
        public async Task<IActionResult> CheckPoNumber(string ponumber, string contractid)
        {
            try
            {
                if (!string.IsNullOrEmpty(ponumber))
                {
                    string endpointUrl;

                    var contractData = new
                    {
                        PoNo = ponumber,
                        Contractid = Convert.ToInt32(contractid)
                    };

                    // Serialize the anonymous object into JSON
                    string jsonData = JsonConvert.SerializeObject(contractData);
                    endpointUrl = $"{_configuration["ServiceUrl"].ToString().Trim()}/api/Contract/CheckPOno";
                    apiManager = new ApiManager(endpointUrl, Session.Token);

                    var (statusCode, responseContent) = await apiManager.PostJson(jsonData);

                    if (statusCode == HttpStatusCode.OK)
                    {
                        return Json(new { success = true, responseContent });
                    }
                    else
                    {
                        return Json(new { success = false, msg = responseContent });
                    }
                }
                else
                {
                    return Json(new { success = false, msg = "Something Went Wrong!!" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, msg = ex.Message });
            }
        }

        public async Task<IActionResult> Forclosure(string Id)
        {
            try
            {
                int ContractId = Convert.ToInt32(Id);
                if (ContractId > 0)
                {
                    string endpointUrl;

                    var forclosureobject = new
                    {
                        ContractId = ContractId,
                        ForeClosureDate = DateTime.Now,
                    };
                     string jsonData = JsonConvert.SerializeObject(forclosureobject);
                    endpointUrl = $"{_configuration["ServiceUrl"].ToString().Trim()}/api/Contract/ForeclosureBillingAction";
                    apiManager = new ApiManager(endpointUrl, Session.Token);

                    var (statusCode, responseContent) = await apiManager.PostJson(jsonData);

                    if (statusCode == HttpStatusCode.OK)
                    {
                        return Json(new { success = true, responseContent });
                      
                    }
                    else
                    {
                        return Json(new { success = false, msg = responseContent });
                    }
                }
                else
                {
                    return Json(new { success = false, msg = "Something Went Wrong!!" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, msg = ex.Message });
            }
        }
        public async Task<IActionResult>CheckContractBilling(string contractno,string contractid,string inputdate)
        {
            try {

                var data = new
                {
                    contractno = contractno,
                    contractid = contractid,
                    inputdate = Convert.ToDateTime(inputdate)
                };

                if (data.contractid == null) { return Json(new { success = false, msg = "Something Went Wrong!!" }); }
                string endpointUrl = _configuration["ServiceUrl"].ToString().Trim() + "/api/Contract/CheckContractForeClosure";
                apiManager = new ApiManager(endpointUrl,Session.Token);
                var (statusCode,responseContent) = await apiManager.PostJson(JsonConvert.SerializeObject(data));
                if (statusCode == HttpStatusCode.OK)
                return Json(new { success = true, responseContent });
                else
                return Json(new { success = false, responseContent });
            }
            catch (Exception ex)
            {
                 return Json(new { success = false, msg = $"An error occurred: {ex.Message}" });
            }

        }

        [HttpPost]
        public async Task<IActionResult> Acknowledgement(string cid)
        {
            if (RoleName != "DA")
                return RedirectToAction("NotAuthorized", "Home");

            try
            {
                int contractid = Convert.ToInt32(cid);
                ContractRequestDto data = new ContractRequestDto
                {
                    Contractid = contractid,
                    Isprojectestimationdone = true, // 
                };

                apiManager = new ApiManager(_configuration["ServiceUrl"] + "/api/Contract/isprojectestimationdone/", Session.Token);
                var apiResult = await apiManager.PostJson(System.Text.Json.JsonSerializer.Serialize(data));

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

    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using RMS.Client.Models.Contract;
using RMS.Client.Models.Employee;
using RMS.Client.Models.Vendor;
using RMS.Client.Utility;
using System.Net;

namespace RMS.Client.Controllers.Contract
{
    public class CostingSheetController : BaseController
    {
        private ApiManager apiManager;
        private IConfiguration _configuration;
        public CostingSheetController(IConfiguration configuration, IWebHostEnvironment env)
              : base(configuration, env)
        {
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            var apiManager = new ApiManager(_configuration["ServiceUrl"].ToString().Trim() + "/api/CostSheet");
            var (statusCode, responseContent) = await apiManager.Get();

            if (statusCode == HttpStatusCode.OK && responseContent != null)
            {
                var cust = JsonConvert.DeserializeObject<List<CostSheet>>(responseContent);
                return View(cust.OrderByDescending(x => x.costsheetid));
            }

            return View(new List<CostSheet>());
        }

        public async Task<IActionResult> Costsheetupsert(int? id)
        {
            var costSheet = new CostSheet();
            ViewBag.skilllist = await GetSkillList();

            try
            {
                if (id > 0 && id != null)
                {
                    apiManager = new ApiManager(_configuration["ServiceUrl"].ToString().Trim() + "/api/CostSheet/" + id); //--TOKEN
                    var (statusCode, responseContent) = await apiManager.Get();

                    if (statusCode == System.Net.HttpStatusCode.OK)
                    {
                        if (responseContent != null)
                        {
                            costSheet = JsonConvert.DeserializeObject<CostSheet>(responseContent);
                            return View(costSheet);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                
                ViewData["ErrorMessage"] = "An error occurred while processing your request. Please try again later.";
            }

            return View(costSheet);
        }



        //[HttpPost]
        //public async Task<JsonResult> Upsert(string costingSheet, string costingSheetValue)
        //{
        //    try
        //    {
        //        var apiUrl = _configuration["ServiceUrl"].ToString().Trim() + "/api/Contract/SaveQuestions/";
        //        var apiManager = new ApiManager(apiUrl, Session.Token);
        //        var (statusCode, responseContent) = await apiManager.PostJson(costingSheet);

        //        return statusCode == HttpStatusCode.OK
        //            ? Json(new { StatusCode = (int)statusCode, Content = responseContent })
        //            : Json(new { Content = responseContent });
        //    }
        //    catch (Exception ex)
        //    {

        //        return Json(new { Error = ex.Message });
        //    }
        //}
        [HttpPost]
        public async Task<IActionResult> CostingSheetPost(string jsonObject, int costsheetid = 0)
        {
            try
            {
                if (string.IsNullOrEmpty(jsonObject))
                    return Json(new { success = false, msg = "JSON object is empty or null" });

                string endpointUrl = costsheetid > 0 ? $"{_configuration["ServiceUrl"]}/api/CostSheet/" + costsheetid : $"{_configuration["ServiceUrl"]}/api/CostSheet";
                apiManager = new ApiManager(endpointUrl, Session.Token);
                
                var (statusCode, responseContent) = costsheetid != 0
                    ? await apiManager.Put(jsonObject)
                    : await apiManager.PostJson(jsonObject);

                if (statusCode == HttpStatusCode.OK)
                    return Json(new { success = true, response = JsonConvert.DeserializeObject<CostSheet>(responseContent) });
                else
                    return Json(new { success = false, msg = responseContent });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, msg = ex.Message });
            }
        }
        public async Task<IActionResult> CostingSheetDetail(int? id)
        {
            var costSheet = new CostSheet();
            
            if (id > 0 && id != null)
            {
                apiManager = new ApiManager(_configuration["ServiceUrl"].ToString().Trim() + "/api/CostSheet/" + id); //--TOKEN
                var (statusCode, responseContent) = await apiManager.Get();

                if (statusCode == System.Net.HttpStatusCode.OK)
                {
                    if (responseContent != null)
                    {
                        costSheet = JsonConvert.DeserializeObject<CostSheet>(responseContent);
                        return View(costSheet);
                    }
                }
            }


            return View(costSheet);
        }
        public async Task<IActionResult> DeleteCostSheet(int id)
        {
            try
            {
                if (id != 0)
                {
                    apiManager = new ApiManager(_configuration["ServiceUrl"].ToString().Trim() + $"/api/CostSheet/+{id}");
                    var (statusCode, responseContent) = await apiManager.Delete();
                    if (statusCode == System.Net.HttpStatusCode.OK)
                    {
                        TempData["success"] = "CostSheet deleted successfully!";
                        return RedirectToAction("Index", "CostingSheet");
                    }
                    else
                    {
                        TempData["error"] = "Failed to delete CostSheet";
                        return RedirectToAction("Index", "CostingSheet");
                    }
                }

            }
            catch (Exception ex)
            {
                ViewData["error"] = $"An error has occurred: {ex.Message}";

            }
            return View();

        }
        // for duplication

        public async Task<IActionResult> CheckCostSheetName(string costsheetname, string costsheetid)
        {
            try
            {
                if (!string.IsNullOrEmpty(costsheetname))
                {
                    string endpointUrl;

                    var data = new
                    {
                        CostSheetName = costsheetname,
                        costSheetid = Convert.ToInt32(costsheetid)
                    };

                    // Serialize the anonymous object into JSON
                    string jsonData = JsonConvert.SerializeObject(data);
                    endpointUrl = $"{_configuration["ServiceUrl"].ToString().Trim()}/api/CostSheet/CheckCostsheet";
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
    }
}

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RMS.Client.Models.Timesheet;
using RMS.Client.Utility;
using System.Net;

namespace RMS.Client.Controllers.Timesheet
{
    public class TemplateController : BaseController
    {
        private ApiManager apiManager;
        private IConfiguration _configuration;
        public TemplateController(IConfiguration configuration, IWebHostEnvironment env)
              : base(configuration, env)
        {
            _configuration = configuration;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                if (!ReadPermission)
                    return RedirectToAction("NotAuthorized", "Home");
                var tem = new List<Templates>();
                ViewData["controller"] = this.ControllerContext.RouteData.Values["controller"].ToString();
                apiManager = new ApiManager(_configuration["ServiceUrl"].ToString().Trim() + $"/api/Template/GetTemplate?employeeId={Session.EmployeeId}");

                var (statusCode, ResponseContent) = await apiManager.Get();

                if (statusCode == System.Net.HttpStatusCode.OK)
                {
                    if (ResponseContent != null)
                    {
                        tem = JsonConvert.DeserializeObject<List<Templates>>(ResponseContent);
                    }
                }
                return View(tem);

            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred: " + ex.Message;
                return View();
            }
        }
        [HttpGet]
        public async Task<IActionResult> CreateTemplate(int id, bool isSummary = false)
        {
            try
            {
                ViewData["controller"] = this.ControllerContext.RouteData.Values["controller"].ToString();
                ViewBag.IsSummaryTemplate = isSummary;
                ViewBag.project = await GetProjectForTimesheet();
                ViewBag.department = await GetDepartment();
                Templates data = new Templates();
                var existingTemplate = await GetExistingTemplateCount();
                ViewBag.templateCount = existingTemplate;
                //if (existingTemplate >= 5)
                //{
                //    return RedirectToAction("Index","Template");
                //}
                if (id <= 0 || id == 0)
                {
                    return View();
                }
                var url = _configuration["ServiceUrl"] + $"/api/Template/GetTemplateById?id={id}";

              //  var url = _configuration["GetTemplateById"].Trim() + id;
                apiManager = new ApiManager(url);
                var (statusCode, ResponseContent) = await apiManager.Get();

                if (statusCode == HttpStatusCode.OK && !string.IsNullOrEmpty(ResponseContent))
                {
                    data = JsonConvert.DeserializeObject<Templates>(ResponseContent);
                    return View(data);
                }

                return View(data);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred: " + ex.Message;
                return View();
            }
        }
        internal async Task<int> GetExistingTemplateCount()
        {
            try
            {
                apiManager = new ApiManager(_configuration["ServiceUrl"].ToString().Trim() + $"/api/Template/GetTemplateCount?employeeId={Session.EmployeeId}");

                var (statusCode, ResponseContent) = await apiManager.Get();

                if (statusCode == System.Net.HttpStatusCode.OK)
                {
                    var tem = JsonConvert.DeserializeObject<int>(ResponseContent);
                    return tem;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return 0;
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateTemplate([FromBody] Templates templateData)
        {
            try
            {
                int EmployeeId = Session.EmployeeId;
                templateData.Employeeid = EmployeeId;
                templateData.Createddate = DateTime.Now;
                var url = _configuration["ServiceUrl"] + $"/api/Template/UpsertTemplate?id={templateData.Templateid}";
                //var url = _configuration["UpsertTemplate"]?.Trim() + templateData.Templateid;
                apiManager = new ApiManager(url,Session.Token);

                var (statusCode, ResponseContent) = templateData.Templateid < 0
                    ? await apiManager.PostJson(JsonConvert.SerializeObject(templateData))
                    : await apiManager.PostJson(JsonConvert.SerializeObject(templateData));

                return Json(new
                {
                    success = statusCode == System.Net.HttpStatusCode.OK,
                    msg = statusCode == System.Net.HttpStatusCode.OK ?
                    (templateData.Templateid < 0 ? "Template Added" : "Template Updated") : "Error submitting data"
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, msg = $"An error occurred: {ex.Message}" });
            }
        }

        public async Task<JsonResult> GetTemplateById(int id)
        {
            try
            {
                var tem = new Templates();
                apiManager = new ApiManager(_configuration["ServiceUrl"].ToString().Trim() + $"/api/Template/GetTemplateById?id={id}");
                var (statusCode, ResponseContent) = await apiManager.Get();
                if (statusCode == System.Net.HttpStatusCode.OK)
                {
                    if (ResponseContent != null)
                    {
                        tem = JsonConvert.DeserializeObject<Templates>(ResponseContent);
                    }
                }
                return Json(ResponseContent);

            }
            catch
            {
                throw;
            }

        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (id != 0)
                {
                    apiManager = new ApiManager(_configuration["ServiceUrl"].ToString().Trim() + $"/api/Template/DeleteTemplate?id={id}");
                    var (statusCode, responseContent) = await apiManager.Delete();
                    if (statusCode == System.Net.HttpStatusCode.OK)
                    {
                        TempData["success"] = "Template deleted successfully!";
                        return RedirectToAction("Index", "Template");
                    }
                    else
                    {
                        TempData["error"] = "Failed to delete Template";
                        return RedirectToAction("Index", "Template");
                    }
                }

            }
            catch(Exception ex)
            {
                ViewData["error"] = $"An error has occurred: {ex.Message}";

            }
            return View();

        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RMS.Client.Utility;
using RMS.Client.Models.Master;
using RMS.Client.Models.Employee;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace RMS.Client.Controllers.Project
{
    public class ProjectController : BaseController
    {
        private ApiManager apiManager;
        private IConfiguration _configuration;
        public ProjectController(IConfiguration configuration, IWebHostEnvironment env)
              : base(configuration, env)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (!ReadPermission)
                return RedirectToAction("NotAuthorized", "Home");

            var project = RowLevelSecurity ? await GetProjects() : await GetProjects();

            ViewData["controller"] = this.ControllerContext.RouteData.Values["controller"].ToString();
            return View(project);
        }
        //await GetProjectByDa(Session.EmployeeId)

        [HttpGet]
        public async Task<IActionResult> Create(int projectid)
        {
            if (Session.RoleName != "DA")
            {
                if (!WritePermission)
                    return RedirectToAction("NotAuthorized", "Home");
            }

            Projects data = new Projects();

            if (projectid != 0)
            {

                apiManager = new ApiManager(_configuration["ServiceUrl"].ToString().Trim() + "/api/Project/" + projectid.ToString()); //--TOKEN
                var (statusCode, responseContent) = await apiManager.Get();
                if (statusCode == System.Net.HttpStatusCode.OK)
                {
                    data = JsonConvert.DeserializeObject<Projects>(responseContent);
                }
            }
            var catsubList = await GetCategorySubStatus();
            var customerList = await GetCustomer();
            ViewBag.CustomerList = customerList;
            //ViewBag.ProjectModelList = await GetProjectModel();
            ViewBag.ProjectTypeList = await GetProjecType();
            ViewBag.PracticeList = await GetPractice();
            ViewBag.SubpracticeList = await GetSubPractice();
            ViewBag.AccountManagers = await GetAccountManager();
            ViewBag.DA = await GetDA();
            ViewBag.SubCategoryStatusList = catsubList;
            ViewBag.Employee = await GetEmployeedropdown();
            //ViewBag.url = _configuration["ServiceUrl"].ToString().Trim();

            ViewData["controller"] = this.ControllerContext.RouteData.Values["controller"].ToString();
            return View(data);
        }
        [HttpPost]
        public async Task<IActionResult> UpsertAsync(Projects project)
        {
            if (Session.RoleName != "DA")
            {
                if (!WritePermission)
                    return RedirectToAction("NotAuthorized", "Home");
            }

            ViewData["controller"] = this.ControllerContext.RouteData.Values["controller"].ToString();
            const string redirectAction = "Index";
            const string redirectController = "Project";
            //   IFormFileCollection images = HttpContext.Request.Form.Files;    if image required
            if (project == null)
            {
                TempData["error"] = "Something Went Wrong !!";
                return RedirectToAction(redirectAction, redirectController);
            }
            var requestObject = new ProjectAddDto
            {
                ProjectId = project.ProjectId,
                ProjectName = project.ProjectName,
                ProjectNo = project.Projectno,
                ProjectDescription = project.ProjectDescription,
                CustomerId = project.CustomerId,
                //ProjectModelId = project.ProjectModelId,
                //ProjectHeadId = project.ProjectHeadId,
                ProjectTypeId = project.ProjectTypeId,
                SubPractiseId = project.SubPracticeId,
                accountmanagerid = project.AccountManagerId,
                CommittedClientBillingDate = project.CommittedClientBillingDate
            };

            // for update
            if (project.ProjectId != 0 && project.ProjectId != null)
            {
                string projectStrings = System.Text.Json.JsonSerializer.Serialize(requestObject);
                apiManager = new ApiManager(_configuration["ServiceUrl"].ToString().Trim() + "/api/Project/UpdateProjectDraft/" + project.ProjectId.ToString(), Session.Token);
                var (statusCodes, responseContents) = await apiManager.PostJson(projectStrings);

                if (statusCodes == System.Net.HttpStatusCode.OK)
                {
                    TempData["success"] = "Project Submited Successfully !!";

                    return RedirectToAction("Create", redirectController, new { projectid = project.ProjectId });
                }

            }
            // for new record
            else
            {
                string projectString = System.Text.Json.JsonSerializer.Serialize(requestObject);
                apiManager = new ApiManager(_configuration["ServiceUrl"].ToString().Trim() + "/api/Project/SaveAsDraft", Session.Token);
                var (statusCode, responseContent) = await apiManager.PostJson(projectString);

                if (statusCode == System.Net.HttpStatusCode.OK)
                {
                    TempData["success"] = "Project Submited Successfully !!";
                    if (statusCode == System.Net.HttpStatusCode.OK && project.poawaited != false)
                    {
                        return Json(responseContent);
                    }
                    else
                    {
                        return RedirectToAction(redirectAction, redirectController);
                    }
                }
            }
            return RedirectToAction(redirectAction, redirectController);
        }
        [HttpGet]
        public async Task<IActionResult> GetSubPracticesByPractice(int practiceId)
        {
            var items = await GetSubPracticeByPracticeId(practiceId);
            ViewData["controller"] = this.ControllerContext.RouteData.Values["controller"].ToString();
            return Json(items);
        }


        [HttpGet]
        public async Task<IActionResult> View(int id)
        {
            if (!ReadPermission)
                return RedirectToAction("NotAuthorized", "Home");

            ProjectDetail data = new ProjectDetail();

            if (id != 0)
            {
                apiManager = new ApiManager(_configuration["ServiceUrl"].ToString().Trim() + "/api/Project/" + id.ToString()); //--TOKEN
                var (statusCode, responseContent) = await apiManager.Get();
                if (statusCode == System.Net.HttpStatusCode.OK)
                {
                    data = JsonConvert.DeserializeObject<ProjectDetail>(responseContent);
                }
            }
            ViewData["controller"] = this.ControllerContext.RouteData.Values["controller"].ToString();
            return View(data);
        }

        // for validation
        public async Task<IActionResult> CheckProjectNumber(string projectno, string projectid)
        {
            try
            {
                if (!string.IsNullOrEmpty(projectno))
                {
                    string endpointUrl;

                    var contractData = new
                    {
                        projectno = projectno,
                        Projectid = Convert.ToInt32(projectid)
                    };

                    // Serialize the anonymous object into JSON
                    string jsonData = JsonConvert.SerializeObject(contractData);
                    endpointUrl = $"{_configuration["ServiceUrl"].ToString().Trim()}/api/Project/CheckProjectno";
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
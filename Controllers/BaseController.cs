using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using RMS.Client.Models.Account;
using RMS.Client.Models.Common;
using RMS.Client.Models.Contract;
using RMS.Client.Models.Customer;
using RMS.Client.Models.Dashboard;
using RMS.Client.Models.Employee;
using RMS.Client.Models.Master;
using RMS.Client.Utility;
using RMS.Utility;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Net;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace RMS.Client.Controllers
{
    public class BaseController : Controller
    {
        IConfiguration _baseConfig;
        Microsoft.AspNetCore.Hosting.IWebHostEnvironment _env;
        private ApiManager apiManager;
        private IsoDateTimeConverter dateTimeConverter;
        public BaseController(IConfiguration config, Microsoft.AspNetCore.Hosting.IWebHostEnvironment env)
        {
            _baseConfig = config;
            _env = env;
            dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" };
        }

        public bool ReadPermission { get; set; }
        public bool WritePermission { get; set; }
        public bool DeletePermission { get; set; }
        public bool BillingPermission { get; set; }
        public bool RowLevelSecurity { get; set; } 

        public UserSession Session
        {
            get
            {
                return HttpContext.Session.GetString("USER_SESSION") == null ? null
                     : JsonConvert.DeserializeObject<UserSession>(HttpContext.Session.GetString("USER_SESSION"));
            }
        }

        public bool CanseeMemberTimesheet
        {
            get
            {
                return HttpContext.Session.GetInt32("canseemembertimesheet") == null || HttpContext.Session.GetInt32("canseemembertimesheet") == 0 ? false : true;
            }
        }

        public string RoleName
        {
            get
            {
                return HttpContext.Session.GetString("RoleName");
            }
        }
        public int? RequestCount
        {
            get
            {
                try
                {
                    ProvisionRequestcount().Wait(); // Wait for the asynchronous task to complete
                    return HttpContext.Session.GetInt32("RequestCount");
                }
                catch (Exception ex)
                {
                    // Handle or log the exception
                    return null; // Return null or a default value if an error occurs
                }
            }
        }

        public async Task ProvisionRequestcount()
        {
            string endpointUrl = $"{_baseConfig["ServiceUrl"]?.ToString()?.Trim()}/api/ContractBilling/approvallist";

            if (string.IsNullOrWhiteSpace(endpointUrl) || Session.Token == null)
            {
                return; // Handle null or empty values gracefully
            }

            try
            {
                var apiManager = new ApiManager(endpointUrl, Session.Token);
                var (statusCode, responseContent) = await apiManager.Get();

                if (statusCode == System.Net.HttpStatusCode.OK)
                {
                    var dataList = JsonConvert.DeserializeObject<List<ContractData>>(responseContent, dateTimeConverter);
                    HttpContext.Session.SetInt32("RequestCount", dataList.Count);
                }
                else
                {
                    HttpContext.Session.SetInt32("RequestCount", 0);
                }
            }
            catch (HttpRequestException ex)
            {
                // Handle HTTP request-related exceptions
                // Log ex.Message and ex.InnerException if available
            }
            catch (JsonException ex)
            {
                // Handle JSON deserialization exceptions
                // Log ex.Message and ex.InnerException if available
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                // Log ex.Message and ex.InnerException if available
            }
        }

        public override async void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Session == null)
            {
                filterContext.Result = new RedirectResult("~/Login/Logout");
                return;
            }

            var controllerName = ((ControllerBase)filterContext.Controller).ControllerContext.ActionDescriptor.ControllerName;
            var actionName = ((ControllerBase)filterContext.Controller).ControllerContext.ActionDescriptor.ActionName;
            var authPages = GetAuthorizationAsync().Result;




            var currentPage = authPages.Where(c=>c.Controller == controllerName && c.Action == "Index" || 
            c.Controller == controllerName && c.Action == actionName).FirstOrDefault();
            if(currentPage != null)
            {
                ReadPermission = currentPage.Isreadpermit ?? false;
                WritePermission = currentPage.Iswritepermit ?? false;
                DeletePermission = currentPage.Isdeletepermit ?? false;
                RowLevelSecurity = currentPage.Isrowlevelpermit ?? false;
                BillingPermission = currentPage.IsBillingpermit ?? false;
            }
            
            ViewBag.AuthPages = authPages;
            base.OnActionExecuting(filterContext);
        }

        public async Task<List<AuthorizePages>> GetAuthorizationAsync()
        {
            return await GetAuthorizationAsync(Session.EmpCode);
        }

        internal async Task<List<AuthorizePages>> GetAuthorizationAsync(string empCode)
        {
            List<AuthorizePages> authorizePages = new List<AuthorizePages>();
            apiManager = new ApiManager(_baseConfig["ServiceUrl"].ToString().Trim() + "/api/Authorize?empcode=" + empCode, Session.Token);
            var (statusCode, responseContent) = await apiManager.Get();

            if (statusCode == System.Net.HttpStatusCode.OK)
            {
                if (responseContent != null)
                {
                    var authorizeData = JsonConvert.DeserializeObject<AuthorizeData>(responseContent);
                   if (authorizeData != null)
                    {
                        var pages = (List<AuthorizePages>)authorizeData.AuthorizeRoles.SelectMany(c => c.AuthorizePages).Distinct().ToList();
                        authorizePages = pages.OrderBy(c => c.Pagesequence).ToList();
                        
                    }
                }
            }
            return authorizePages;
        }

        // List Common Methods 
        // customers
        internal async Task<List<CustomerWithDetailsDto>> GetCustomer()
        {
            var cust = new List<CustomerWithDetailsDto>();

            apiManager = new ApiManager(_baseConfig["ServiceUrl"].ToString().Trim() + "/api/Customer"); //--TOKEN
            var (statusCode, responseContent) = await apiManager.Get();

            if (statusCode == System.Net.HttpStatusCode.OK)
            {
                if (responseContent != null)
                {
                    cust = JsonConvert.DeserializeObject<List<CustomerWithDetailsDto>>(responseContent);
                }
            }
            return cust;

        }

        internal async Task<List<CustomerWithDetailsDto>> GetCustomerByDa(int DeliveryAnchorId)
        {
            var cust = new List<CustomerWithDetailsDto>();

            apiManager = new ApiManager(_baseConfig["ServiceUrl"].ToString().Trim() + $"/api/Customer/CustomerByDeliveryAnchor/{DeliveryAnchorId}"); //--TOKEN
            var (statusCode, responseContent) = await apiManager.Get();

            if (statusCode == System.Net.HttpStatusCode.OK)
            {
                if (responseContent != null)
                {
                    cust = JsonConvert.DeserializeObject<List<CustomerWithDetailsDto>>(responseContent);
                }
            }
            return cust;

        }

        //Employee
        internal async Task<List<Employees>> GetEmployee()
        {
            var emp = new List<Employees>();

            apiManager = new ApiManager(_baseConfig["ServiceUrl"].ToString().Trim() + "/api/Employee"); //--TOKEN
            var (statusCode, responseContent) = await apiManager.Get();

            if (statusCode == System.Net.HttpStatusCode.OK)
            {
                if (responseContent != null)
                {
                    emp = JsonConvert.DeserializeObject<List<Employees>>(responseContent);

                }
            }
            return emp;
        }
        internal async Task<List<DataForEmployeeReport>> GetEmployeeByNewProcedure()
        {
            var emp = new List<DataForEmployeeReport>();
            apiManager = new ApiManager(_baseConfig["ServiceUrl"] + "/api/Employee/EmployeeDetailsView/", Session.Token);
            var (statusCode, responseContent) = await apiManager.Get();

            if (statusCode == System.Net.HttpStatusCode.OK)
            {
                if (responseContent != null)
                {
                    emp = JsonConvert.DeserializeObject<List<DataForEmployeeReport>>(responseContent);

                }
            }
            return emp;
        }
        public async Task<List<Employees>> GetEmployeeByDa(int DeliveryAnchorId)
        {
            var emp = new List<Employees>();

            apiManager = new ApiManager(_baseConfig["ServiceUrl"].ToString().Trim() + $"/api/Employee/EmployeeByDeliveryAnchor/{DeliveryAnchorId}"); //--TOKEN
            var (statusCode, responseContent) = await apiManager.Get();

            if (statusCode == System.Net.HttpStatusCode.OK)
            {
                if (responseContent != null)
                {
                    emp = JsonConvert.DeserializeObject<List<Employees>>(responseContent);
                }
            }
            return emp;
        }
        public async Task<List<DataForEmployeeReport>> GetEmployeeByDaByProcedure(int DeliveryAnchorId)
        {
            var emp = new List<DataForEmployeeReport>();

            // API call to fetch DaList
            apiManager = new ApiManager(_baseConfig["ServiceUrl"].Trim() + "/api/Employee/DaList", Session.Token);
            var (statusCode, responseContent) = await apiManager.Get();

            if (statusCode == System.Net.HttpStatusCode.OK && responseContent != null)
            {
                try
                {
                    // Deserialize DaListDto
                    var danames = JsonConvert.DeserializeObject<List<DaListDto>>(responseContent);
                    if (danames == null || !danames.Any()) return emp;

                    // Extract DA names as a set for efficient lookup
                    var daNameSet = new HashSet<string>(danames.Select(d => d.DaName.Trim()), StringComparer.OrdinalIgnoreCase);

                    // API call to fetch EmployeeDetailsView
                    var apiManager = new ApiManager(_baseConfig["ServiceUrl"] + "/api/Employee/EmployeeDetailsView/" + DeliveryAnchorId, Session.Token);
                    var (code, content) = await apiManager.Get();

                    if (code == System.Net.HttpStatusCode.OK && content != null)
                    {
                        // Deserialize DataForEmployeeReport
                        emp = JsonConvert.DeserializeObject<List<DataForEmployeeReport>>(content);
                        if (emp == null || !emp.Any()) return emp;

                        //// Filter employees based on DA names
                        //var filteredEmp = emp
                        //    .Where(e =>
                        //        e.Da.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                        //        .Any(da => daNameSet.Contains(da)))
                        //    .ToList();

                        return emp;
                    }
                }
                catch (JsonException ex)
                {
                    // Log deserialization error
                    Console.WriteLine($"JSON Deserialization Error: {ex.Message}");
                }
            }

            return emp;
        }





        //EmployeeCount
        internal async Task<List<Employees>> GetEmployeesForCount()
        {
            var emp = new List<Employees>();

            apiManager = new ApiManager(_baseConfig["ServiceUrl"].ToString().Trim() + "/api/Employee/GetEmployeesForCount"); //--TOKEN
            var (statusCode, responseContent) = await apiManager.Get();

            if (statusCode == System.Net.HttpStatusCode.OK)
            {
                if (responseContent != null)
                {
                    emp = JsonConvert.DeserializeObject<List<Employees>>(responseContent);

                }
            }
            return emp;
        }

        //for dropdown
        internal async Task<List<Employees>> GetEmployeedropdown()
        {
            var emp = new List<Employees>();

            apiManager = new ApiManager(_baseConfig["ServiceUrl"].ToString().Trim() + "/api/Employee/Employees"); //--TOKEN
            var (statusCode, responseContent) = await apiManager.Get();

            if (statusCode == System.Net.HttpStatusCode.OK)
            {
                if (responseContent != null)
                {
                    emp = JsonConvert.DeserializeObject<List<Employees>>(responseContent);

                }
            }
            return emp;
        }

        //Employee
        internal async Task<List<EmployeeDetailDto>> GetEmployeeByProcedure()
        {
            var emp = new List<EmployeeDetailDto>();

            apiManager = new ApiManager(_baseConfig["ServiceUrl"].ToString().Trim() + "/api/Employee/GetEmployeeByProcedure"); //--TOKEN
            var (statusCode, responseContent) = await apiManager.Get();

            if (statusCode == System.Net.HttpStatusCode.OK)
            {
                if (responseContent != null)
                {
                    emp = JsonConvert.DeserializeObject<List<EmployeeDetailDto>>(responseContent);

                }
            }
            return emp;
        }




       






        public async Task<Employees> GetEmployee(int id = 0)
        {
            var emp = new Employees();
            apiManager = new ApiManager(_baseConfig["ServiceUrl"].ToString().Trim() + $"/api/Employee/{id}"); //--TOKEN
            var (statusCode, responseContent) = await apiManager.Get();
            if (statusCode == System.Net.HttpStatusCode.OK)
            {
                if (responseContent != null)
                    emp = JsonConvert.DeserializeObject<Employees>(responseContent);
            }
            return emp;
        }

        //skill
        public async Task<List<SelectListItem>> GetSkillList()
        {
            var items = new List<SelectListItem>();

            apiManager = new ApiManager(_baseConfig["ServiceUrl"].ToString().Trim() + "/api/Skill"); //--TOKEN
            var (statusCode, responseContent) = await apiManager.Get();

            if (statusCode == System.Net.HttpStatusCode.OK)
            {
                if (responseContent != null)
                {
                    List<Skill> type = JsonConvert.DeserializeObject<List<Skill>>(responseContent);

                    // Convert the Currency objects to SelectListItem
                    items = type.Select(c => new SelectListItem
                    {
                        Value = c.skillid.ToString(),
                        Text = c.skillname.ToString()
                    }).ToList();
                }
            }

            return items;
        }
        //Projects
        internal async Task<List<Projects>> GetProjects()
        {
            var projects = new List<Projects>();

            apiManager = new ApiManager(_baseConfig["ServiceUrl"].ToString().Trim() + "/api/Project");
            var (statusCode, responseContent) = await apiManager.Get();

            if (statusCode == System.Net.HttpStatusCode.OK)
            {
                if (responseContent != null)
                {
                    projects = JsonConvert.DeserializeObject<List<Projects>>(responseContent);

                }
            }
            return projects;

        }

        public async Task<List<Projects>> GetProjectByDa(int DeliveryAnchorId)
        {
            var projects = new List<Projects>();

            apiManager = new ApiManager(_baseConfig["ServiceUrl"].ToString().Trim() + $"/api/Project/ProjectListByDeliveryAnchor/{DeliveryAnchorId}");
            var (statusCode, responseContent) = await apiManager.Get();

            if (statusCode == System.Net.HttpStatusCode.OK)
            {
                if (responseContent != null)
                {
                    projects = JsonConvert.DeserializeObject<List<Projects>>(responseContent);

                }
            }
            return projects;

        }

        internal async Task<Projects> GetProjects(int id = 0)
        {
            var emp = new Projects();
            apiManager = new ApiManager(_baseConfig["ServiceUrl"].ToString().Trim() + $"/api/Project/{id}");
            var (statusCode, responseContent) = await apiManager.Get();
            if (statusCode == System.Net.HttpStatusCode.OK)
            {
                if (responseContent != null)
                    emp = JsonConvert.DeserializeObject<Projects>(responseContent);
            }
            return emp;
        }

        //Currency


        internal async Task<List<SelectListItem>> GetCurrency()
        {
            var items = new List<SelectListItem>();

            apiManager = new ApiManager(_baseConfig["ServiceUrl"].ToString().Trim() + "/api/Currency"); //--TOKEN
            var (statusCode, responseContent) = await apiManager.Get();

            if (statusCode == System.Net.HttpStatusCode.OK)
            {
                if (responseContent != null)
                {
                    List<Currency> currencies = JsonConvert.DeserializeObject<List<Currency>>(responseContent);

                    // Convert the Currency objects to SelectListItem
                    items = currencies.Select(c => new SelectListItem
                    {
                        Value = c.CurrencyId.ToString(),
                        Text = c.CurrencyName
                    }).ToList();
                }
            }

            return items;
        }
        // Country

        internal async Task<List<SelectListItem>> GetCountry()
        {
            var items = new List<SelectListItem>();

            apiManager = new ApiManager(_baseConfig["ServiceUrl"].ToString().Trim() + "/api/Country"); //--TOKEN
            var (statusCode, responseContent) = await apiManager.Get();

            if (statusCode == System.Net.HttpStatusCode.OK)
            {
                if (responseContent != null)
                {
                    List<Country> currencies = JsonConvert.DeserializeObject<List<Country>>(responseContent);

                    // Convert the Currency objects to SelectListItem
                    items = currencies.Select(c => new SelectListItem
                    {
                        Value = c.CountryId.ToString(),
                        Text = c.CountryName
                    }).ToList();
                }
            }

            return items;
        }
        // State

        public async Task<List<SelectListItem>> GetState(int countryId)
        {
            var items = new List<SelectListItem>();

            apiManager = new ApiManager(_baseConfig["ServiceUrl"].ToString().Trim() + "/api/State/GeStatesByCountryId?CountryId=" + countryId.ToString()); //--TOKEN

            //// Create an anonymous object with "obj" property and assign the countryId
            //var requestData = new { obj = countryId };

            var (statusCode, responseContent) = await apiManager.Get();

            if (statusCode == System.Net.HttpStatusCode.OK)
            {
                if (responseContent != null)
                {
                    List<State> state = JsonConvert.DeserializeObject<List<State>>(responseContent);

                    // Convert the state objects to SelectListItem
                    items = state.Select(c => new SelectListItem
                    {
                        Value = c.StateId.ToString(),
                        Text = c.StateName
                    }).ToList();
                }
            }

            return items;
        }

        // City
        public async Task<List<SelectListItem>> GetCity(int StateId)
        {
            var items = new List<SelectListItem>();

            apiManager = new ApiManager(_baseConfig["ServiceUrl"].ToString().Trim() + "/api/City/GetCitiesByStateId?StateId=" + StateId.ToString()); //--TOKEN

            //// Create an anonymous object with "obj" property and assign the countryId
            //var requestData = new { obj = countryId };

            var (statusCode, responseContent) = await apiManager.Get();

            if (statusCode == System.Net.HttpStatusCode.OK)
            {
                if (responseContent != null)
                {
                    List<City> city = JsonConvert.DeserializeObject<List<City>>(responseContent);

                    // Convert the state objects to SelectListItem
                    items = city.Select(c => new SelectListItem
                    {
                        Value = c.CityId.ToString(),
                        Text = c.CityName
                    }).ToList();
                }
            }

            return items;
        }


        // Payment Terms

        internal async Task<List<SelectListItem>> GetPaymentTerms()
        {
            var items = new List<SelectListItem>();

            apiManager = new ApiManager(_baseConfig["ServiceUrl"].ToString().Trim() + "/api/PaymentTerm"); //--TOKEN
            var (statusCode, responseContent) = await apiManager.Get();

            if (statusCode == System.Net.HttpStatusCode.OK)
            {
                if (responseContent != null)
                {
                    List<PaymentTerm> currencies = JsonConvert.DeserializeObject<List<PaymentTerm>>(responseContent);

                    // Convert the Currency objects to SelectListItem
                    items = currencies.Select(c => new SelectListItem
                    {
                        Value = c.PaymentTermId.ToString(),
                        Text = c.PaymentTermName
                    }).ToList();
                }
            }

            return items;
        }
        // ProjectModel

        internal async Task<List<SelectListItem>> GetProjectModel()
        {
            var items = new List<SelectListItem>();

            apiManager = new ApiManager(_baseConfig["ServiceUrl"].ToString().Trim() + "/api/ProjectModel"); //--TOKEN
            var (statusCode, responseContent) = await apiManager.Get();

            if (statusCode == System.Net.HttpStatusCode.OK)
            {
                if (responseContent != null)
                {
                    List<Projectmodel> projectmodels = JsonConvert.DeserializeObject<List<Projectmodel>>(responseContent);

                    // Convert the Currency objects to SelectListItem
                    items = projectmodels.Select(c => new SelectListItem
                    {
                        Value = c.Projectmodelid.ToString(),
                        Text = c.Projectmodelname.ToString()
                    }).ToList();
                }
            }

            return items;
        }
        // ProjecType

        internal async Task<List<SelectListItem>> GetProjecType()
        {
            var items = new List<SelectListItem>();

            apiManager = new ApiManager(_baseConfig["ServiceUrl"].ToString().Trim() + "/api/ProjectType"); //--TOKEN
            var (statusCode, responseContent) = await apiManager.Get();

            if (statusCode == System.Net.HttpStatusCode.OK)
            {
                if (responseContent != null)
                {
                    List<Projecttype> projecttypes = JsonConvert.DeserializeObject<List<Projecttype>>(responseContent);

                    // Convert the Currency objects to SelectListItem
                    items = projecttypes.Select(c => new SelectListItem
                    {
                        Value = c.Projecttypeid.ToString(),
                        Text = c.Projecttypename.ToString()
                    }).ToList();
                }
            }

            return items;
        }
        // Practice

        internal async Task<List<SelectListItem>> GetPractice()
        {
            var items = new List<SelectListItem>();

            apiManager = new ApiManager(_baseConfig["ServiceUrl"].ToString().Trim() + "/api/Practice"); //--TOKEN
            var (statusCode, responseContent) = await apiManager.Get();

            if (statusCode == System.Net.HttpStatusCode.OK)
            {
                if (responseContent != null)
                {
                    List<Practice> practices = JsonConvert.DeserializeObject<List<Practice>>(responseContent);

                    // Convert the Currency objects to SelectListItem
                    items = practices.Select(c => new SelectListItem
                    {
                        Value = c.Practiceid.ToString(),
                        Text = c.Practicename.ToString()
                    }).ToList();
                }
            }

            return items;
        }
        // SubPractice

        internal async Task<List<SelectListItem>> GetSubPractice()
        {
            var items = new List<SelectListItem>();

            apiManager = new ApiManager(_baseConfig["ServiceUrl"].ToString().Trim() + "/api/SubPractice"); //--TOKEN
            var (statusCode, responseContent) = await apiManager.Get();

            if (statusCode == System.Net.HttpStatusCode.OK)
            {
                if (responseContent != null)
                {
                    List<Subpractice> subpractices = JsonConvert.DeserializeObject<List<Subpractice>>(responseContent);

                    // Convert the Currency objects to SelectListItem
                    items = subpractices.Select(c => new SelectListItem
                    {
                        Value = c.Subpracticeid.ToString(),
                        Text = c.Subpracticename.ToString()
                    }).ToList();
                }
            }

            return items;
        }
        // SubPractice

        public async Task<List<SelectListItem>> GetSubPracticeByPracticeId(int practiceId)
        {
            var items = new List<SelectListItem>();

            apiManager = new ApiManager(_baseConfig["ServiceUrl"].ToString().Trim() + "/api/SubPractice/GetSubPracticeByPracticeId?PracticeId=" + practiceId.ToString()); //--TOKEN
            var (statusCode, responseContent) = await apiManager.Get();

            if (statusCode == System.Net.HttpStatusCode.OK)
            {
                if (responseContent != null)
                {
                    List<Subpractice> subpractices = JsonConvert.DeserializeObject<List<Subpractice>>(responseContent);

                    // Convert the Currency objects to SelectListItem
                    items = subpractices.Select(c => new SelectListItem
                    {
                        Value = c.Subpracticeid.ToString(),
                        Text = c.Subpracticename.ToString()
                    }).ToList();
                }
            }

            return items;
        }

        // customer

        internal async Task<List<SelectListItem>> Customerslist()
        {
            var items = new List<SelectListItem>();

            apiManager = new ApiManager(_baseConfig["ServiceUrl"].ToString().Trim() + "/api/Customer"); //--TOKEN
            var (statusCode, responseContent) = await apiManager.Get();

            if (statusCode == System.Net.HttpStatusCode.OK)
            {
                if (responseContent != null)
                {
                    List<CustomerWithDetailsDto> currencies = JsonConvert.DeserializeObject<List<CustomerWithDetailsDto>>(responseContent);

                    // Convert the Currency objects to SelectListItem
                    items = currencies.Select(c => new SelectListItem
                    {
                        Value = c.CustomerId.ToString(),
                        Text = c.CustomerCompanyName.ToString()
                    }).ToList();
                }
            }

            return items;
        }
        //project by customer

        public async Task<JsonResult> GetProjectAndPracticeByCustomerId(int Customerid)
        {
            apiManager = new ApiManager(_baseConfig["ServiceUrl"].ToString().Trim() + "/api/Project/ProjectListByCustomerId/" + Customerid.ToString()); // --TOKEN
            var (statusCode, responseContent) = await apiManager.Get();

            if (statusCode == System.Net.HttpStatusCode.OK)
            {
                if (!string.IsNullOrEmpty(responseContent))
                {
                    var responseData = JsonConvert.DeserializeObject<List<Projects>>(responseContent);
                    return Json(responseData);
                }
            }

            return Json(new { error = "Failed to fetch data" }, HttpStatusCode.InternalServerError);
        }

        //internal async Task<List<SelectListItem>> GetCategorySubStatus()
        //{
        //    var items = new List<SelectListItem>();

        //    apiManager = new ApiManager(_baseConfig["ServiceUrl"].ToString().Trim() + "/api/CategorySubStatus"); //--TOKEN
        //    var (statusCode, responseContent) = await apiManager.Get();
        //    if (statusCode == System.Net.HttpStatusCode.OK)
        //    {
        //        if (responseContent != null)
        //        {
        //            List<CategorySubStatus> practices = JsonConvert.DeserializeObject<List<CategorySubStatus>>(responseContent);
        //            // Convert the Currency objects to SelectListItem
        //            items = practices.Select(c => new SelectListItem
        //            {
        //                Value = c.categorysubstatusid.ToString(),
        //                Text = c.categorysubstatusname.ToString()
        //            }).ToList();
        //        }
        //    }

        //    return items;
        //}
        public async Task<List<SelectListItem>> GetCategorySubStatus()
        {
            var items = new List<SelectListItem>();

            apiManager = new ApiManager(_baseConfig["ServiceUrl"].ToString().Trim() + "/api/CategorySubStatus"); //--TOKEN
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
                        c.categorysubstatusname == "Bench" ||
                        c.categorysubstatusname == "Shadow"
                    );

                    // Convert the filtered category substatuses to SelectListItem
                    items = filteredCategorySubstatuses.Select(c => new SelectListItem
                    {
                        Value = c.categorysubstatusid.ToString(),
                        Text = c.categorysubstatusname.ToString()
                    }).ToList();
                }
            }

            return items;
        }

        // For Costing Sheet Drop down 
#region costsheetdropdown
        public class CustomSelectListItem
        {
            public string Value { get; set; }
            public string Text { get; set; }
            public decimal XValue { get; set; }
            public decimal? TotalAmount { get; set; }

        }

        public async Task<List<CustomSelectListItem>> GetExistingCostingSheet()
        {
            var items = new List<CustomSelectListItem>();

            apiManager = new ApiManager(_baseConfig["ServiceUrl"].ToString().Trim() + "/api/CostSheet"); //--TOKEN
            var (statusCode, responseContent) = await apiManager.Get();

            if (statusCode == System.Net.HttpStatusCode.OK)
            {
                if (responseContent != null)
                {
                    List<CostSheet> costingsheet = JsonConvert.DeserializeObject<List<CostSheet>>(responseContent);

                    items = costingsheet.Select(c => new CustomSelectListItem
                    {
                        Value = c.costsheetid.ToString(),
                        Text = c.costsheetname.ToString(),
                        XValue = c.AverageXValue,
                        TotalAmount = c.TotalAmount

                    }).ToList();
                }
            }

            return items;
        }
        #endregion
        public async Task<JsonResult> GetLinks()
        {
            string jsonlinks = string.Empty;
            try
            {
                List<SearchLink> links = new List<SearchLink>();
                var AuthPages = await GetAuthorizationAsync(Session.EmpCode);
                if (AuthPages != null)
                {
                    foreach (var pg in AuthPages)
                        links.Add(new SearchLink
                        {
                            label = pg.PageName,
                            the_link = Url.Action(pg.Action, pg.Controller, null, HttpContext.Request.Scheme),
                            controller = pg.Controller,
                            action = pg.Action
                        });
                }

                jsonlinks = JsonConvert.SerializeObject(links, Formatting.Indented);
            }
            catch (Exception)
            {
            }
            return Json(jsonlinks);
        }
        internal async Task<List<SelectListItem>> ContractTypeList()
        {
            var items = new List<SelectListItem>();

            apiManager = new ApiManager(_baseConfig["ServiceUrl"].ToString().Trim() + "/api/ContractType"); //--TOKEN
            var (statusCode, responseContent) = await apiManager.Get();

            if (statusCode == System.Net.HttpStatusCode.OK)
            {
                if (responseContent != null)
                {
                    List<ContractType> type = JsonConvert.DeserializeObject<List<ContractType>>(responseContent);

                    // Convert the Currency objects to SelectListItem
                    items = type.Select(c => new SelectListItem
                    {
                        Value = c.ContractTypeId.ToString(),
                        Text = c.ContractTypeName.ToString()
                    }).ToList();
                }
            }

            return items;
        }
        public async Task<JsonResult> GetSkill()
        {
            var costingsheet = new List<Skill>();

            apiManager = new ApiManager(_baseConfig["ServiceUrl"].ToString().Trim() + "/api/Skill"); //--TOKEN
            var (statusCode, responseContent) = await apiManager.Get();

            if (statusCode == System.Net.HttpStatusCode.OK)
            {
                if (responseContent != null)
                {
                    costingsheet = JsonConvert.DeserializeObject<List<Skill>>(responseContent);

                    return Json(costingsheet);
                }
                return Json(costingsheet);
            }

            return Json(new { error = "Failed to fetch data" }, HttpStatusCode.InternalServerError);
        }
        internal async Task<List<SelectListItem>> GetDepartment()
        {
            var items = new List<SelectListItem>();

            apiManager = new ApiManager(_baseConfig["ServiceUrl"].ToString().Trim() + "/api/Department"); //--TOKEN
            var (statusCode, responseContent) = await apiManager.Get();

            if (statusCode == System.Net.HttpStatusCode.OK)
            {
                if (responseContent != null)
                {
                    List<Department> practices = JsonConvert.DeserializeObject<List<Department>>(responseContent);

                    // Convert the Currency objects to SelectListItem
                    items = practices.Select(c => new SelectListItem
                    {
                        Value = c.departmentid.ToString(),
                        Text = c.departmentname.ToString()
                    }).ToList();
                }
            }

            return items;
        }

        internal async Task<List<SelectListItem>> GetBranch()
        {
            var items = new List<SelectListItem>();

            apiManager = new ApiManager(_baseConfig["ServiceUrl"].ToString().Trim() + "/api/Branch"); //--TOKEN
            var (statusCode, responseContent) = await apiManager.Get();

            if (statusCode == System.Net.HttpStatusCode.OK)
            {
                if (responseContent != null)
                {
                    List<Branch> practices = JsonConvert.DeserializeObject<List<Branch>>(responseContent);

                   
                    items = practices.Select(c => new SelectListItem
                    {
                        Value = c.branchid.ToString(),
                        Text = c.branchname.ToString()
                    }).ToList();
                }
            }

            return items;
        }
        internal async Task<List<SelectListItem>> GetDesignation()
        {
            var items = new List<SelectListItem>();

            apiManager = new ApiManager(_baseConfig["ServiceUrl"].ToString().Trim() + "/api/Designation"); //--TOKEN
            var (statusCode, responseContent) = await apiManager.Get();

            if (statusCode == System.Net.HttpStatusCode.OK)
            {
                if (responseContent != null)
                {
                    List<Designation> practices = JsonConvert.DeserializeObject<List<Designation>>(responseContent);


                    items = practices.Select(c => new SelectListItem
                    {
                        Value = c.designationid.ToString(),
                        Text = c.designationname.ToString()
                    }).ToList();
                }
            }

            return items;
        }
        //for time sheet
        internal async Task<List<SelectListItem>> GetProjectForTimesheet()
        {
            var items = new List<SelectListItem>();

            apiManager = new ApiManager(_baseConfig["ServiceUrl"].ToString().Trim() + $"/api/Timesheet/GetProjectForTimesheet/{Session.EmployeeId}"); //--TOKEN
            var (statusCode, responseContent) = await apiManager.Get();

            if (statusCode == System.Net.HttpStatusCode.OK)
            {
                if (responseContent != null)
                {
                    List<Projects> practices = JsonConvert.DeserializeObject<List<Projects>>(responseContent);

                    // Convert the Currency objects to SelectListItem
                    items = practices.Select(c => new SelectListItem
                    {
                        Value = c.ProjectId.ToString(),
                        Text = c.ProjectName.ToString()
                    }).ToList();
                }
            }

            return items;
        }

        internal async Task<List<SelectListItem>> GetAccountManager()
        {
            var items = new List<SelectListItem>();

            apiManager = new ApiManager(_baseConfig["ServiceUrl"].ToString().Trim() + "/api/AccountManager"); //--TOKEN
            var (statusCode, responseContent) = await apiManager.Get();

            if (statusCode == System.Net.HttpStatusCode.OK)
            {
                if (responseContent != null)
                {
                    List<AccountManager> accountmanagers = JsonConvert.DeserializeObject<List<AccountManager>>(responseContent);

                    // Convert the Currency objects to SelectListItem
                    items = accountmanagers.Select(c => new SelectListItem
                    {
                        Value = c.accountManagerId.ToString(),
                        Text = c.accountManagerName.ToString()
                    }).ToList();
                }
            }

            return items;
        }
        //da dropdown

        internal async Task<List<SelectListItem>> GetDA()
        {
            var items = new List<SelectListItem>();

            apiManager = new ApiManager(_baseConfig["ServiceUrl"].ToString().Trim() + "/api/AccountManager/DADropDown"); //--TOKEN
            var (statusCode, responseContent) = await apiManager.Get();

            if (statusCode == System.Net.HttpStatusCode.OK)
            {
                if (responseContent != null)
                {
                    List<EmployeeDetailDto> currencies = JsonConvert.DeserializeObject<List<EmployeeDetailDto>>(responseContent);

                    // Convert the Currency objects to SelectListItem
                    items = currencies.Select(c => new SelectListItem
                    {
                        Value = c.EmployeeId.ToString(),
                        Text = c.EmployeeName
                    }).ToList();
                }
            }

            return items;
        }

        //Get Employee Skills Method
        public async Task<List<SelectListItem>> GetEmployeeBySkills(int skillId)
        {
            var items = new List<SelectListItem>();

            apiManager = new ApiManager(_baseConfig["ServiceUrl"].ToString().Trim() + "/api/Employee/GetEmployeebyskill/" + skillId); //--TOKEN

            var (statusCode, responseContent) = await apiManager.Get();

            if (statusCode == System.Net.HttpStatusCode.OK)
            {
                if (responseContent != null)
                {
                    List<Employees> city = JsonConvert.DeserializeObject<List<Employees>>(responseContent);

                    // Convert the state objects to SelectListItem
                    items = city.Select(c => new SelectListItem
                    {
                        Value = c.Employeeid.ToString(),
                        Text = $"{c.Employeename} ({c.TmcId})"
                    }).ToList();
                }
            }

            return items;
        }
        public async Task<CostSheet> GetCostSheet(int id)
        {
            var costSheet = new CostSheet();

            apiManager = new ApiManager(_baseConfig["ServiceUrl"].ToString().Trim() + "/api/CostSheet/" + id); //--TOKEN
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
        #region timesheet

        public async Task<List<SelectListItem>> GetProjectsByDA(int DeliveryAnchorId)
        {
            var items = new List<SelectListItem>();

            apiManager = new ApiManager(_baseConfig["ServiceUrl"].ToString().Trim() + $"/api/Project/ProjectListByDeliveryAnchor/{DeliveryAnchorId}");
            var (statusCode, responseContent) = await apiManager.Get();

            if (statusCode == System.Net.HttpStatusCode.OK)
            {
                if (responseContent != null)
                {
                    var projects = JsonConvert.DeserializeObject<List<Projects>>(responseContent);
                    items = projects.Select(c => new SelectListItem
                    {
                        Value = c.ProjectId.ToString(),
                        Text = c.ProjectName.ToString()
                    }).ToList();

                }
            }
            return items;
        }

        public async Task<List<SelectListItem>> GetResourceByDA(int DeliveryAnchorId)
        {
            var items = new List<SelectListItem>();


            apiManager = new ApiManager(_baseConfig["ServiceUrl"].ToString().Trim() + $"/api/Employee/EmployeeByDeliveryAnchor/{DeliveryAnchorId}"); //--TOKEN
            var (statusCode, responseContent) = await apiManager.Get();

            if (statusCode == System.Net.HttpStatusCode.OK)
            {
                if (responseContent != null)
                {
                    var emp = JsonConvert.DeserializeObject<List<Employees>>(responseContent);
                    items = emp.Select(c => new SelectListItem
                    {
                        Value = c.Employeeid.ToString(),
                        Text = c.Employeename.ToString()
                    }).ToList();
                }
            }
            return items;
        }

        public async Task<List<SelectListItem>> GetCategoryOfActivityForTimesheet()
        {
            var items = new List<SelectListItem>();

            apiManager = new ApiManager(_baseConfig["ServiceUrl"].ToString().Trim() + $"/api/Timesheet/categoryofactivity/");
            var (statusCode, responseContent) = await apiManager.Get();

            if (statusCode == System.Net.HttpStatusCode.OK)
            {
                if (responseContent != null)
                {
                    // Parse the response dynamically
                    var data = JObject.Parse(responseContent)["dataObject"];

                    // Convert the dynamic objects to SelectListItem
                    items = data.Select(c => new SelectListItem
                    {
                        Value = c["categoryofactivityid"].ToString(),
                        Text = c["categoryofactivityname"].ToString()
                    }).ToList();
                }
            }

            return items;
        }


        #endregion

        //mailing
        public bool SendMail(string username, string sMailSubject, string sMailBody, List<string> ccemails, out string sMailOutput, string attachement = null)
        {
            try
            {
                string strSenderEmail = _baseConfig["MailCredential:Email"] ?? "";
                string strSenderPassword = _baseConfig["MailCredential:Password"] ?? "";
                int strSenderPort = int.Parse(_baseConfig["MailCredential:Port"] ?? "587");
                string strSenderHost = _baseConfig["MailCredential:Host"] ?? "";
                bool lEnableSsl = bool.Parse(_baseConfig["MailCredential:EnableSsl"] ?? "true");

                Mail mail = new Mail(strSenderEmail, strSenderPassword, strSenderPort, strSenderHost, lEnableSsl);

                mail.SendMail(username.Split(",").ToList<string>(), sMailSubject, sMailBody, out sMailOutput, ccemails, attachement);
                return true;
            }
            catch (Exception ex)
            {
                sMailOutput = ex.Message;
                return false;
            }
        }

    }


}
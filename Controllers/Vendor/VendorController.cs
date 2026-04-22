using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using RMS.Client.Models.Customer;
using RMS.Client.Models.Employee;
using RMS.Client.Models.Oaf;
using RMS.Client.Models.Vendor;
using RMS.Client.Utility;
using System.Globalization;

namespace RMS.Client.Controllers.Vendor
{
    public class VendorController : BaseController
    {
        private ApiManager apiManager;
        private IConfiguration _configuration;
        public VendorController(IConfiguration configuration, IWebHostEnvironment env)
              : base(configuration, env)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var cust = new List<VendorWithDetailDto>();

            apiManager = new ApiManager(_configuration["ServiceUrl"].ToString().Trim() + "/api/Vendor");

            var (statusCode, responseContent) = await apiManager.Get();

            if (statusCode == System.Net.HttpStatusCode.OK)
            {
                if (responseContent != null)
                {
                    cust = JsonConvert.DeserializeObject<List<VendorWithDetailDto>>(responseContent);

                    // Calculate and set the total count of active employees for each VendorWithDetailDto
                    for (int i = 0; i < cust.Count; i++)
                    {
                        var totalCount = cust[i].rmsemployees.Count(r => r.isactive == true && cust[i].vendorid == r.Udf2);
                        cust[i].TotalEmployee = totalCount;
                    }
                }
            }

            ViewData["controller"] = this.ControllerContext.RouteData.Values["controller"].ToString();
            return View(cust);
        }


        [HttpGet]
        public async Task<IActionResult> Create(int id)
        {
            //if (!WritePermission)
            //    return RedirectToAction("NotAuthorized", "Home");
            Vendors vendordata = new Vendors();
            try
            {
                ViewBag.CurrencyList = GetCurrency().Result;
                ViewBag.CountryList = await GetCountry();
                ViewBag.PaymentTermList = await GetPaymentTerms();
                ViewData["controller"] = this.ControllerContext.RouteData.Values["controller"].ToString();
                if (id != 0)
                {
                    apiManager = new ApiManager(_configuration["ServiceUrl"].ToString().Trim() + "/api/Vendor/" + id.ToString()); //--TOKEN
                    var (statusCode, responseContent) = await apiManager.Get();
                    if (statusCode == System.Net.HttpStatusCode.OK)
                    {
                        VendorWithDetailDto data = JsonConvert.DeserializeObject<VendorWithDetailDto>(responseContent);
                        vendordata.vendorid = data.vendorid;
                        vendordata.VendorCode = data.vendorcode;
                        vendordata.VendorName = data.vendorname;
                        vendordata.Vendorcontact = data.vendorcontact;
                        vendordata.Address1 = data.address1;
                        vendordata.Address2 = data.address2;
                        vendordata.Phone1 = data.phone1;
                        vendordata.Phone2 = data.phone2;
                        vendordata.Email = data.email;
                        vendordata.Gstnumber = data.gstnumber;
                        vendordata.Pannumber = data.pannumber;
                        vendordata.Zipcode = data.zipcode;
                        vendordata.Cityid = data.city?.cityId ?? 0;
                        vendordata.Paymenttermid = data.paymentTerm?.paymentTermId ?? 0;
                        vendordata.stateId = data.city.state?.stateId ?? 0;
                        vendordata.countryId = data.city?.state?.country?.countryId ?? 0;
                        vendordata.Currencyid = data.currency?.currencyId ?? 0;
                    }
                }

                return View(vendordata);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred: " + ex.Message;
                return View();
            }

        }

        [HttpPost]
        public async Task<IActionResult> Upsert(Vendors vendor)
        {
            //if (!WritePermission)
            //    return RedirectToAction("NotAuthorized", "Home");
            try
            {
                if (vendor == null)
                {
                    ViewData["Error"] = "Vendor data is empty";
                }
                var baseApiUrl = _configuration["ServiceUrl"].ToString().Trim();
                IFormFileCollection images = HttpContext.Request.Form.Files;
                var apiURL = (vendor.VendorId != 0) ? $"{baseApiUrl}/api/Vendor/{vendor.VendorId}" : $"{baseApiUrl}/api/Vendor";
                apiManager = new ApiManager(apiURL, Session.Token);
                System.Net.HttpStatusCode statusCode;
                string responseContent;
                if (vendor.VendorId != 0)
                {
                    (statusCode, responseContent) = await apiManager.PutWithFiles(System.Text.Json.JsonSerializer.Serialize(vendor), images);
                }
                else
                {
                    (statusCode, responseContent) = await apiManager.PostWithFiles(System.Text.Json.JsonSerializer.Serialize(vendor), images);
                }
                if (statusCode == System.Net.HttpStatusCode.OK)
                {
                    TempData["success"] = (vendor.VendorId != 0) ? "Vendor Updated Successfully !!" : "Vendor Submitted Successfully !!";
                }
                else
                {
                    TempData["error"] = "Vendor Code Already Exist Please Use Unique Code!!";
                }

            }
            catch (Exception ex)
            {
                ViewData["error"] = $"An unexpected error occurred: {ex.Message}";

            }
            return RedirectToAction("Index", "Vendor");
        }
        [HttpGet]
        public async Task<IActionResult> View(int id)
        {
            ViewData["controller"] = this.ControllerContext.RouteData.Values["controller"].ToString();
            VendorWithDetailDto vendor = new VendorWithDetailDto();
            try
            {
                if (id != 0)
                {
                    apiManager = new ApiManager(_configuration["ServiceUrl"].ToString().Trim() + "/api/Vendor/" + id.ToString()); //--TOKEN
                    var (statusCode, ResponseContent) = await apiManager.Get();

                    if (statusCode == System.Net.HttpStatusCode.OK)
                    {
                        vendor = JsonConvert.DeserializeObject<VendorWithDetailDto>(ResponseContent);

                    }
                    else
                    {
                        ViewData["error"] = "Error occurred while processing your request.";
                    }
                }
            }
            catch (Exception ex)
            {
                ViewData["error"] = $"An unexpected error occurred: " + ex.Message;
            }
            return View(vendor);
        }

        [HttpGet]
        public async Task<IActionResult> AddVendorEmployee(int id, int vendorid)
        {
            ViewData["controller"] = this.ControllerContext.RouteData.Values["controller"].ToString();
            EmployeeDt vendors = new EmployeeDt();
            VendorWithDetailDto data = new VendorWithDetailDto();
            try
            {
                ViewBag.PracticeList = await GetPractice();
                ViewBag.SubpracticeList = await GetSubPractice();
                ViewBag.Department = await GetDepartment();
                ViewBag.Branch = await GetBranch();
                ViewBag.Designation = await GetDesignation();
                if (id != 0)
                {
                    apiManager = new ApiManager(_configuration["ServiceUrl"].ToString().Trim() + "/api/Vendor/EmployeeGetById/" + id.ToString()); //--TOKEN
                    var (statusCode, responseContent) = await apiManager.Get();
                    vendors = JsonConvert.DeserializeObject<EmployeeDt>(responseContent);
                    ViewBag.Vendorname = vendors.vendor.VendorName;
                    ViewBag.Vendorid = vendors.vendor.vendorid;

                }
                if (vendorid > 0) {
                    apiManager = new ApiManager(_configuration["ServiceUrl"].ToString().Trim() + "/api/Vendor/" + vendorid.ToString()); //--TOKEN
                    var (statusCode, responseContent) = await apiManager.Get();
                    data = JsonConvert.DeserializeObject<VendorWithDetailDto>(responseContent);
                    ViewBag.Vendorname = data.vendorname;
                    ViewBag.Vendorid = data.vendorid;

                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred: " + ex.Message;
                return View();
            }
            return View(vendors);
        }
        [HttpPost]
        public async Task<IActionResult> AddVendorEmployee(EmployeeDt vendor)
        {
            if (vendor == null)
            {
                TempData["Error"] = "Vendor data is empty";
                return RedirectToAction("View", "Vendor", new { id = vendor.VendorId });
            }
            try
            {
                vendor.departmentid = 22;
                string dateOfJoiningString = vendor.dateofjoining; // Replace with your actual property

                DateTime dateOfJoining;
                if (DateTime.TryParseExact(dateOfJoiningString, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateOfJoining))
                {
                    string formattedDate = dateOfJoining.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture);

                    vendor.dateofjoining = formattedDate;
                }
                vendor.Udf2 = vendor.VendorId;
                var baseApiUrl = _configuration["ServiceUrl"].ToString().Trim();
                IFormFileCollection images = HttpContext.Request.Form.Files;
                var apiURL = $"{baseApiUrl}/api/Vendor/AddVendorEmployee/{(vendor.employeeid != 0 ? vendor.employeeid : "")}";
                apiManager = new ApiManager(apiURL, Session.Token);

                var (statusCode, responseContent) = vendor.employeeid > 0
                    ? await apiManager.PutWithFiles(System.Text.Json.JsonSerializer.Serialize(vendor), images)
                    : await apiManager.PostWithFiles(System.Text.Json.JsonSerializer.Serialize(vendor), images);

                TempData[statusCode == System.Net.HttpStatusCode.OK ? "success" : "error"] =
                    statusCode == System.Net.HttpStatusCode.OK
                        ? (vendor.VendorId != 0 ? "Employee Updated Successfully !!" : "Employee Submitted Successfully !!")
                        : "Error occurred while processing your request.";
            }
            catch (Exception ex)
            {
                ViewData["error"] = $"An error has occurred: {ex.Message}";
            }

            return RedirectToAction("View", "Vendor", new { id = vendor.VendorId });
        }

        public async Task<IActionResult> DeleteVendorEmployee(int id)
        {
            EmployeeDt vendors = new EmployeeDt();
           
            if (id == 0 || id < 0)
            {
                ViewData["Error"] = "Vendor data is empty";
                return RedirectToAction("Index", "Vendor");
            }
            try
            {
                if (id != 0)
                {
                    apiManager = new ApiManager(_configuration["ServiceUrl"].ToString().Trim() + "/api/Vendor/EmployeeGetById/" + id.ToString()); //--TOKEN
                    var (statusCode, responseContent) = await apiManager.Get();
                    vendors = JsonConvert.DeserializeObject<EmployeeDt>(responseContent);
                   
                }
                if (id > 0)
                {
                    apiManager = new ApiManager(_configuration["ServiceUrl"].ToString().Trim() + "/api/Vendor/DeleteVendorEmployee/" + id.ToString());
                    var (statusCode, responseCode) = await apiManager.Delete();
                    if (statusCode == System.Net.HttpStatusCode.OK)
                    {
                        TempData["success"] = "Resource Deleted!!";
                        return RedirectToAction("View", "Vendor", new { id = vendors.vendor.vendorid});
                    }

                }
                return View();
            }
            catch (Exception ex)
            {
                ViewData["Error"] = $"An error has occurred: {ex.Message}";
            }
            return View();
        }

        public async Task<IActionResult> DeleteVendor(int id)
        {
            if (id <= 0)
            {
                TempData["error"] = "Invalid vendor ID";
                return RedirectToAction("Index", "Vendor");
            }

            try
            {
                apiManager = new ApiManager(_configuration["ServiceUrl"].ToString().Trim() + $"/api/Vendor/{id}"); //--TOKEN
                var (statusCodes, ResponseContent) = await apiManager.Get();

                if (statusCodes == System.Net.HttpStatusCode.OK)
                {
                    var vendor = JsonConvert.DeserializeObject<VendorWithDetailDto>(ResponseContent);
                    if (vendor.employees != null)
                    {
                        var data = vendor.employees.Count();
                        if (data > 0 && vendor.employees !=null)
                        {
                            TempData["error"] = "Vendor cannot be deleted because it contains active resources!";
                            return RedirectToAction("Index", "Vendor");
                        }
                    }
                    var (statusCode, responseCode) = await apiManager.Delete();
                    if (statusCode == System.Net.HttpStatusCode.OK)
                    {
                        TempData["success"] = "Vendor deleted successfully!";
                        return RedirectToAction("Index", "Vendor");
                    }
                    else
                    {
                        ViewData["error"] = "Failed to delete vendor";
                    }
                }
                else
                {
                    ViewData["error"] = "Failed to fetch vendor data";
                }
            }
            catch (Exception ex)
            {
                ViewData["error"] = $"An error has occurred: {ex.Message}";
            }

            return View();
        }



    }
}

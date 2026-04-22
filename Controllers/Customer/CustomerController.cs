using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RMS.Client.Models;
using RMS.Client.Models.Customer;
using RMS.Client.Models.Master;
using RMS.Client.Utility;
using System;
using System.Text.Json.Serialization;
using static System.Collections.Specialized.BitVector32;

namespace RMS.Client.Controllers.Customer
{
    public class CustomerController : BaseController
    {
        private ApiManager apiManager;
        private IConfiguration _configuration;
        public CustomerController(IConfiguration configuration, IWebHostEnvironment env)
              : base(configuration, env)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (!ReadPermission)
                return RedirectToAction("NotAuthorized", "Home");

            var customerList = RowLevelSecurity ?  await GetCustomer() : await GetCustomer();

            ViewData["controller"] = this.ControllerContext.RouteData.Values["controller"].ToString();
            return View(customerList);
        }
       // GetCustomerByDa(Session.EmployeeId)

        [HttpGet]
        public async Task<IActionResult> Create(int customerid)
        {
            if (Session.RoleName != "DA")
            {
                if (!WritePermission)
                    return RedirectToAction("NotAuthorized", "Home");
            }

            Customers customerData = new Customers(); 
            try
            {
                if (customerid != 0)
                {
                    apiManager = new ApiManager(_configuration["ServiceUrl"].ToString().Trim() + "/api/Customer/" + customerid.ToString()); //--TOKEN
                    var (statusCode, responseContent) = await apiManager.Get();
                    if (statusCode == System.Net.HttpStatusCode.OK)
                    {
                        CustomerWithDetailsDto data = JsonConvert.DeserializeObject<CustomerWithDetailsDto>(responseContent);


                        customerData.Customerid = data.CustomerId;
                        customerData.Firstname = data.CustomerFirstName;
                        customerData.Lastname = data.CustomerLastName;
                        customerData.Companyname = data.CustomerCompanyName;
                        customerData.Companyemail = data.CustomerEmail;
                        customerData.CustomerPhone2 = data.CustomerPhone2;
                        customerData.CustomerPhone1 = data.CustomerPhone1;
                        customerData.Pannumber = data.PAN;
                        customerData.Currencyid = data.Currency?.CurrencyId ?? 0;
                        customerData.Gstnumber = data.GST;
                        customerData.Paymenttermid = data.PaymentTerm?.PaymentTermId ?? 0;
                        customerData.Cityid = data.City?.CityId ?? 0;
                        customerData.Zipcode = data.ZipCode;
                        customerData.CustomerAddress1 = data.customerAddress1;
                        customerData.Companylogourl = data.customerCompanyLogoUrl;
                        customerData.Stateid = data.City?.State?.StateId ?? 0;
                        customerData.Countryid = data.City?.State?.Country?.CountryId ?? 0;
                        customerData.CustomerAddress2 = data.customerAddress2;
                        customerData.Customercode = data.Customercode?.ToString() ?? "";

                    }
                }

                ViewBag.CurrencyList = GetCurrency().Result;
                ViewBag.CountryList = await GetCountry();
                ViewBag.PaymentTermList = await GetPaymentTerms();
                ViewData["controller"] = this.ControllerContext.RouteData.Values["controller"].ToString();
                return View(customerData);
            }
            catch (Exception ex)
            {
               
                ViewBag.ErrorMessage = "An error occurred: " + ex.Message;
                return View(); 
            }
        }


        [HttpPost]
        public async Task<IActionResult> UpsertAsync(Customers customer)
        {
            //if (Session.RoleName != "DA")
            //{
            //    if (!WritePermission)
            //        return RedirectToAction("NotAuthorized", "Home");
            //}

            const string redirectAction = "Index";
            const string redirectController = "Customer";

            try
            {
                if (customer == null)
                {
                    ViewData["error"] = "Customer data is empty.";
                    return RedirectToAction(redirectAction, redirectController);
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
                }

                if (statusCode == System.Net.HttpStatusCode.OK)
                {
                    ViewData["success"] = (customer.Customerid != 0) ? "Customer Updated Successfully !!" : "Customer Submitted Successfully !!";
                }
                else
                {
                    ViewData["error"] = "Error occurred while processing your request.";
                }
            }
            catch (Exception ex)
            {
              

                ViewData["error"] = $"An unexpected error occurred: {ex.Message}";
            }

            return RedirectToAction(redirectAction, redirectController);
        }





        [HttpGet]
        public async Task<IActionResult> View(int Id)
        {
            if (!ReadPermission)
                return RedirectToAction("NotAuthorized", "Home");

            ViewData["controller"] = this.ControllerContext.RouteData.Values["controller"].ToString();
            CustomerWithDetailsDto data = new CustomerWithDetailsDto();
            var msg = "";
            try
            {
                if (Id != 0)
                {
                    apiManager = new ApiManager(_configuration["ServiceUrl"].ToString().Trim() + "/api/Customer/" + Id.ToString());
                    var (statusCode, responseContent) = await apiManager.Get();

                    if (statusCode == System.Net.HttpStatusCode.OK)
                    {
                        data = JsonConvert.DeserializeObject<CustomerWithDetailsDto>(responseContent);
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

            return View(data);
        }

    }

}

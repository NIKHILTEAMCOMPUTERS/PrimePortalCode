using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using RMS.Client.Models;
using RMS.Client.Models.Account;
using RMS.Client.Models.Common;
using RMS.Client.Utility;
using System;
using System.Net;

namespace RMS.Client.Controllers.Account
{
    public class LoginController : Controller
    {
        private ApiManager apiManager;
        private IConfiguration _configuration;
        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IActionResult> Index(Login user)
        {
            try
            {
                if (user.tmC_Id != null)
                {
                    apiManager = new ApiManager(_configuration["ServiceUrl"].ToString().Trim() + "/api/UserAuth/ValidateUser");  //--TOKEN
                    var (statusCode, responseContent) = await apiManager.PostJson(JsonConvert.SerializeObject(user));

                    if (statusCode == System.Net.HttpStatusCode.OK)
                    {
                        var loginResponse = JsonConvert.DeserializeObject<LoginResponse>(responseContent);
                        HttpContext.Session.SetInt32("canseemembertimesheet", loginResponse.CanSeeMemberTimesheet ? 1 : 0);
                        HttpContext.Session.SetString("USER_SESSION", JsonConvert.SerializeObject(new UserSession
                        {
                            EmployeeId = loginResponse.EmployeeId,
                            Token = loginResponse.Token,
                            EmpCode = loginResponse.UserId,
                            Email = loginResponse.Email,
                            Name = loginResponse.Name,
                            RoleName = loginResponse.Role,
                           

                        }));
                        return RedirectToAction("Authorize", "Home");
                    }

                    else
                    {
                        ViewData["error"] = "Login failed. Please check your credentials or reset your password via the Team Intranet portal.";
                        return View();
                    }
                }
            }
            catch (Exception ex)
            {
                ViewData["error"] = $"Error : {ex.Message}";
                return View();
            }
            
            return View();
        }


        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.Session.Remove("USER_SESSION");
            HttpContext.Session.Remove("canseemembertimesheet");
            return RedirectToAction("Index", "Login");
        }
        private string GetClientIp()
        {
            string ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

            if (string.IsNullOrEmpty(ipAddress))
            {
                ipAddress = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            }

            if (string.IsNullOrEmpty(ipAddress))
            {
                ipAddress = HttpContext.Connection.LocalIpAddress?.ToString();
            }

            return ipAddress;
        }
    }
}

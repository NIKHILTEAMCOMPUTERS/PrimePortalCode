using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RMS.Data.Models;
using RMS.Entity.Account;
using RMS.Interfaces;
using RMS.Service.Interfaces;
using RMS.Services;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace RMS.Controllers.Account
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserAuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ITokenService _tokenRepo;
        private readonly IUnitOfWork _uow;
        public UserAuthController(IConfiguration configuration, ITokenService tokenRepo, IUnitOfWork uow)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _tokenRepo = tokenRepo ?? throw new ArgumentNullException(nameof(tokenRepo));
            _uow = uow?? throw new ArgumentNullException(nameof(uow));
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> ValidateUser([FromBody] UserRequetDto requestData)
        {

            if (requestData == null)
            {
                return Unauthorized("User does not exist. Please check TMC_ID and Password.");
            }

            var apiManager = new ApiManager("https://wap.teamcomputers.com/emaauth/api/AD/ValidateAD");
            //var apiManager = new ApiManager("https://teamworks.teamcomputers.com/API2/api/ECR/ValidateEmployee");
            var formData = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("uname", requestData.TMC_Id),
                new KeyValuePair<string, string>("pass", requestData.Password),
            };

            var (statusCode, responseContent) = await apiManager.PostFormDatanick(formData);

            if (statusCode == HttpStatusCode.OK)
            {
                var loginResponse = JsonConvert.DeserializeObject<UserResponse>(responseContent);
                if (loginResponse?.Status == true && loginResponse.Data.Count == 1)
                {
                    var item = loginResponse.Data[0];
                    var userData = new AppUser
                    {
                        UserId = item.UserId,
                        Email = item.CompanyEmail,
                        Name = item.Name,
                        Mobile = item.ContactNo,
                        Designation = item.Designation,
                        ReportingHeadId = item.ReportingHeadId,
                        DateofJoining = item.DateofJoining,
                        Department = item.Department,
                    };

                    var rmsEmployeeRecord = await _uow.EmployeeRepository.GetByTmc(item.UserId);
                    var role = rmsEmployeeRecord.Employeeroles.Select(x => x.Role.Rolename).FirstOrDefault();
                    userData.EmployeeId = rmsEmployeeRecord == null ? 0 : rmsEmployeeRecord.Employeeid;

                    var generatedToken = _tokenRepo.CreateToken(userData);
                    // this call the method to log user data
                    await LogUserData(int.Parse(item.UserId), generatedToken.Result, true);
                    userData.CanSeeMemberTimesheet = await CanSeeMemberTimesheet(item.UserId);
                    return Ok(new
                    {
                        token = generatedToken.Result,
                        userData.EmployeeId,
                        userData.Name,
                        userData.UserId,
                        userData.Email,
                        userData.Designation,
                        userData.ReportingHeadId,
                        userData.DateofJoining,
                        userData.Department,
                        userData.CanSeeMemberTimesheet,
                        role

                    });
                }
            }

            return Unauthorized("User does not exist. Please check TMC_ID and Password.");
        }




        //[AllowAnonymous]
        //[HttpPost]
        //public async Task<IActionResult> ValidateUser([FromBody] UserRequetDto requestData)
        //{

        //    if (requestData == null)
        //    {
        //        return Unauthorized("User does not exist. Please check TMC_ID and Password.");
        //    }


        //    var userData = new AppUser
        //    {
        //        UserId = "10376",
        //        Email = "ravi.v@teamcomputers.com",
        //        Name = "Ravi Verma",
        //        Mobile = "8657904937",
        //        Designation = "Delivery Anchor (TDE_APP_DEL ANCHOR)",
        //        ReportingHeadId = "16171",
        //        DateofJoining = "01-May-2023",
        //        Department = "Implementation/Delivery",
        //    };

        //    var rmsEmployeeRecord = await _uow.EmployeeRepository.GetByTmc(userData.UserId);
        //    string role = "DA";
        //    userData.EmployeeId = rmsEmployeeRecord == null ? 0 : rmsEmployeeRecord.Employeeid;
        //    var a = Convert.ToString(userData.EmployeeId);
        //    var generatedToken = _tokenRepo.CreateToken(userData);
        //    await LogUserData(int.Parse(userData.UserId), generatedToken.Result, true);
        //    return Ok(new
        //    {
        //        token = generatedToken.Result,
        //        userData.EmployeeId,
        //        userData.Name,
        //        userData.UserId,
        //        userData.Email,
        //        userData.Designation,
        //        userData.ReportingHeadId,
        //        userData.DateofJoining,
        //        userData.Department,
        //        role

        //    });
        //}









        private async Task LogUserData(int userId, string generatedToken, bool status)
        {
            _uow.UserLoginLogRepository.Add(new Userloginlog
            {
                Userid = userId,
                Jwttoken = generatedToken,
                Loginstatus = status.ToString()
            });

            await _uow.Complete();
        }


        private async Task<bool> CanSeeMemberTimesheet(string userId)
        {

            return await _uow.EmployeeRepository.CheckMemberCanSeeResourceTimesheet(userId);
        }



    }
}

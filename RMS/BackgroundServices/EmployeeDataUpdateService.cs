using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RMS.Entity.DTO;
using RMS.Service.Interfaces;
using RMS.Service.Repositories.Master;
using RMS.Utility;
using System.Globalization;
using System.Text;

namespace RMS.BackgroundServices
{
    public partial class EmployeeDataUpdateService : IHostedService, IDisposable
    {
        private Timer _timer;
        private Timer _doWorkTimer;
        private Timer _sendMailerTimer;
        private Timer _crmUpdateTimer;
        private readonly HttpClient _client;
        private readonly IServiceProvider _serviceProvider;
        IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<EmployeeDataUpdateService> _logger;
        Microsoft.AspNetCore.Hosting.IHostingEnvironment _env;
        IConfiguration _config;
      

        public EmployeeDataUpdateService(IServiceProvider serviceProvider, IConfiguration configuration,
            Microsoft.AspNetCore.Hosting.IHostingEnvironment environment, IServiceScopeFactory serviceScopeFactory,
            ILogger<EmployeeDataUpdateService> logger)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _env = environment;
            //_serviceProvider = serviceProvider;
            _config = configuration;
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
            _client = new HttpClient();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            //_timer = new Timer(DoWork, null, TimeSpan.Zero,
            //    TimeSpan.FromHours(5));
            //return Task.CompletedTask;

            // Calculate the time until 3 AM
            var now = DateTime.Now;
            var targetTime = now.Date.AddHours(3);
            if (now > targetTime)
            {
                targetTime = targetTime.AddDays(1);
            }
            var timeToStart = targetTime - now;

            var crmTargetTime = now.Date.AddHours(6); 
            if (now > crmTargetTime)
            {
                crmTargetTime = crmTargetTime.AddDays(1); 
            }
            var timeToStartCRMUpdate = crmTargetTime - now;


            // Start the timer to run at 3 AM and then every 24 hours
            //_timer = new Timer(DoWork, null, timeToStart, TimeSpan.FromHours(24));
            //_timer = new Timer(SendMailer, null, TimeSpan.Zero, TimeSpan.FromMinutes(5));
            _doWorkTimer = new Timer(DoWork, null, timeToStart, TimeSpan.FromHours(24));// // Runs at 3 AM daily
            _sendMailerTimer = new Timer(SendMailer, null, TimeSpan.Zero, TimeSpan.FromMinutes(5));
            _crmUpdateTimer = new Timer(UpdateCRMData, null, timeToStartCRMUpdate, TimeSpan.FromHours(24)); // Runs at 6 AM daily
            return Task.CompletedTask;
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            //_timer?.Change(Timeout.Infinite, 0);
            _doWorkTimer?.Change(Timeout.Infinite, 0);
            _sendMailerTimer?.Change(Timeout.Infinite, 0);
            _crmUpdateTimer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;            
        }
        public void Dispose()
        {
            //_timer?.Dispose();
            _doWorkTimer?.Dispose();
            _sendMailerTimer?.Dispose();
            _crmUpdateTimer?.Dispose();
            _client?.Dispose();
        }
        private async void SendMailer(object state)
        {
            DateTime dateTime = DateTime.Now;
            TimeSpan startTime = new TimeSpan(11, 0, 0); // 8:00 AM
            TimeSpan endTime = new TimeSpan(11, 05, 0); // 8:05 AM
            TimeSpan deleteTime = new TimeSpan(12, 0, 0); // 8:00 AM
            TimeSpan deleteendTime = new TimeSpan(12, 05, 0); // 8:05 AM
            TimeSpan currentTimeOfDay = dateTime.TimeOfDay;
            // manually sending mails 
      //GetBillingRptMonthDAMailer();
            //if (dateTime.DayOfWeek == DayOfWeek.Thursday && currentTimeOfDay >= startTime && currentTimeOfDay <= endTime)
            try
            {
               
                if (currentTimeOfDay >= startTime && currentTimeOfDay <= endTime)
                {
                    if (dateTime.DayOfWeek == DayOfWeek.Monday)
                    {
                        GetBillingRptMonthDAMailer(false);
                    }
                    else
                        GetBillingRptMonthDAMailer();
                }
                if (currentTimeOfDay >= deleteTime && currentTimeOfDay <= deleteendTime)
                {
                    DeleteCSVFiles();
                }
              
            }
            catch (Exception ex)
            {
                // Log the exception
                // For example, using ILogger:
                _logger.LogInformation($"An error occurred while executing SendMailer at {DateTime.UtcNow}",ex);
            }

        }
        private async void DoWork(object state)
        {
            
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var _uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                    //var apiData = await GetApiDataAsync("https://bam.teamcomputers.com/rmp/Master/employee");
                     // var apiData = await GetApiDataBulkAsync("https://tara.teamcomputers.com/api/thirdparty/internalDataSync");
                   var apiData = await GetApiDataBulkAsync(""); 
                    if (apiData != null)
                    {
                        //await _uow.EmployeeRepository.UpsertEmployeesOnebyOne(apiData);
                        await _uow.EmployeeRepository.UpsertEmployeesBulkData(apiData);
                        await _uow.Complete();
                    }
                }
            }
            catch (Exception ex)
            {                
                _logger.LogInformation($"An error occurred while executing DoWork at {DateTime.UtcNow}",ex);
            }

        }

        private async Task<List<EmployeAPIDarwinDto>> GetApiDataAsync(string apiUrl)
        {
            var response = await _client.PostAsync(apiUrl, new StringContent(""));
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<EmployeAPIDarwinDto>>(content);
        }
        private async Task<List<EmployeAPIDarwinDtoJson>> GetApiDataBulkAsync(string apiUrl)
        {
            var requestBody = JsonConvert.SerializeObject(new { search = "", isActive = 1 });
            var content = new StringContent(requestBody, Encoding.UTF8, "application/json");
            
            var response = await _client.PostAsync(apiUrl, content);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            
            try
            {
                var taraResponse = JsonConvert.DeserializeObject<TaraAPIResponse>(responseContent);
                
                // Map TARA API response to existing DTO structure
                var mappedData = taraResponse.EmployeeData.Select(MapTaraToDarwinDto).ToList();

                return mappedData;
            }
            catch (JsonSerializationException ex)
            {
                _logger.LogError($"JSON deserialization error: {ex.Message}");
                // Return empty list if deserialization fails
                return new List<EmployeAPIDarwinDtoJson>();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error processing API response: {ex.Message}");
                return new List<EmployeAPIDarwinDtoJson>();
            }
        }

        private EmployeAPIDarwinDtoJson MapTaraToDarwinDto(TaraEmployeeData tara)
        {
            return new EmployeAPIDarwinDtoJson
            {
                sbu = tara.EmployeeSBU ?? tara.SBU,
                sbu_code = tara.SBUCode,
                employee_id = tara.TMC,
                date_of_birth = tara.BirthDate,
                reporting_head_id = tara.ReportingHeadID,
                base_office_location = tara.LocationCode,
                leavingdate = tara.DateOfExit,
                employee_name = tara.FullName,
                designation = tara.Designation,
                company_email_id = tara.CompanyEMail,
                office_mobile_no = tara.MobilePhoneNo,
                joiningdate = tara.DateOfJoining,
                branch_code = tara.Branch,
                employee_region = tara.LocationCode,
                holiday_calendar = tara.HolidayCalendar,
                department = tara.Department,
                cost_center = tara.CostCenter,
                CostCenterId = tara.CostCenterId,
                past_work_experience = tara.PastWorkExperience,
                DepartmentCode = tara.DepartmentCode,
                date_of_resignation = tara.DateOfExit
            };
        }       
        public void ManualTrigger()
        {
            DoWork(null);
        }
        public void ManualTriggerforCRM()
        {
            UpdateCRMData(null);
        }
        public async void GetBillingRptMonthDAMailer(bool isDA = true)
        {
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    DateTime now = DateTime.Now;
                    string monthyear = now.ToString("MMM-yy",CultureInfo.InvariantCulture);
                    //if (monthyear == null) { return BadRequest("Month year can not be null"); }
                    var context = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                    var Results = await context.ReportRepository.GetReportByMonthYearProcedure_DA_WiseForMailer(monthyear);

                    string mailBody = (System.IO.File.Exists(Path.Combine(_env.WebRootPath, "Resource", "Mailer", "monthwisedareport.html")))
                                        ? System.IO.File.ReadAllText(Path.Combine(_env.WebRootPath, "Resource", "Mailer", "monthwisedareport.html"))
                                        : string.Empty;



                    string sMailOutput = "";
                    string sMailSubject = "Month Wise DA Report";


                    string mailData = "";
                    mailData += $@"    <table style = 'width: 100%; border: 1px solid #e1e1e1' cellspacing = '0' >    ";
                    mailData += $@"        <tr>                                        ";
                    mailData += $@"          <th width='20%'  style = 'border-right: 1px solid #e1e1e1;text-align:left; border-bottom: 1px solid #e1e1e1;background: #fbfbfb;padding: 15px;font-weight: 600;font-size: 15px;text-transform: capitalize;  color: #65696b;'>DA Name  </th> ";
                    mailData += $@"          <th width='10%'  style = 'text-align:left;border-right: 1px solid #e1e1e1;border-bottom: 1px solid #e1e1e1;background: #fbfbfb;padding: 15px;font-weight: 600;font-size: 15px;text-transform: capitalize;  color: #65696b;'> T&M Count (Project Wise)</th>    ";
                    mailData += $@"           <th width='10%' style = 'text-align:left;border-right: 1px solid #e1e1e1;border-bottom: 1px solid #e1e1e1;background: #fbfbfb;padding: 15px;font-weight: 600;font-size: 15px;text-transform: capitalize;  color: #65696b;'> Fixed Bid Count (Project Wise) </th> ";
                    mailData += $@"          <th width='40%' style =  'text-align:center;border-right: 1px solid #e1e1e1;border-bottom: 1px solid #e1e1e1;background: #fbfbfb;padding: 15px;font-weight: 600;font-size: 15px;text-transform: capitalize;  color: #65696b;' colspan='2'>Provision</th> ";
                    mailData += $@"          <th width='10%'  style = 'text-align:left;border-right: 1px solid #e1e1e1;border-bottom: 1px solid #e1e1e1;background: #fbfbfb;padding: 15px;font-weight: 600;font-size: 15px;text-transform: capitalize;  color: #65696b;'>Total Actual Billing</th>  ";
                    mailData += $@"          <th width='10%'  style = 'text-align:left;border-right: 1px solid #e1e1e1;border-bottom: 1px solid #e1e1e1;background: #fbfbfb;padding: 15px;font-weight: 600;font-size: 15px;text-transform: capitalize;  color: #65696b;'>Total Billing</th>  ";
                    mailData += $@"        </tr >  ";


                    mailData += $@"        <tr>                                        ";
                    mailData += $@"          <th width='20%'  style = 'border-right: 1px solid #e1e1e1;text-align:left; border-bottom: 1px solid #e1e1e1;background: #fbfbfb;padding: 15px;font-weight: 400;font-size: 12px;text-transform: capitalize;  color: #65696b;'></th> ";
                    mailData += $@"          <th width='10%'  style = 'text-align:left;border-right: 1px solid #e1e1e1;border-bottom: 1px solid #e1e1e1;background: #fbfbfb;padding: 15px;font-weight: 400;font-size: 12px;text-transform: capitalize;  color: #65696b;'></th>    ";
                    mailData += $@"          <th width='10%' style = 'text-align:left;border-right: 1px solid #e1e1e1;border-bottom: 1px solid #e1e1e1;background: #fbfbfb;padding: 15px;font-weight: 400;font-size: 12px;text-transform: capitalize;  color: #65696b;'></th> ";
                    mailData += $@"          <th width='20%' style =  'text-align:left;border-right: 1px solid #e1e1e1;border-bottom: 1px solid #e1e1e1;background: #fbfbfb;padding: 15px;font-weight: 400;font-size: 12px;text-transform: capitalize;  color: #65696b;'>Total Pending Provision Billing</th> ";
                    mailData += $@"          <th width='20%' style =  'text-align:left;border-right: 1px solid #e1e1e1;border-bottom: 1px solid #e1e1e1;background: #fbfbfb;padding: 15px;font-weight: 400;font-size: 12px;text-transform: capitalize;  color: #65696b;'>Total Approved Provision Billing</th> ";
                    mailData += $@"          <th width='10%'  style = 'text-align:left;border-right: 1px solid #e1e1e1;border-bottom: 1px solid #e1e1e1;background: #fbfbfb;padding: 15px;font-weight: 400;font-size: 12px;text-transform: capitalize;  color: #65696b;'></th>  ";
                    mailData += $@"          <th width='10%'  style = 'text-align:left;border-right: 1px solid #e1e1e1;border-bottom: 1px solid #e1e1e1;background: #fbfbfb;padding: 15px;font-weight: 400;font-size: 12px;text-transform: capitalize;  color: #65696b;'></th>  ";
                    mailData += $@"        </tr >  ";


                    foreach (var data in Results)
                    {
                        mailData += $@"        <tr >                                                                      ";
                        mailData += $@"           <td width='20%'  style ='border-right: 1px solid #e1e1e1; border-bottom: 1px solid #e1e1e1;background: #fbfbfb;padding: 15px; font-size: 12px; text-transform: capitalize; color: #65696b;'> ";
                        mailData += $@"            {data.DAname}                                                                  ";
                        mailData += $@"          </td >                                                                   ";
                        mailData += $@"           <td width='10%' style ='border-right: 1px solid #e1e1e1; border-bottom: 1px solid #e1e1e1;background: #fbfbfb;padding: 15px; font-size: 12px; text-transform: capitalize; color: #65696b;'> ";
                        mailData += $@"           {data.TAndMCount}                                                                   ";
                        mailData += $@"          </td >                                                                   ";
                        mailData += $@"           <td width='10%' style ='border-right: 1px solid #e1e1e1; border-bottom: 1px solid #e1e1e1;background: #fbfbfb;padding: 15px; font-size: 12px; text-transform: capitalize; color: #65696b;'> ";
                        mailData += $@"            {data.FixedBidCount}                                                                 ";
                        mailData += $@"           </td >                                                                  ";
                        mailData += $@"           <td width='20%' style ='border-right: 1px solid #e1e1e1; border-bottom: 1px solid #e1e1e1;background: #fbfbfb;padding: 15px; font-size: 12px; text-transform: capitalize; color: #65696b;'> ";
                        mailData += $@"            {FormatIndianNumber(data.TotalPendingProvisions)}";
                        mailData += $@"           </td >   ";
                        mailData += $@"           <td width='20%' style ='border-right: 1px solid #e1e1e1; border-bottom: 1px solid #e1e1e1;background: #fbfbfb;padding: 15px; font-size: 12px; text-transform: capitalize; color: #65696b;'> ";
                        mailData += $@"            {FormatIndianNumber(data.TotalProvisionBilling)}";
                        mailData += $@"           </td >   ";
                        mailData += $@"           <td width='10%' style ='border-right: 1px solid #e1e1e1; border-bottom: 1px solid #e1e1e1;background: #fbfbfb;padding: 15px; font-size: 12px; text-transform: capitalize; color: #65696b;'> ";
                        mailData += $@"            {FormatIndianNumber(data.TotalActualBilling)}                                                                  ";
                        mailData += $@"           </td >                                                                  ";
                        mailData += $@"           <td width='10%' style ='border-right: 1px solid #e1e1e1; border-bottom: 1px solid #e1e1e1;background: #fbfbfb;padding: 15px; font-size: 12px; text-transform: capitalize; color: #65696b;'> ";
                        mailData += $@"            {FormatIndianNumber(data.TotalActualBilling + data.TotalProvisionBilling)}                                                                 ";
                        mailData += $@"           </td >                                                                  ";
                        mailData += $@"        </tr >      ";
                    }

                    mailData += $@"        <tr >                              ";
                    mailData += $@"        <td  width='20%' style = 'padding: 15px;font-weight: 400;font-size: 12px;text-transform: capitalize;color: #65696b;'></td>";
                    mailData += $@"        <td  width='10%' style = 'padding: 15px;font-weight: 400;font-size: 12px;text-transform: capitalize;color: #65696b;'>{Results.Sum(x => x.TAndMCount)}</td>";
                    mailData += $@"        <td  width='10%' style = 'padding: 15px;font-weight: 400;font-size: 12px;text-transform: capitalize;color: #65696b;'>{Results.Sum(x => x.FixedBidCount)}</td>";
                    mailData += $@"        <td  width='20%' style = 'padding: 15px;font-weight: 400;font-size: 12px;text-transform: capitalize;color: #65696b;'>₹{FormatIndianNumber(Results.Sum(x => x.TotalPendingProvisions))}</td>";
                    mailData += $@"        <td  width='20%' style = 'padding: 15px;font-weight: 400;font-size: 12px;text-transform: capitalize;color: #65696b;'>₹{FormatIndianNumber(Results.Sum(x => x.TotalProvisionBilling))}</td>";
                    mailData += $@"        <td width='10%' style = 'padding: 15px;font-weight: 400;font-size: 12px;text-transform: capitalize;color: #00cc83;'>₹{FormatIndianNumber(Results.Sum(x => x.TotalActualBilling))} </td>";
                    mailData += $@"        <td  width='10%' style = 'padding: 15px;font-weight: 400;font-size: 12px;font-weight: bold;text-transform: capitalize;color: #000000;'>₹{FormatIndianNumber(Results.Sum(x => x.TotalActualBilling + x.TotalProvisionBilling + x.TotalPendingProvisions))} </td>";

                    mailData += $@"      </tr > ";

                    mailData += $@"      </table >     ";
                    string sMailBody = string.Format(mailBody.ToString(), mailData, monthyear);
                    var objItem = await context.ReportRepository.GetReportByMonthYearProcedure(monthyear);
                    objItem = objItem.Where(x =>(x.provesionbilling != null && x.provesionbilling > 0) || (x.actualbilling != null && x.actualbilling > 0)).ToList();
                    var listOfAnonymousObjects = new List<object>();
                    foreach (var data in objItem)
                    {
                        var castedData = new
                        {
                            TMC = data.TMC,
                            ResourceName = data.ResourceName,
                            AccountManager = data.CCname,
                            CustomerName = data.CustomerName,
                            Projectname = data.projectname,
                            ProjectType = data.ProjectType,
                            Actualbilling = data.actualbilling,
                            Provesionbilling = data.provesionbilling,
                            DAname = data.DAname,
                            Contractenddate = $"{data.contractstartdate:dd/MM/yyyy} - {data.contractenddate:dd/MM/yyyy}",
                            Products = data.SPname,
                            Practice = data.Pname,
                            ClosureDateOfBilling = data.estimatedbillingdate,
                            POno = data.POno,
                            projectno = data.projectno,
                            Cno = data.Cno,
                        };
                        listOfAnonymousObjects.Add(castedData);
                    }


                    string fileName = "FinancialReport_" + Guid.NewGuid() + ".csv";
                    string directoryPath = Path.Combine(_env.WebRootPath, "temp", "tempcsv");


                    Directory.CreateDirectory(directoryPath);

                    string filePath = Path.Combine(directoryPath, fileName);
                    WriteToCSV(listOfAnonymousObjects, filePath);
                    if (isDA)
                        SendMail(_config["MailTo"].ToString(), sMailSubject, sMailBody, null, out sMailOutput, filePath);
                    else
                    {
                        SendMail(_config["MailToDirector"].ToString(), sMailSubject, sMailBody, null, out sMailOutput, filePath);
                        SendMail(_config["MailTo"].ToString(), sMailSubject, sMailBody, null, out sMailOutput, filePath);
                    }

                }
            }
            catch (Exception e)
            {
                _logger.LogInformation($"An error occurred while executing DoWork at {DateTime.UtcNow}",e);
            }
        }

        static string FormatIndianNumber(dynamic number)
        {
            string formattedNumber = number.ToString("N", new System.Globalization.CultureInfo("hi-IN"));
            string[] parts = formattedNumber.Split('.');

            // Round the decimal part to two decimal places
            decimal roundedDecimal = Math.Round(decimal.Parse(parts[1]), 2);

            // If the rounded decimal part is zero, remove the decimal point
            string decimalString = roundedDecimal == 0 ? "" : "." + roundedDecimal.ToString().TrimEnd('0');

            // Combine the integer part and rounded decimal part
            string roundedNumber = $"{parts[0]}{decimalString}";

            return roundedNumber;
        }

        static void WriteToCSV(IEnumerable<dynamic> list, string filePath)
        {
            // Create a StringBuilder to construct CSV content
            //StringBuilder csvContent = new StringBuilder();

            //// Header row
            //csvContent.AppendLine(string.Join(",", objItem.First().Keys));

            //// Data rows
            //foreach (var data in objItem)
            //{
            //    csvContent.AppendLine(string.Join(",", data.Values));
            //}

            var csv = new StringBuilder();

            // Extract header row from the properties of the first item
            var firstItem = list.FirstOrDefault();
            if (firstItem != null)
            {
                var properties = firstItem.GetType().GetProperties();
                foreach (var property in properties)
                {
                    csv.Append($"{property.Name},");
                }
                csv.AppendLine(); // New line after header

                // Append data rows
                foreach (var item in list)
                {
                    foreach (var property in properties)
                    {
                        var value = property.GetValue(item);
                        csv.Append($"{(value != null ? value.ToString() : string.Empty)},");
                    }
                    csv.AppendLine(); // New line after each object
                }
            }


            // Write content to file
            System.IO.File.WriteAllText(filePath, csv.ToString());
        }

        internal bool SendMail(string username, string sMailSubject, string sMailBody, List<string> ccemails, out string sMailOutput, string attachement = null)
        {
            try
            {
                string strSenderEmail = _config["MailCredential:Email"] != null ? _config["MailCredential:Email"].ToString() : "";
                string strSenderPassword = _config["MailCredential:Password"] != null ? _config["MailCredential:Password"].ToString() : "";
                int strSenderPort = _config["MailCredential:Port"] != null ? Convert.ToInt32(_config["MailCredential:Port"].ToString()) : 587;
                string strSenderHost = _config["MailCredential:Host"] != null ? _config["MailCredential:Host"].ToString() : "";
                bool lEnableSsl = _config["MailCredential:EnableSsl"] != null ? Convert.ToBoolean(_config["MailCredential:EnableSsl"].ToString()) : true;

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
        public void DeleteCSVFiles()
        {
            
            string directoryPath = Path.Combine(_env.WebRootPath, "temp", "tempcsv");
            var csvFiles =   Directory.GetFiles(directoryPath, "*.csv");

            foreach (var csvFile in csvFiles)
            {
                System.IO.File.Delete(csvFile);
            }
        }


        
    }
}

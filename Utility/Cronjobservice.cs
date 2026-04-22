using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using RMS.Client.Models.Contract;
using RMS.Client.Models.Master;
using RMS.Utility;

namespace RMS.Client.Utility
{
    public class Cronjobservice : IHostedService, IDisposable
    {
        private readonly Timer _timer;
        private readonly Timer _alert;
        private readonly HttpClient _client;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _config;
        private readonly ILogger<Cronjobservice> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ApiManager _apiManager;
        private readonly IsoDateTimeConverter _dateTimeConverter;
        private IsoDateTimeConverter dateTimeConverter;
        public Cronjobservice(IServiceProvider serviceProvider, IConfiguration configuration, IWebHostEnvironment environment, ILogger<Cronjobservice> logger)
        {
            _env = environment ?? throw new ArgumentNullException(nameof(environment));
            _config = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _alert = _timer = new Timer(ContractExpiryAlerts, null, TimeSpan.Zero, TimeSpan.FromSeconds(30));
            _client = new HttpClient();
            _timer = new Timer(SendMailer, null, TimeSpan.Zero, TimeSpan.FromMinutes(1)); // Change interval if needed
            _dateTimeConverter = new IsoDateTimeConverter(); // Initialize dateTimeConverter if needed
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Background service started.");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Background service stopped.");
            _timer?.Change(Timeout.Infinite, 0);
            _client?.Dispose();
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
            _client?.Dispose();
        }

        private bool emailSentToday = false;
        private bool filesDeletedToday = false;

        private async void SendMailer(object state)
        {
            DateTime dateTime = DateTime.Now;
            TimeSpan targetTime = new TimeSpan(4, 0, 0); // 4:00 AM
            TimeSpan deleteTime = new TimeSpan(5, 0, 0); // 5:00 AM

            if (dateTime.DayOfWeek == DayOfWeek.Monday || dateTime.DayOfWeek == DayOfWeek.Wednesday || dateTime.DayOfWeek == DayOfWeek.Friday)
            {
                TimeSpan currentTimeOfDay = dateTime.TimeOfDay;

                if (!emailSentToday && currentTimeOfDay.Hours == targetTime.Hours && currentTimeOfDay.Minutes == targetTime.Minutes)
                {
                    await ResourceReport(); // Call your method to send emails and perform other tasks
                    emailSentToday = true; // Set flag to true after successful execution
                }
                if (!filesDeletedToday && currentTimeOfDay.Hours == deleteTime.Hours && currentTimeOfDay.Minutes == deleteTime.Minutes)
                {
                    DeleteCSVFiles();
                    filesDeletedToday = true; // Set flag to true after successful execution
                }
            }
            else // Reset flags at the beginning of each new day
            {
                emailSentToday = false;
                filesDeletedToday = false;
            }
         //    await ResourceReport(); // for manual mail
        }


        public async Task ResourceReport()
        {
            try
            {


                string currentyear = DateTime.Now.ToString("yyyy");
                string endpointUrl = $"{_config["ServiceUrl"].ToString().Trim()}/api/Report/GetResourcesInfoForHR";
                var dataList = new List<ResourceInfoForHRDto>();
                
                    var apiManager = new ApiManager(endpointUrl);
                    var (statusCode, responseContent) = await apiManager.Get();

                    if (statusCode == System.Net.HttpStatusCode.OK)
                    {
                        dataList = JsonConvert.DeserializeObject<List<ResourceInfoForHRDto>>(responseContent);
                    //dataList = dataList
                    // .Where(x =>
                    //     (x.BillableNonbillable == null || // Include if BillableNonbillable is null
                    //      x.BillableNonbillable.ToLower() != "non-billable") &&
                    //     (x.Flag == null || x.Flag.ToLower() != "deployed"))
                    // .ToList();
                
                   
                    string mailBody = (System.IO.File.Exists(Path.Combine(_env.WebRootPath, "Resource", "Mailer", "resourcestatusreport.html")))
                                        ? System.IO.File.ReadAllText(Path.Combine(_env.WebRootPath, "Resource", "Mailer", "resourcestatusreport.html"))
                                        : string.Empty;

                    string sMailOutput = "";
                        string sMailSubject = "Resources Status Report";

                        string sMailBody = string.Format(mailBody.ToString(), currentyear);
                        string fileName = "ResourceReport_" + Guid.NewGuid() + ".csv";
                        string directoryPath = Path.Combine(_env.WebRootPath, "temp", "tempcsv");
                        Directory.CreateDirectory(directoryPath);

                        string filePath = Path.Combine(directoryPath, fileName);
                        WriteToCSV(dataList, filePath);

                        SendMail(_config["MailTo"].ToString(), sMailSubject, sMailBody, null, out sMailOutput, filePath);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while generating resource report.");
                    // Handle exceptions
                }
            }

        public bool SendMail(string username, string sMailSubject, string sMailBody, List<string> ccemails, out string sMailOutput, string attachement = null)
        {
            try
            {
                string strSenderEmail = _config["MailCredential:Email"] ?? "";
                string strSenderPassword = _config["MailCredential:Password"] ?? "";
                int strSenderPort = int.Parse(_config["MailCredential:Port"] ?? "587");
                string strSenderHost = _config["MailCredential:Host"] ?? "";
                bool lEnableSsl = bool.Parse(_config["MailCredential:EnableSsl"] ?? "true");

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
            var csvFiles = Directory.GetFiles(directoryPath, "*.csv");

            foreach (var csvFile in csvFiles)
            {
                System.IO.File.Delete(csvFile);
            }
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


        #region Contractexpiryalerts
        // For contract expiry mail
        private bool alertSentToday = false;
        private async void ContractExpiryAlerts(object state)
        {
           // await Contractendingsoon(); // mannual trigger
            DateTime dateTime = DateTime.Now;
            TimeSpan targetTime = new TimeSpan(2, 00, 0); // 4:00 AM
            bool successful = false;
           
            if (dateTime.DayOfWeek == DayOfWeek.Sunday || dateTime.DayOfWeek == DayOfWeek.Monday || dateTime.DayOfWeek == DayOfWeek.Wednesday|| dateTime.DayOfWeek == DayOfWeek.Friday)
            {
                TimeSpan currentTimeOfDay = dateTime.TimeOfDay;

                if (!alertSentToday && currentTimeOfDay.Hours == targetTime.Hours && currentTimeOfDay.Minutes == targetTime.Minutes)
                {
                     successful = await Contractendingsoon(); // Call your method to send emails and perform other tasks
                    if (successful)
                    {
                        alertSentToday = true; // Set flag to true after successful execution
                    }
                }
               
            }
            else // Reset flags at the beginning of each new day
            {
                alertSentToday = false;
               
            }
          
        }
        public async Task<bool> Contractendingsoon(bool mailSent = false)
        {
            var emp = new List<ContractendingsoonDto>();
            bool emailSentSuccessfully = false; // Track if any email was sent successfully
            try
            {
                string endpointurl = $"{_config["ServiceUrl"].ToString().Trim()}/api/Contract/Contractendingsoon";

                var apiManager = new ApiManager(endpointurl);
                var (statusCode, responseContent) = await apiManager.Get();

                if (statusCode == System.Net.HttpStatusCode.OK)
                {
                    if (responseContent != null)
                    {
                        emp = JsonConvert.DeserializeObject<List<ContractendingsoonDto>>(responseContent);

                        // Define target intervals in days
                        var targetIntervals = new[] { 30, 15, 7, 2 };

                        foreach (var interval in targetIntervals)
                        {
                            // Find contracts expiring in the specified interval or less
                            var contractsInInterval = emp
                                .Where(c => c.ContractEndDate.HasValue &&
                                            (c.ContractEndDate.Value - DateTime.Now).TotalDays <= interval &&
                                            (c.ContractEndDate.Value - DateTime.Now).TotalDays > 0)
                                .ToList();

                            if (contractsInInterval.Any())
                            {
                                // Group contracts by Delivery Anchor Email
                                var contractsGroupedByAnchor = contractsInInterval
                                    .GroupBy(c => c.DeliveryAnchorEmail);

                                foreach (var anchorGroup in contractsGroupedByAnchor)
                                {
                                    var deliveryAnchorEmail = anchorGroup.Key;

                                    // Prepare email content
                                    string sMailOutput;
                                    string sMailSubject = $"Contracts Expiring Soon in {interval} Days or Less";
                                    string mailcontent;

                                    if (anchorGroup.Count() > 1)
                                    {
                                        // Create an HTML table for multiple contracts
                                        mailcontent = "<p>Here are your contracts expiring in " + interval + " days or less:</p>" +
                                                    "<table border='1' cellpadding='5' cellspacing='0' style='width: 100%; border-collapse: collapse; border: 1px solid #e2e8f0; margin-bottom: 1rem;'>" +
                                                    "<thead style='background-color: #e2e8f0;'><tr><th style='text-align: left; padding: .75rem 1rem;'>Company Name</th><th style='text-align: left; padding: .75rem 1rem;'>Contract No</th><th style='text-align: left; padding: .75rem 1rem;'>PO No</th><th style='text-align: left; padding: .75rem 1rem;'>End Date</th></tr></thead>" +
                                                    "<tbody>";

                                        foreach (var contract in anchorGroup)
                                        {
                                            mailcontent += $"<tr><td style='padding: 1rem;'>{contract.Companyname}</td><td style='padding: 1rem;'>{contract.ContractNo}</td><td style='padding: 1rem;'>{contract.PoNumber}</td><td style='padding: 1rem;'>{contract.ContractEndDate.Value.ToShortDateString()}</td></tr>";
                                        }

                                        mailcontent += "</tbody></table>";
                                    }
                                    else
                                    {
                                        // Single contract
                                        var contract = anchorGroup.First();
                                        mailcontent = $@"
                                        <p>Your contract (Contract No.: {contract.ContractNo} / PO Number: {contract.PoNumber}) with {contract.Companyname} is set to expire on {contract.ContractEndDate.Value.ToShortDateString()}.</p>
                                        <p>Please review and take the necessary actions before the expiration date.</p>";

                                    }

                                    // CC Emails: Practice Head and Account Manager
                                    var ccEmails = anchorGroup
                                        .Select(c => c.PracticeHeadEmail)
                                        .Concat(anchorGroup.Select(c => c.AccountManagerEmail))
                                        .Distinct().ToList();
                                    string mailTemplatePath = Path.Combine(_env.WebRootPath, "Resource", "Mailer", "genericmailer.html");

                                    string mailBody = System.IO.File.Exists(mailTemplatePath) ? System.IO.File.ReadAllText(mailTemplatePath) : string.Empty;

                                    string sMailBody = string.Format(
                                      mailBody,
                                      DateTime.Now.ToString("yyyy"),
                                      "",
                                      "",
                                      "Delivery Anchor",
                                      mailcontent
                                  );


                                    // Sending mail to the current delivery anchor
                                    // mailSent = true;
                                    mailSent = SendMail(
                                                deliveryAnchorEmail, // Sending to the current delivery anchor's email
                                            sMailSubject,
                                             sMailBody,
                                                ccEmails,
                                                     out sMailOutput
                                          );    // always comment this when running in development.

                                    if (mailSent)
                                    {
                                        emailSentSuccessfully = true;
                                    }
                                    else
                                    {
                                        return false;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;

            }
            return emailSentSuccessfully; // Return whether any email was sent successfully

        }
        #endregion
    }
}

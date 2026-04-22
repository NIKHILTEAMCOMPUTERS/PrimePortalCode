using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RMS.Entity.DTO;
using RMS.Service.Interfaces;

namespace RMS.BackgroundServices
{
    public partial class EmployeeDataUpdateService
    {

        #region[CRM DATA UPDATION ]

        private async void UpdateCRMData(object state)
        {
            try
            {
                int currentPage = 1, totalPages =  1;
                //int totalPages = 1;

                do
                {

                    string apiURL = $"https://teamcomputerscrm.myfreshworks.com/crm/sales/api/deals/view/18000573935?page={currentPage}&per_page=100&" +
                                    $"include=deal_type,deal_payment_status,deal_reason,creater,updater,sales_activities,owner,source,territory,creater,updater,deal_stage,currency,sales_account";


                    var crmApiData = await GetCrmApiDataAsync(apiURL);
                    if (crmApiData != null)
                    {

                        using (var scope = _serviceProvider.CreateScope())
                        {
                            var _uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                            await _uow.EmployeeRepository.UpsertCRMBulkData(crmApiData);
                            await _uow.Complete();
                        }


                        var meta = crmApiData.meta;
                        if (meta != null)
                        {
                            totalPages = crmApiData.meta.total_pages.Value;
                            currentPage++;
                        }
                    }

                } while (currentPage <= totalPages);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while updating CRM data: {ex.Message}", ex);
            }
        }

        public async Task<CRMresponseDto> GetCrmApiDataAsync(string apiUrl)
        {
            //string apiUrl = "https://teamcomputerscrm.myfreshworks.com/crm/sales/api/deals/view/18000573935?page=1&per_page=100&include=deal_type,deal_payment_status,deal_reason,creater,updater,sales_activities,owner,source,territory,creater,updater,deal_stage,currency,sales_account";

            // Create an HttpClientHandler to automatically decompress gzip responses
            using (var handler = new HttpClientHandler { AutomaticDecompression = System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.Deflate })
            using (var client = new HttpClient(handler))
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("*/*"));

                // Set Authorization as seen in the Postman request
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Token", "token=bjTfbsw_kHFPWWT018gfLg");

                // Additional headers to match Postman request
                client.DefaultRequestHeaders.UserAgent.ParseAdd("PostmanRuntime/7.42.0");
                client.DefaultRequestHeaders.AcceptEncoding.ParseAdd("gzip, deflate, br");
                client.DefaultRequestHeaders.Connection.ParseAdd("keep-alive");

                var response = await client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<CRMresponseDto>(responseBody);
                    return result;
                }

                return null;
            }

            #endregion[CRM DATA UPDATION ]
        }
    }
}

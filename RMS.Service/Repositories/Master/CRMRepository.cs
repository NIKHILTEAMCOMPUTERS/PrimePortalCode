using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;
using NpgsqlTypes;
using RMS.Entity.DTO;
using System.Diagnostics;
using System.Text.Json;

namespace RMS.Service.Repositories.Master
{
    public partial class EmployeeRepository
    {

        public async Task<bool> UpsertCRMBulkData(CRMresponseDto crmdata )
        {            

            var startTime = DateTime.UtcNow;
            var stopwatch = Stopwatch.StartNew();
            _logger.LogInformation("---- CRM Data Service Started at {StartTime} ----", startTime);

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
               
                List<string> individual_dtypes_Jsons = crmdata.deal_types.Select(dtypes => JsonSerializer.Serialize(dtypes)).ToList();
                List<string> individual_reasons_Jsons = crmdata.deal_reasons.Select(reason => JsonSerializer.Serialize(reason)).ToList();
                List<string> individual_users_Jsons = crmdata.users.Select(user => JsonSerializer.Serialize(user)).ToList();
                List<string> individual_lead_sources_Jsons = crmdata.lead_sources.Select(leadSource => JsonSerializer.Serialize(leadSource)).ToList();
                List<string> individual_territories_Jsons = crmdata.territories.Select(territory => JsonSerializer.Serialize(territory)).ToList();
                List<string> individual_deal_stages_Jsons = crmdata.deal_stages.Select(dealStage => JsonSerializer.Serialize(dealStage)).ToList();
                List<string> individual_sales_accounts_Jsons = crmdata.sales_accounts.Select(salesAccount => JsonSerializer.Serialize(salesAccount)).ToList();
                List<string> individual_deals_Jsons = crmdata.deals.Select(deals => JsonSerializer.Serialize(deals)).ToList();





                // Call the stored procedure 'upsert_crm_deal_types'
                if (individual_dtypes_Jsons != null && individual_dtypes_Jsons.Count > 0)
                {

                    _logger.LogInformation("Calling stored procedure 'upsert_crm_deal_types' with {Count} records.", individual_dtypes_Jsons.Count);
                    int upsertResult = await _context.Database.ExecuteSqlRawAsync("CALL upsert_crm_deal_types(@data)",
                    new NpgsqlParameter("data", NpgsqlDbType.Array | NpgsqlDbType.Jsonb) { Value = individual_dtypes_Jsons });
                    _logger.LogInformation("'upsert_crm_deal_types' completed with result {Result}.", upsertResult);

                }
                // Call the stored procedure 'upsert_crm_deal_reasons'
                if (individual_reasons_Jsons != null && individual_reasons_Jsons.Count > 0)
                {

                    _logger.LogInformation("Calling stored procedure 'upsert_crm_deal_reasons' with {Count} records.", individual_reasons_Jsons.Count);
                    int upsertResult = await _context.Database.ExecuteSqlRawAsync("CALL upsert_crm_deal_reasons(@data)",
                    new NpgsqlParameter("data", NpgsqlDbType.Array | NpgsqlDbType.Jsonb) { Value = individual_reasons_Jsons });
                    _logger.LogInformation("'upsert_crm_deal_reasons' completed with result {Result}.", upsertResult);

                }
                // Call the stored procedure 'upsert_crm_users'
                if (individual_users_Jsons != null && individual_users_Jsons.Count > 0)
                {

                    _logger.LogInformation("Calling stored procedure 'upsert_crm_users' with {Count} records.", individual_users_Jsons.Count);
                    int upsertResult = await _context.Database.ExecuteSqlRawAsync("CALL upsert_crm_users(@data)",
                    new NpgsqlParameter("data", NpgsqlDbType.Array | NpgsqlDbType.Jsonb) { Value = individual_users_Jsons });
                    _logger.LogInformation("'upsert_crm_users' completed with result {Result}.", upsertResult);

                }
                // Call the stored procedure 'upsert_crm_lead_sources'
                if (individual_lead_sources_Jsons != null && individual_lead_sources_Jsons.Count > 0)
                {

                    _logger.LogInformation("Calling stored procedure 'upsert_crm_lead_sources' with {Count} records.", individual_lead_sources_Jsons.Count);
                    int upsertResult = await _context.Database.ExecuteSqlRawAsync("CALL upsert_crm_lead_sources(@data)",
                    new NpgsqlParameter("data", NpgsqlDbType.Array | NpgsqlDbType.Jsonb) { Value = individual_lead_sources_Jsons });
                    _logger.LogInformation("'upsert_crm_lead_sources' completed with result {Result}.", upsertResult);

                }
                // Call the stored procedure 'upsert_crm_territories'
                if (individual_territories_Jsons != null && individual_territories_Jsons.Count > 0)
                {

                    _logger.LogInformation("Calling stored procedure 'upsert_crm_territories' with {Count} records.", individual_territories_Jsons.Count);
                    int upsertResult = await _context.Database.ExecuteSqlRawAsync("CALL upsert_crm_territories(@data)",
                    new NpgsqlParameter("data", NpgsqlDbType.Array | NpgsqlDbType.Jsonb) { Value = individual_territories_Jsons });
                    _logger.LogInformation("'upsert_crm_territories' completed with result {Result}.", upsertResult);

                }
                // Call the stored procedure 'upsert_crm_deal_stages'
                if (individual_deal_stages_Jsons != null && individual_deal_stages_Jsons.Count > 0)
                {

                    _logger.LogInformation("Calling stored procedure 'upsert_crm_deal_stages' with {Count} records.", individual_deal_stages_Jsons.Count);
                    int upsertResult = await _context.Database.ExecuteSqlRawAsync("CALL upsert_crm_deal_stages(@data)",
                    new NpgsqlParameter("data", NpgsqlDbType.Array | NpgsqlDbType.Jsonb) { Value = individual_deal_stages_Jsons });
                    _logger.LogInformation("'upsert_crm_deal_stages' completed with result {Result}.", upsertResult);

                }
                // Call the stored procedure 'upsert_crm_sales_accounts'
                if (individual_sales_accounts_Jsons != null && individual_sales_accounts_Jsons.Count > 0)
                {

                    _logger.LogInformation("Calling stored procedure 'upsert_crm_sales_accounts' with {Count} records.", individual_sales_accounts_Jsons.Count);
                    int upsertResult = await _context.Database.ExecuteSqlRawAsync("CALL upsert_crm_sales_accounts(@data)",
                    new NpgsqlParameter("data", NpgsqlDbType.Array | NpgsqlDbType.Jsonb) { Value = individual_sales_accounts_Jsons });
                    _logger.LogInformation("'upsert_crm_sales_accounts' completed with result {Result}.", upsertResult);

                }
                // Call the stored procedure 'upsert_crm_deals'
                if (individual_deals_Jsons != null && individual_deals_Jsons.Count > 0)
                {

                    _logger.LogInformation("Calling stored procedure 'upsert_crm_deals' with {Count} records.", individual_deals_Jsons.Count);
                    int upsertResult = await _context.Database.ExecuteSqlRawAsync("CALL upsert_crm_deals(@data)",
                    new NpgsqlParameter("data", NpgsqlDbType.Array | NpgsqlDbType.Jsonb) { Value = individual_deals_Jsons });
                    _logger.LogInformation("'upsert_crm_deals' completed with result {Result}.", upsertResult);

                }

                await transaction.CommitAsync();

                       stopwatch.Stop();
                      _logger.LogInformation("UpsertEmployeesBulkData executed successfully in {ElapsedMilliseconds} ms", stopwatch.ElapsedMilliseconds);


                _logger.LogInformation("UpsertCRMBulkData executed successfully in {ElapsedMilliseconds} ms.", stopwatch.ElapsedMilliseconds);
                return true;

            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error during UpsertCRMBulkData at {Time}. Duration: {ElapsedMilliseconds} ms.", DateTime.UtcNow, stopwatch.ElapsedMilliseconds);
                return false;
            }
        }



    }
    
}

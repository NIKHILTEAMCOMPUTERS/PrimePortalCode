using Newtonsoft.Json;
using RMS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Entity.DTO
{
    public class CRMresponseDto
    {
        [JsonProperty("deal_types")]
        public List<DealType>? deal_types { get; set; }
        [JsonProperty("deal_payment_statuses")]
        public List<object>? deal_payment_statuses { get; set; }
        [JsonProperty("deal_reasons")]
        public List<DealReason>? deal_reasons { get; set; }
        [JsonProperty("users")]
        public List<User>? users { get; set; }
        [JsonProperty("sales_activities")]
        public List<object>? sales_activities { get; set; }
        [JsonProperty("lead_sources")]
        public List<LeadSource>? lead_sources { get; set; }
        [JsonProperty("territories")]
        public List<Territory>? territories { get; set; }
        [JsonProperty("deal_stages")]
        public List<DealStage>? deal_stages { get; set; }
        [JsonProperty("currencies")]
        public List<Currency>? currencies { get; set; }
        [JsonProperty("rate_changes")]
        public List<object> ? rate_changes { get; set; }
        [JsonProperty("sales_accounts")]
        public List<SalesAccount>? sales_accounts { get; set; }
        [JsonProperty("deals")]
        public List<Deal>? deals { get; set; }
        [JsonProperty("meta")]
        public Meta? meta { get; set; }

    }
    public class DealType
    {
        [JsonProperty("partial")]
        public bool? partial { get; set; }
        [JsonProperty("id")]
        public long? id { get; set; }
        [JsonProperty("name")]
        public string? name { get; set; }
        [JsonProperty("position")]
        public int? position { get; set; }
    }
    public class DealReason
    {
        [JsonProperty("partial")]
        public bool? partial { get; set; }
        [JsonProperty("id")]
        public long? id { get; set; }
        [JsonProperty("name")]
        public string? name { get; set; }
        [JsonProperty("position")]
        public int? position { get; set; }
    }
    public class User
    {
        [JsonProperty("id")]
        public long? id { get; set; }
        [JsonProperty("display_name")]
        public string? display_name { get; set; }
        [JsonProperty("email")]
        public string? email { get; set; }
        [JsonProperty("is_active")]
        public bool? is_active { get; set; }
        [JsonProperty("work_number")]
        public string? work_number { get; set; }
        [JsonProperty("mobile_number")]
        public string? mobile_number { get; set; }
    }
    public class LeadSource
    {
        [JsonProperty("partial")]
        public bool? partial { get; set; }
        [JsonProperty("id")]
        public long? id { get; set; }
        [JsonProperty("name")]
        public string? name { get; set; }
        [JsonProperty("position")]
        public int? position { get; set; }
    }
    public class Territory
    {
        [JsonProperty("partial")]
        public bool? partial { get; set; }
        [JsonProperty("id")]
        public long? id { get; set; }
        [JsonProperty("name")]
        public string? name { get; set; }
        [JsonProperty("position")]
        public int? position { get; set; }
    }
    public class DealStage
    {
        [JsonProperty("partial")]
        public bool? partial { get; set; }
        [JsonProperty("id")]
        public long? id { get; set; }
        [JsonProperty("name")]
        public string? name { get; set; }
        [JsonProperty("position")]
        public int? position { get; set; }
        [JsonProperty("forecast_type")]
        public string? forecast_type { get; set; }
        [JsonProperty("updated_at")]
        public string? updated_at { get; set; }
        [JsonProperty("deal_pipeline_id")]
        public long? deal_pipeline_id { get; set; }
        [JsonProperty("choice_type")]
        public int? choice_type { get; set; }
        [JsonProperty("probability")]
        public int? probability { get; set; }
    }
    public class Currency
    {
        [JsonProperty("partial")]
        public bool? partial { get; set; }
        [JsonProperty("id")]
        public long? id { get; set; }
        [JsonProperty("is_active")]
        public bool? is_active { get; set; }
        [JsonProperty("currency_code")]
        public string? currency_code { get; set; }
        [JsonProperty("exchange_rate")]
        public string? exchange_rate { get; set; }
        [JsonProperty("currency_type")]
        public int? currency_type { get; set; }
        [JsonProperty("schedule_info")]
        public object? schedule_info { get; set; }
        [JsonProperty("rate_change_ids")]
        public List<object>? rate_change_ids { get; set; }
    }
    public class SalesAccount
    {
        [JsonProperty("partial")]
        public bool? partial { get; set; }
        [JsonProperty("id")]
        public long? id { get; set; }
        [JsonProperty("name")]
        public string? name { get; set; }
        [JsonProperty("avatar")]
        public object? avatar { get; set; }
        [JsonProperty("website")]
        public string? website { get; set; }
        [JsonProperty("open_deals_amount")]
        public string? open_deals_amount { get; set; }
        [JsonProperty("open_deals_count")]
        public int? open_deals_count { get; set; }
        [JsonProperty("won_deals_amount")]
        public string? won_deals_amount { get; set; }
        [JsonProperty("won_deals_count")]
        public int? won_deals_count { get; set; }
        [JsonProperty("last_contacted")]
        public object? last_contacted { get; set; }
    }
    public class Deal
    {
        [JsonProperty("id")]
        public long? id { get; set; }
        [JsonProperty("name")]
        public string? name { get; set; }
        [JsonProperty("amount")]
        public string? amount { get; set; }
        [JsonProperty("base_currency_amount")]
        public string ? base_currency_amount { get; set; }
        [JsonProperty("expected_close")]
        public string? expected_close { get; set; }
        [JsonProperty("closed_date")]
        public string? closed_date { get; set; }
        [JsonProperty("stage_updated_time")]
        public string? stage_updated_time { get; set; }
        [JsonProperty("custom_field")]
        public CustomField? custom_field { get; set; }
        [JsonProperty("probability")]
        public int? probability { get; set; }
        [JsonProperty("updated_at")]
        public string? updated_at { get; set; }
        [JsonProperty("created_at")]
        public string? created_at { get; set; }
        [JsonProperty("deal_pipeline_id")]
        public long? deal_pipeline_id { get; set; }
        [JsonProperty("deal_stage_id")]
        public long? deal_stage_id { get; set; }
        [JsonProperty("age")]
        public int? age { get; set; }
        [JsonProperty("links")]
        public Links? links { get; set; }
        [JsonProperty("recent_note")]
        public object? recent_note { get; set; }
        [JsonProperty("completed_sales_sequences")]
        public object? completed_sales_sequences { get; set; }
        [JsonProperty("active_sales_sequences")]
        public object? active_sales_sequences { get; set; }
        [JsonProperty("web_form_id")]
        public object? web_form_id { get; set; }
        [JsonProperty("upcoming_activities_time")]
        public object? upcoming_activities_time { get; set; }
        [JsonProperty("collaboration")]
        public object? collaboration { get; set; }
        [JsonProperty("last_assigned_at")]
        public string? last_assigned_at { get; set; }
        [JsonProperty("last_contacted_sales_activity_mode")]
        public object? last_contacted_sales_activity_mode { get; set; }
        [JsonProperty("last_contacted_via_sales_activity")]
        public object? last_contacted_via_sales_activity { get; set; }
        [JsonProperty("expected_deal_value")]
        public string? expected_deal_value { get; set; }
        [JsonProperty("is_deleted")]
        public bool? is_deleted { get; set; }
        [JsonProperty("team_user_ids")]
        public object? team_user_ids { get; set; }
        [JsonProperty("avatar")]
        public object? avatar { get; set; }
        [JsonProperty("fc_widget_collaboration")]
        public object? fc_widget_collaboration { get; set; }
        [JsonProperty("forecast_category")]
        public int? forecast_category { get; set; }
        [JsonProperty("deal_prediction")]
        public int? deal_prediction { get; set; }
        [JsonProperty("deal_prediction_last_updated_at")]
        public string? deal_prediction_last_updated_at { get; set; }
        [JsonProperty("freddy_forecast_metrics")]
        public object? freddy_forecast_metrics { get; set; }
        [JsonProperty("last_deal_prediction")]
        public object? last_deal_prediction { get; set; }
        [JsonProperty("has_products")]
        public bool? has_products { get; set; }
        [JsonProperty("products")]
        public List<Product>? products { get; set; }
        [JsonProperty("deal_price_adjustments")]
        public List<object>? deal_price_adjustments { get; set; }
        [JsonProperty("rotten_days")]
        public object? rotten_days { get; set; }
        [JsonProperty("tags")]
        public List<object>? tags { get; set; }
        [JsonProperty("deal_type_id")]
        public object? deal_type_id { get; set; }
        [JsonProperty("deal_payment_status_id")]
        public object? deal_payment_status_id { get; set; }
        [JsonProperty("deal_reason_id")]
        public object? deal_reason_id { get; set; }
        [JsonProperty("creater_id")]
        public long? creater_id { get; set; }
        [JsonProperty("updater_id")]
        public long? updater_id { get; set; }
        [JsonProperty("sales_activity_ids")]
        public List<object>? sales_activity_ids { get; set; }
        [JsonProperty("owner_id")]
        public long? owner_id { get; set; }
        [JsonProperty("lead_source_id")]
        public long? lead_source_id { get; set; }
        [JsonProperty("territory_id")]
        public long? territory_id { get; set; }
        [JsonProperty("currency_id")]
        public long? currency_id { get; set; }
        [JsonProperty("sales_account_id")]
        public long? sales_account_id { get; set; }
    }


    public class CustomField
    {
        [JsonProperty("cf_deal_value_in_lacs")]
        public object? cf_deal_value_in_lacs { get; set; }

        [JsonProperty("cf_product_description")]
        public object? cf_product_description { get; set; }

        [JsonProperty("cf_address")]
        public object? cf_address { get; set; }

        [JsonProperty("cf_zipcode")]
        public object? cf_zipcode { get; set; }

        [JsonProperty("cf_proposal_status")]
        public string? cf_proposal_status { get; set; }

        [JsonProperty("cf_type_of_opportunity")]
        public string? cf_type_of_opportunity { get; set; }

        [JsonProperty("cf_country")]
        public object? cf_country { get; set; }

        [JsonProperty("cf_state")]
        public object? cf_state { get; set; }

        [JsonProperty("cf_city")]
        public object? cf_city { get; set; }

        [JsonProperty("cf_gst_number")]
        public object? cf_gst_number { get; set; }

        [JsonProperty("cf_organisation_pan")]
        public object? cf_organisation_pan { get; set; }

        [JsonProperty("cf_bu")]
        public string? cf_bu { get; set; }

        [JsonProperty("cf_sub_bu")]
        public string? cf_sub_bu { get; set; }

        [JsonProperty("cf_inside_sales_owner_for_each_deal")]
        public object? cf_inside_sales_owner_for_each_deal { get; set; }

        [JsonProperty("cf_pocs_phone")]
        public object? cf_pocs_phone { get; set; }

        [JsonProperty("cf_pocs_email")]
        public object? cf_pocs_email { get; set; }

        [JsonProperty("cf_additional_remarks")]
        public object? cf_additional_remarks { get; set; }

        [JsonProperty("cf_followup_date")]
        public string? cf_followup_date { get; set; }

        [JsonProperty("cf_bmr_leadtest")]
        public object? cf_bmr_leadtest { get; set; }

        [JsonProperty("cf_bu_sample")]
        public object? cf_bu_sample { get; set; }

        [JsonProperty("cf_primary_deal")]
        public object? cf_primary_deal { get; set; }

        [JsonProperty("cf_opp_name")]
        public object? cf_opp_name { get; set; }

        [JsonProperty("cf_sure_shot")]
        public object? cf_sure_shot { get; set; }

        [JsonProperty("cf_bu_for_reports")]
        public object? cf_bu_for_reports { get; set; }

        [JsonProperty("cf_is_this_is_secondary_deal")]
        public bool? cf_is_this_is_secondary_deal { get; set; }

        [JsonProperty("cf_qty")]
        public object? cf_qty { get; set; }

        [JsonProperty("cf_region")]
        public object? cf_region { get; set; }

        [JsonProperty("cf_oem_name")]
        public object? cf_oem_name { get; set; }

        [JsonProperty("cf_regional_prosuct_manager")]
        public object? cf_regional_prosuct_manager { get; set; }

        [JsonProperty("cf_product_manager")]
        public object? cf_product_manager { get; set; }

        [JsonProperty("cf_stage")]
        public object? cf_stage { get; set; }

        [JsonProperty("cf_eup")]
        public object? cf_eup { get; set; }

        [JsonProperty("cf_deal_product_manager")]
        public object? cf_deal_product_manager { get; set; }

        [JsonProperty("cf_cost_price")]
        public object? cf_cost_price { get; set; }

        [JsonProperty("cf_quantity")]
        public object? cf_quantity { get; set; }

        [JsonProperty("cf_hsn_code")]
        public object? cf_hsn_code { get; set; }

        [JsonProperty("cf_dimension")]
        public object? cf_dimension { get; set; }

        [JsonProperty("cf_model_number")]
        public object? cf_model_number { get; set; }

        [JsonProperty("cf_edd")]
        public object? cf_edd { get; set; }

        [JsonProperty("cf_competition_vendoroem")]
        public object? cf_competition_vendoroem { get; set; }

        [JsonProperty("cf_compete_factors")]
        public object? cf_compete_factors { get; set; }

        [JsonProperty("cf_part_no")]
        public object? cf_part_no { get; set; }

        [JsonProperty("cf_sow_is_attached")]
        public object? cf_sow_is_attached { get; set; }

        [JsonProperty("cf_win__loss_type")]
        public object? cf_win__loss_type { get; set; }

        [JsonProperty("cf_previous_pipeline_stage")]
        public object? cf_previous_pipeline_stage { get; set; }

        [JsonProperty("cf_margin")]
        public object? cf_margin { get; set; }

        [JsonProperty("cf_valuein_lacs")]
        public object? cf_valuein_lacs { get; set; }

        [JsonProperty("cf_proposal_verification_by_bu_head")]
        public object? cf_proposal_verification_by_bu_head { get; set; }

        [JsonProperty("cf_approval_from_sales_head")]
        public object? cf_approval_from_sales_head { get; set; }

        [JsonProperty("cf_approval_attached")]
        public object? cf_approval_attached { get; set; }

        [JsonProperty("cf_attached_proposal")]
        public object? cf_attached_proposal { get; set; }

        [JsonProperty("cf_financial_bu_approval")]
        public object? cf_financial_bu_approval { get; set; }

        [JsonProperty("cf_delivery_head_go_ahead_received")]
        public object? cf_delivery_head_go_ahead_received { get; set; }

        [JsonProperty("cf_financial_bu_approval_file_atteched")]
        public object? cf_financial_bu_approval_file_atteched { get; set; }

        [JsonProperty("cf_bdm_document_attached")]
        public object? cf_bdm_document_attached { get; set; }

        [JsonProperty("cf_sow_attached")]
        public object? cf_sow_attached { get; set; }

        [JsonProperty("cf_won_reasons")]
        public object? cf_won_reasons { get; set; }

        [JsonProperty("cf_sales_process_document_filled")]
        public object? cf_sales_process_document_filled { get; set; }

        [JsonProperty("cf_confirmation_from_delivery_on_sow")]
        public object? cf_confirmation_from_delivery_on_sow { get; set; }

        [JsonProperty("cf_description")]
        public object? cf_description { get; set; }

        [JsonProperty("cf_presales_owner_for_each_deal")]
        public object? cf_presales_owner_for_each_deal { get; set; }
    }

    public class Links
    {
        [JsonProperty("conversations")]
        public string? conversations { get; set; }

        [JsonProperty("document_associations")]
        public string? document_associations { get; set; }

        [JsonProperty("notes")]
        public string? notes { get; set; }

        [JsonProperty("tasks")]
        public string? tasks { get; set; }

        [JsonProperty("appointments")]
        public string? appointments { get; set; }
    }


    public class Product
    {
        [JsonProperty("product_id")]
        public long? product_id { get; set; }

        [JsonProperty("name")]
        public string? name { get; set; }

        [JsonProperty("unit_price")]
        public double? unit_price { get; set; }

        [JsonProperty("setup_fee")]
        public object? setup_fee { get; set; }

        [JsonProperty("billing_type")]
        public object? billing_type { get; set; }

        [JsonProperty("billing_cycle")]
        public object? billing_cycle { get; set; }

        [JsonProperty("currency_code")]
        public string? currency_code { get; set; }

        [JsonProperty("quantity")]
        public int? quantity { get; set; }

        [JsonProperty("discount")]
        public double? discount { get; set; }

        [JsonProperty("discount_type")]
        public int? discount_type { get; set; }

        [JsonProperty("deal_id")]
        public long? deal_id { get; set; }

        [JsonProperty("association_id")]
        public long? association_id { get; set; }

        [JsonProperty("_id")]
        public string? _id { get; set; }

        [JsonProperty("pricing_type")]
        public int? pricing_type { get; set; }

        [JsonProperty("avatar")]
        public object? avatar { get; set; }

        [JsonProperty("owner_id")]
        public object? owner_id { get; set; }

        [JsonProperty("category")]
        public string? category { get; set; }

        [JsonProperty("adjustments")]
        public List<object>? adjustments { get; set; }
    }

    public class Meta
    {
        [JsonProperty("total_pages")]
        public int? total_pages { get; set; }
        [JsonProperty("total")]
        public int? total { get; set; }
    }


}

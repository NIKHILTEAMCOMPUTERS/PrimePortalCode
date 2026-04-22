using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using RMS.Data.Models;
using RMS.Entity.DTO;
using RMS.Extensions;
using RMS.Service.Interfaces;
using System.Security.Claims;


namespace RMS.Controllers.Master
{
    public  class ReportController: BaseApiController
    {
        private readonly IUnitOfWork _uow;
        public ReportController(IUnitOfWork uow)
        {
            _uow = uow;
        }    

       
        [HttpGet("GetBillingRptMonth")]
        public async Task<IActionResult> GetBillingRptMonth( string monthyear)
        {
           if (monthyear == null) { return BadRequest("Month year can not be null"); }
            var Results= await _uow.ReportRepository.GetReportByMonthYearProcedure(monthyear);
           return  Ok(Results);
        }
        [HttpGet("GetBillingRptMonthDA")]
        public async Task<IActionResult> GetBillingRptMonthDA(string monthyear)
        {
            if (monthyear == null) { return BadRequest("Month year can not be null"); }
            var Results = await _uow.ReportRepository.GetReportByMonthYearProcedure_DA_Wise(monthyear);
            return Ok(Results);
        }
        [HttpPost("GetProvisionRptMonth")]
        public async Task<IActionResult> GetProvisionRptMonth([FromBody]InputForrpt obj)
        {
            var result =await  _uow.ReportRepository.GetRptProvisionHistory(obj.monthyear, obj.contractid);
            return Ok(result);
        }
        [HttpGet("GetRptMonthForBilled")]
        public async Task<IActionResult> GetBillingRptMonthProvision(string monthyear, int? DeliveryAnchorId)
        {
            if (monthyear == null) { return BadRequest("Month year can not be null"); }
            var Results = await _uow.ReportRepository.GetReportByMonthYearProcedureProvision(monthyear, DeliveryAnchorId);
            return Ok(Results);
        }
        [HttpGet("GetRptMonthForBilled-TobeBilled")]
        public async Task<IActionResult> GetRptMonthForBilledTobeBilled(string monthyear,int? DeliveryAnchorId)
        {
            if (monthyear == null) { return BadRequest("Month year can not be null"); }
            var Results = await _uow.ReportRepository.GetReportByMonthYearProcedureActualBilling(monthyear, DeliveryAnchorId);
            return Ok(Results);
        }
        [HttpGet("BillingDropDown")]
        public async Task<IActionResult> BillingDropDown()
        {
          return Ok(  await  _uow.ReportRepository.BillingDropDown());
        }
        [HttpGet("GetResourcesInfoForHR")]
        public async Task<IActionResult> GetResourcesInfoForHR()
        {
            
            var Results = await _uow.ReportRepository.GetResourcesInfoForHR();
            return Ok(Results);
        }

        
    }
   
}
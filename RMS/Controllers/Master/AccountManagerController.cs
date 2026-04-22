using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RMS.Data.Models;
using RMS.Entity.DTO;
using RMS.Service.Interfaces;
using RMS.Service.Repositories.Master;

namespace RMS.Controllers.Master
{
    public class AccountManagerController : BaseApiController
    {
        private readonly IUnitOfWork _uow;
        public AccountManagerController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result= await _uow.AccountManagerRepository.GetAccountManagerList();
            if (result == null) { return NotFound(); }
            else
            {
                return Ok(result);
            }
        }
        [HttpGet("DADropDown")]
        public async Task<IActionResult> GetDADropDown()
        {
            var result = await _uow.AccountManagerRepository.GetDAList();
            if (result == null) { return NotFound(); }
            else
            {
                return Ok(result);
            }
        }
        [HttpPost("BilledAmountSubmission")]
        public async Task<IActionResult> SubmitProvisionBilling(List<ProvisionBillingSubmittingDataDto> obj)
        {
            if (obj == null ||obj.Count<=0) { return NotFound(); }
            var result = await _uow.AccountManagerRepository.ProvisionBillingSubmission(obj);
            return Ok(result);
        }

    }
    
}

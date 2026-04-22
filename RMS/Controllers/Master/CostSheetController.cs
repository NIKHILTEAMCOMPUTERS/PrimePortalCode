using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using RMS.Data.Models;
using RMS.Entity.DTO;
using RMS.Extensions;
using RMS.Service.Interfaces;
using System.Security.Claims;

namespace RMS.Controllers.Master
{
    public class CostSheetController : BaseApiController
    {
        private readonly IUnitOfWork _uow;
        public CostSheetController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpGet]
        public async Task<IEnumerable<CostSheetDto>> Get()
        {
            return await _uow.CostSheetRepository.GetCostSheetWithDetails();   
        }

        [HttpGet("{id}")]
        public async Task<CostSheetDto> Get(int id)
        {
            return await _uow.CostSheetRepository.GetCostSheetById(id);
        }


        [HttpPost, Authorize]
        public async Task<CostSheetDto> Post([FromBody] CostSheetDto value)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity == null)
            {
                throw new Exception("Authentication Fails");
            }
            var LoginDetails = AuthenticUserDetails.GetCurrentUserDetails(identity);
            return await _uow.CostSheetRepository.UpsertCostsheet(value, LoginDetails);
            //return await _uow.Complete();
        }

        [HttpPut("{id}"), Authorize]
        public async Task<CostSheetDto> Put([FromBody] CostSheetDto value)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity == null)
            {
                throw new Exception("Authentication Fails");
            }
            var LoginDetails = AuthenticUserDetails.GetCurrentUserDetails(identity);
            return await _uow.CostSheetRepository.UpsertCostsheet(value, LoginDetails);
           // return await _uow.Complete();
        }

        [HttpDelete("{id}")]
        public async Task<Response> Delete(int id)
        {
            return await _uow.CostSheetRepository.DeleteCostsheet(id);
        }

 

        [HttpPost("CheckCostsheet")]
        public async Task<IActionResult> CheckCostsheet([FromBody] CostSheetCheckDto DtoObject)
        {

            var resut = await _uow.CostSheetRepository.CheckCostsheet(DtoObject.CostSheetid,DtoObject.CostSheetName);
            return Ok(resut);
        }
    }
}
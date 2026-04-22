using Microsoft.AspNetCore.Mvc;
using RMS.Data.Models;
using RMS.Service.Interfaces;

namespace RMS.Controllers.Master
{
    public class CityController : BaseApiController
    {
        private readonly IUnitOfWork _uow;
        public CityController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpGet]
        public async Task<IEnumerable<City>> Get()
        {
            return await _uow.CityRepository.GetAsync();
        }

        [HttpGet("GetAllCRMdata")]
        public async Task<IActionResult> GetAllCRMdata()
        {
            var result= await _uow.CityRepository.GetCRMdataAsyncAsync();
            return Ok(result);
        }


        [HttpGet("{id}")]
        public async Task<City> Get(int id)
        {
            return await _uow.CityRepository.GetByIdAsync(id);
        }
        [HttpGet("GetCitiesByStateId")]
        public async Task<IActionResult> GetCitiesByStateId(int StateId)
        {
            var result = await _uow.CityRepository.GetCitiesByStateId(StateId);
            return Ok(result);

        }

        [HttpPost]
        public async Task<bool> Post([FromBody] City value)
        {
            _uow.CityRepository.Add(value);
            return await _uow.Complete();
        }

        [HttpPut("{id}")]
        public async Task<bool> Put([FromBody] City value)
        {
            _uow.CityRepository.Update(value);
            return await _uow.Complete();
        }

        [HttpDelete("{id}")]
        public async Task<bool> Delete(int id)
        {
            _uow.CityRepository.Delete(id);
            return await _uow.Complete();
        }
    }
}
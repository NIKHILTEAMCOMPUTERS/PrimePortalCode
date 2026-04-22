using Microsoft.AspNetCore.Mvc;
using RMS.Data.Models;
using RMS.Service.Interfaces;

namespace RMS.Controllers.Master
{
    public class StateController : BaseApiController 
    {
        private readonly IUnitOfWork _uow;
        public StateController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpGet]
        public async Task<IEnumerable<State>> Get()
        {
            return await _uow.StateRepository.GetAsync();
        }

        [HttpGet("{id}")]
        public async Task<State> Get(int id)
        {
            return await _uow.StateRepository.GetByIdAsync(id);
        }
        [HttpGet("GeStatesByCountryId")]
        public async Task<IActionResult> GetStatesByCountryId(int CountryId)
        {
            var result = await _uow.StateRepository.GetStateByCountryId(CountryId);
            return Ok(result);
        }

        [HttpPost]
        public async Task<bool> Post([FromBody] State value)
        {
            _uow.StateRepository.Add(value);
            return await _uow.Complete();
        }

        [HttpPut("{id}")]
        public async Task<bool> Put([FromBody] State value)
        {
            _uow.StateRepository.Update(value);
            return await _uow.Complete();
        }

        [HttpDelete("{id}")]
        public async Task<bool> Delete(int id)
        {
            _uow.StateRepository.Delete(id);
            return await _uow.Complete();
        }
    }
}

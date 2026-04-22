using Microsoft.AspNetCore.Mvc;
using RMS.Data.Models;
using RMS.Service.Interfaces;

namespace RMS.Controllers.Master
{
    public class CurrencyController : BaseApiController
    {
        private readonly IUnitOfWork _uow;
        public CurrencyController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpGet]
        public async Task<IEnumerable<Currency>> Get()
        {
            return await _uow.CurrencyRepository.GetAsync();
        }

        [HttpGet("{id}")]
        public async Task<Currency> Get(int id)
        {
            return await _uow.CurrencyRepository.GetByIdAsync(id);
        }

        [HttpPost]
        public async Task<bool> Post([FromBody] Currency value)
        {
            _uow.CurrencyRepository.Add(value);
            return await _uow.Complete();
        }

        [HttpPut("{id}")]
        public async Task<bool> Put([FromBody] Currency value)
        {
            _uow.CurrencyRepository.Update(value);
            return await _uow.Complete();
        }

        [HttpDelete("{id}")]
        public async Task<bool> Delete(int id)
        {
            _uow.CurrencyRepository.Delete(id);
            return await _uow.Complete();
        }
    }
}
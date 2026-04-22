using Microsoft.AspNetCore.Mvc;
using RMS.Data.Models;
using RMS.Service.Interfaces;

namespace RMS.Controllers.Master
{
    public class CountryController : BaseApiController
    {
        private readonly IUnitOfWork _uow;
        public CountryController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpGet]
        public async Task<IEnumerable<Country>> Get()
        {
            return await _uow.CountryRepository.GetAsync();
        }

        [HttpGet("{id}")]
        public async Task<Country> Get(int id)
        {
            return await _uow.CountryRepository.GetByIdAsync(id);
        }

        [HttpPost]
        public async Task<bool> Post([FromBody] Country value)
        {
            _uow.CountryRepository.Add(value);
            return await _uow.Complete();
        }

        [HttpPut("{id}")]
        public async Task<bool> Put([FromBody] Country value)
        {
            _uow.CountryRepository.Update(value);
            return await _uow.Complete();
        }

        [HttpDelete("{id}")]
        public async Task<bool> Delete(int id)
        {
            _uow.CountryRepository.Delete(id);
            return await _uow.Complete();
        }
    }
}
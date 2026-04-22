using Microsoft.AspNetCore.Mvc;
using RMS.Data.Models;
using RMS.Service.Interfaces;

namespace RMS.Controllers.Master
{
    public class ContractTypeController : BaseApiController
    {
        private readonly IUnitOfWork _uow;
        public ContractTypeController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpGet]
        public async Task<IEnumerable<Contracttype>> Get()
        {
            return await _uow.ContractTypeRepository.GetAsync();
        }

        [HttpGet("{id}")]
        public async Task<Contracttype> Get(int id)
        {
            return await _uow.ContractTypeRepository.GetByIdAsync(id);
        }
        

        [HttpPost]
        public async Task<bool> Post([FromBody] Contracttype value)
        {
            _uow.ContractTypeRepository.Add(value);
            return await _uow.Complete();
        }

        [HttpPut("{id}")]
        public async Task<bool> Put([FromBody] Contracttype value)
        {
            _uow.ContractTypeRepository.Update(value);
            return await _uow.Complete();
        }

        [HttpDelete("{id}")]
        public async Task<bool> Delete(int id)
        {
            _uow.ContractTypeRepository.Delete(id);
            return await _uow.Complete();
        }
    }
}
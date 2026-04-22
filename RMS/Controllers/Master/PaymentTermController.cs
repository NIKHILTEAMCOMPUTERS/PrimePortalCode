using Microsoft.AspNetCore.Mvc;
using RMS.Data.Models;
using RMS.Service.Interfaces;

namespace RMS.Controllers.Master
{
    public class PaymentTermController : BaseApiController
    {
        private readonly IUnitOfWork _uow;
        public PaymentTermController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpGet]
        public async Task<IEnumerable<Paymentterm>> Get()
        {
            return await _uow.PaymentTermRepository.GetAsync();
        }

        [HttpGet("{id}")]
        public async Task<Paymentterm> Get(int id)
        {
            return await _uow.PaymentTermRepository.GetByIdAsync(id);
        }

        [HttpPost]
        public async Task<bool> Post([FromBody] Paymentterm value)
        {
            _uow.PaymentTermRepository.Add(value);
            return await _uow.Complete();
        }

        [HttpPut("{id}")]
        public async Task<bool> Put([FromBody] Paymentterm value)
        {
            _uow.PaymentTermRepository.Update(value);
            return await _uow.Complete();
        }

        [HttpDelete("{id}")]
        public async Task<bool> Delete(int id)
        {
            _uow.PaymentTermRepository.Delete(id);
            return await _uow.Complete();
        }
    }
}
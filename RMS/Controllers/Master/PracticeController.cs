using Microsoft.AspNetCore.Mvc;
using RMS.Data.Models;
using RMS.Service.Interfaces;

namespace RMS.Controllers.Master
{
    public class PracticeController : BaseApiController
    {
        private readonly IUnitOfWork _uow;
        public PracticeController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpGet]
        public async Task<IEnumerable<Practice>> Get()
        {
            var result= await _uow.PracticeRepository.GetAsync();
            //result= result.Where(x=>x.Practiceid==30).ToList();
            return result;
        }

        [HttpGet("{id}")]
        public async Task<Practice> Get(int id)
        {
            return await _uow.PracticeRepository.GetByIdAsync(id);
        }

        [HttpPost]
        public async Task<bool> Post([FromBody] Practice value)
        {
            _uow.PracticeRepository.Add(value);
            return await _uow.Complete();
        }

        [HttpPut("{id}")]
        public async Task<bool> Put([FromBody] Practice value)
        {
            _uow.PracticeRepository.Update(value);
            return await _uow.Complete();
        }

        [HttpDelete("{id}")]
        public async Task<bool> Delete(int id)
        {
            _uow.PracticeRepository.Delete(id);
            return await _uow.Complete();
        }
    }
}
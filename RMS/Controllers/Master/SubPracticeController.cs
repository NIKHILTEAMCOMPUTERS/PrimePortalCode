using Microsoft.AspNetCore.Mvc;
using RMS.Data.Models;
using RMS.Service.Interfaces;

namespace RMS.Controllers.Master
{
    public class SubPracticeController : BaseApiController
    {
        private readonly IUnitOfWork _uow;
        public SubPracticeController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpGet]
        public async Task<IEnumerable<Subpractice>> Get()
        {
            return await _uow.SubpracticeRepository.GetAsync();
        }

        [HttpGet("{id}")]
        public async Task<Subpractice> Get(int id)
        {
            return await _uow.SubpracticeRepository.GetByIdAsync(id);
        }
        [HttpGet("GetSubPracticeByPracticeId")]
        public async Task<IActionResult> GetCitiesByStateId(int PracticeId)
        {
            var result = await _uow.SubpracticeRepository.GetSubpracticeByPracticeId(PracticeId);
            return Ok(result);

        }

        [HttpPost]
        public async Task<bool> Post([FromBody] Subpractice value)
        {
            _uow.SubpracticeRepository.Add(value);
            return await _uow.Complete();
        }

        [HttpPut("{id}")]
        public async Task<bool> Put([FromBody] Subpractice value)
        {
            _uow.SubpracticeRepository.Update(value);
            return await _uow.Complete();
        }

        [HttpDelete("{id}")]
        public async Task<bool> Delete(int id)
        {
            _uow.SubpracticeRepository.Delete(id);
            return await _uow.Complete();
        }
    }
}
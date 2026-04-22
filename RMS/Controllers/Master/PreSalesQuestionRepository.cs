using Microsoft.AspNetCore.Mvc;
using RMS.Data.Models;
using RMS.Service.Interfaces;

namespace RMS.Controllers.Master
{
    public class PreSalesQuestionRepository : BaseApiController
    {
        private readonly IUnitOfWork _uow;
        public PreSalesQuestionRepository(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpGet]
        public async Task<IEnumerable<Presalesquestionmaster>> Get()
        {
            return await _uow.PreSalesQuestionRepository.GetAsync();
        }

        [HttpGet("{id}")]
        public async Task<Presalesquestionmaster> Get(int id)
        {
            return await _uow.PreSalesQuestionRepository.GetByIdAsync(id);
        }
        [HttpGet("GetQuesByPracticeId")]
        public async Task<IActionResult> GetQuesByPracticeId(int PracticeId)
        {
            var result = await _uow.PreSalesQuestionRepository.GetQuestionsByPractice(PracticeId);
            return Ok(result);

        }

        [HttpPost]
        public async Task<bool> Post([FromBody] Presalesquestionmaster value)
        {
            _uow.PreSalesQuestionRepository.Add(value);
            return await _uow.Complete();
        }

        [HttpPut("{id}")]
        public async Task<bool> Put([FromBody] Presalesquestionmaster value)
        {
            _uow.PreSalesQuestionRepository.Update(value);
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
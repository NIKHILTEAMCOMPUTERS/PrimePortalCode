using Microsoft.AspNetCore.Mvc;
using RMS.Data.Models;
using RMS.Service.Interfaces;

namespace RMS.Controllers.Master
{
    public class CategoryStatusController : BaseApiController
    {
        private readonly IUnitOfWork _uow;
        public CategoryStatusController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpGet]
        public async Task<IEnumerable<Categorystatus>> Get()
        {
            return await _uow.CategoryStatusRepository.GetAsync();
        }

        [HttpGet("{id}")]
        public async Task<Categorystatus> Get(int id)
        {
            return await _uow.CategoryStatusRepository.GetByIdAsync(id);
        }      

        [HttpPost]
        public async Task<bool> Post([FromBody] Categorystatus value)
        {
            _uow.CategoryStatusRepository.Add(value);
            return await _uow.Complete();
        }

        [HttpPut("{id}")]
        public async Task<bool> Put([FromBody] Categorystatus value)
        {
            _uow.CategoryStatusRepository.Update(value);
            return await _uow.Complete();
        }

        [HttpDelete("{id}")]
        public async Task<bool> Delete(int id)
        {
            _uow.CategoryStatusRepository.Delete(id);
            return await _uow.Complete();
        }
    }
}
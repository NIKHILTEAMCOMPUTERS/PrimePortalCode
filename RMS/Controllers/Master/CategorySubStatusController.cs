using Microsoft.AspNetCore.Mvc;
using RMS.Data.Models;
using RMS.Service.Interfaces;

namespace RMS.Controllers.Master
{
    public class CategorySubStatusController : BaseApiController
    {
        private readonly IUnitOfWork _uow;
        public CategorySubStatusController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpGet]
        public async Task<IEnumerable<Categorysubstatus>> Get()
        {
            return await _uow.CategorySubStatusRepository.GetAsync();
        }

        [HttpGet("{id}")]
        public async Task<Categorysubstatus> Get(int id)
        {
            return await _uow.CategorySubStatusRepository.GetByIdAsync(id);
        }
        [HttpGet("GetSubCategoryByCategoryId/{CategoryId:int}")]
        public async Task<IEnumerable<Categorysubstatus>> GetSubCategoryByCategoryId(int CategoryId)
        {
            return await _uow.CategorySubStatusRepository.GetSubCategoryByCategoryId(CategoryId);
        }

        [HttpPost]
        public async Task<bool> Post([FromBody] Categorysubstatus value)
        {
            _uow.CategorySubStatusRepository.Add(value);
            return await _uow.Complete();
        }

        [HttpPut("{id}")]
        public async Task<bool> Put([FromBody] Categorysubstatus value)
        {
            _uow.CategorySubStatusRepository.Update(value);
            return await _uow.Complete();
        }

        [HttpDelete("{id}")]
        public async Task<bool> Delete(int id)
        {
            _uow.CategorySubStatusRepository.Delete(id);
            return await _uow.Complete();
        }
    }
}
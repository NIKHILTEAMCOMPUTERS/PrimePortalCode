using Microsoft.AspNetCore.Mvc;
using RMS.Data.Models;
using RMS.Service.Interfaces;

namespace RMS.Controllers.Master
{
    public class ProjectModelController : BaseApiController
    {
        private readonly IUnitOfWork _uow;
        public ProjectModelController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpGet]
        public async Task<IEnumerable<Projectmodel>> Get()
        {
            return await _uow.ProjectModelRepository.GetAsync();
        }

        [HttpGet("{id}")]
        public async Task<Projectmodel> Get(int id)
        {
            return await _uow.ProjectModelRepository.GetByIdAsync(id);
        }

        [HttpPost]
        public async Task<bool> Post([FromBody] Projectmodel value)
        {
            _uow.ProjectModelRepository.Add(value);
            return await _uow.Complete();
        }

        [HttpPut("{id}")]
        public async Task<bool> Put([FromBody] Projectmodel value)
        {
            _uow.ProjectModelRepository.Update(value);
            return await _uow.Complete();
        }

        [HttpDelete("{id}")]
        public async Task<bool> Delete(int id)
        {
            _uow.ProjectModelRepository.Delete(id);
            return await _uow.Complete();
        }
    }
}
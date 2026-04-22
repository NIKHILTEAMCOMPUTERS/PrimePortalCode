using Microsoft.AspNetCore.Mvc;
using RMS.Data.Models;
using RMS.Service.Interfaces;

namespace RMS.Controllers.Master
{
    public class ProjectTypeController : BaseApiController
    {
        private readonly IUnitOfWork _uow;
        public ProjectTypeController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpGet]
        public async Task<IEnumerable<Projecttype>> Get()
        {
            return await _uow.ProjectTypeRepository.GetAsync();
        }

        [HttpGet("{id}")]
        public async Task<Projecttype> Get(int id)
        {
            return await _uow.ProjectTypeRepository.GetByIdAsync(id);
        }

        [HttpPost]
        public async Task<bool> Post([FromBody] Projecttype value)
        {
            _uow.ProjectTypeRepository.Add(value);
            return await _uow.Complete();
        }

        [HttpPut("{id}")]
        public async Task<bool> Put([FromBody] Projecttype value)
        {
            _uow.ProjectTypeRepository.Update(value);
            return await _uow.Complete();
        }

        [HttpDelete("{id}")]
        public async Task<bool> Delete(int id)
        {
            _uow.ProjectTypeRepository.Delete(id);
            return await _uow.Complete();
        }
    }
}
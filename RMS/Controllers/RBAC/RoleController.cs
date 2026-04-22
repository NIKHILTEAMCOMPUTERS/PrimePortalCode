using Microsoft.AspNetCore.Mvc;
using RMS.Data.Models;
using RMS.Service.Interfaces;

namespace RMS.Controllers.RBAC
{
    public class RoleController : BaseApiController
    {
        private readonly IUnitOfWork _uow;
        public RoleController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpGet]
        public async Task<IEnumerable<Role>> Get()
        {
            return await _uow.RoleRepository.GetAll();
        }

        [HttpGet("{id}")]
        public async Task<Role> Get(int id)
        {
            return await _uow.RoleRepository.GetById(id);
        }

        [HttpPost]
        public async Task<bool> Post([FromBody] Role value)
        {
            _uow.RoleRepository.Add(value);
            return await _uow.Complete();
        }

        [HttpPut("{id}")]
        public async Task<bool> Put([FromBody] Role value)
        {
            _uow.RoleRepository.Update(value);
            return await _uow.Complete();
        }

        [HttpDelete("{id}")]
        public async Task<bool> Delete(int id)
        {
            _uow.RoleRepository.Delete(id);
            return await _uow.Complete();
        }
    }
}

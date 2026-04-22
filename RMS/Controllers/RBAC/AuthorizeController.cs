using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.Data.Models;
using RMS.Entity.DTO;
using RMS.Service.Interfaces;

namespace RMS.Controllers.RBAC
{
    //[Authorize]
    public class AuthorizeController : BaseApiController
    {
        private readonly IUnitOfWork _uow;
        public AuthorizeController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpGet]
        public async Task<AuthorizeDto> Get(string empCode)
        {
            return await _uow.AuthorizeRepository.Get(empCode);
        }

        [HttpPost]
        public async Task<bool> Post([FromBody] Employeerole value)
        {
            _uow.AuthorizeRepository.Add(value);
            return await _uow.Complete();
        }

        [HttpPut("{id}")]
        public async Task<bool> Put([FromBody] Employeerole value)
        {
            _uow.AuthorizeRepository.Update(value);
            return await _uow.Complete();
        }

        [HttpDelete("{id}")]
        public async Task<bool> Delete(int id)
        {
            _uow.AuthorizeRepository.Delete(id);
            return await _uow.Complete();
        }

    }
}

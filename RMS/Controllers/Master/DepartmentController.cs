using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RMS.Data.Models;
using RMS.Service.Interfaces;
using RMS.Service.Repositories.Master;

namespace RMS.Controllers.Master
{
    public class DepartmentController : BaseApiController
    {
        private readonly IUnitOfWork _uow;
        public DepartmentController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpGet]
        public async Task<IEnumerable<Department>> Get()
        {
            return await _uow.DepartmentRepository.GetAsync();
        }

    }
}

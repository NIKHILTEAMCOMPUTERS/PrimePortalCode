using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RMS.Data.Models;
using RMS.Service.Interfaces;
using RMS.Service.Repositories.Master;

namespace RMS.Controllers.Master
{
    public class DesignationController : BaseApiController
    {
        private readonly IUnitOfWork _uow;
        public DesignationController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpGet]
        public async Task<IEnumerable<Designation>> Get()
        {
            return await _uow.DesignationRepository.GetAsync();
        }

    }
}

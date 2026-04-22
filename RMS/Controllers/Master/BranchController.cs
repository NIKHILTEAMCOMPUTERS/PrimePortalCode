using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RMS.Data.Models;
using RMS.Service.Interfaces;
using RMS.Service.Repositories.Master;

namespace RMS.Controllers.Master
{
    public class BranchController : BaseApiController
    {
        private readonly IUnitOfWork _uow;
        public BranchController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpGet]
        public async Task<IEnumerable<Branch>> Get()
        {
            return await _uow.BranchRepository.GetAsync();
        }

    }
}

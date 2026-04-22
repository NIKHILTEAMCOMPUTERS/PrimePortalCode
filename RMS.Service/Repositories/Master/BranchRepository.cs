using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using RMS.Data.Models;
using RMS.Entity.DTO;
using RMS.Service.Interfaces.Master;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Security.Cryptography.X509Certificates;

namespace RMS.Service.Repositories.Master
{
    public class BranchRepository : GenericRepository<Branch>, IBranchRepository
    {
        private readonly RmsDevContext _context;
        private readonly IHostingEnvironment _env;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public BranchRepository(RmsDevContext context, IHostingEnvironment env, IHttpContextAccessor httpContextAccessor) : base(context)
        {
            _context = context;
            _env = env;
            _httpContextAccessor = httpContextAccessor;

        }

        public async Task<List<Branch>> GetBranch()
        {
            var result = new List<Branch>();
            try
            {

                result = await _context.Branches.AsNoTracking().Where(x => x.Isdeleted != true && x.Isactive != false)
                                          .ToListAsync();

                return result;


            }
            catch (Exception e)
            {
                throw;

            }

        }
    }
}
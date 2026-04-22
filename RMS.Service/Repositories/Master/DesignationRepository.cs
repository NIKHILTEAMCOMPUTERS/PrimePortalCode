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
    public class DesignationRepository : GenericRepository<Designation>, IDesignationRepository
    {
        private readonly RmsDevContext _context;
        private readonly IHostingEnvironment _env;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public DesignationRepository(RmsDevContext context, IHostingEnvironment env, IHttpContextAccessor httpContextAccessor) : base(context)
        {
            _context = context;
            _env = env;
            _httpContextAccessor = httpContextAccessor;

        }

        public async Task<List<Designation>> GetDesignation()
        {
            var result = new List<Designation>();
            try
            {

                result = await _context.Designations.AsNoTracking().Where(x => x.Isdeleted != true && x.Isactive != false)
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
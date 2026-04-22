using Microsoft.AspNetCore.Http;
using RMS.Data.Models;
using RMS.Entity.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Service.Interfaces.Master
{
    public interface IBranchRepository : IGenericRepository<Branch>
    {

        public Task<List<Branch>> GetBranch();

    }
}

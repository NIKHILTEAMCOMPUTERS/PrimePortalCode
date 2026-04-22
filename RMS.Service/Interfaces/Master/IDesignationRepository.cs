using RMS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Service.Interfaces.Master
{
    public interface IDesignationRepository : IGenericRepository<Designation>
    {

        public Task<List<Designation>> GetDesignation();
    }
}

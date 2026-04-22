using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Entity.DTO
{
    public class JwtLoginDetailDto
    {   
        public string TmcId { get; set; }
        public string EmailId { get; set; }
        public string Name { get; set; }
        public string Contact { get; set; }
        public string Designation { get; set; }
   

    }
}

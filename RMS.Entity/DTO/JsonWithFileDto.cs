using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Entity.DTO
{
    public class JsonWithFileDto
    {
        public string JasonData { get; set; }
        public List<IFormFile> Files { get; set; }
    }

}

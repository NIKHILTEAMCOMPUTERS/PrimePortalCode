using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Entity.DTO
{
    public class TemplateDTO
    {
        public int Templateid { get; set; }

        public int Employeeid { get; set; }

        public string Templatename { get; set; } = null!;

        public string Totalhours { get; set; } = null!;

        public bool? Isactive { get; set; }

        public DateTime Createddate { get; set; }

        public DateTime Lastupdatedate { get; set; }

        public int Createdby { get; set; }

        public int Lastupdateby { get; set; }
        public List<TemplateDetailDTO> templatedetails { get; set; }

    }
    public class TemplateDetailDTO
    {
        public int? Templatedetailid { get; set; }

        public int? Templateid { get; set; }

        public int Departmentid { get; set; }

        public int Projectid { get; set; }

        public string Activity { get; set; } = null!;

        public string Hours { get; set; } = null!;
        public string? Remarks { get; set; }

    }
}


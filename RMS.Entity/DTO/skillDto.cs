using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Entity.DTO
{
    public class SkillRequestDto
    {
        public int?SkillId { get; set; }
        public string SkillName { get; set; }
        public string ActionType { get; set; }
    }

    public partial class EmployeeskillDto
    {
        public int? Employeeskillid { get; set; }

        public int Employeeid { get; set; }

        public int Skillid { get; set; }

        public decimal? Experinceinmonths { get; set; }

        public string? Certificationurl { get; set; }

        public bool? Isprimary { get; set; }

        public decimal? Managerrating { get; set; }

        public decimal? Selfreting { get; set; }      

        public DateTime? Createddate { get; set; }

        public DateTime? Lastupdatedate { get; set; }

        public int? Createdby { get; set; }

        public int? Lastupdateby { get; set; }


    }









}

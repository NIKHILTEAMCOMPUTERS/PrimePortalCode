using RMS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Entity.DTO
{
    public class TimeSheetItemDto
    {

        public int ?Timesheetdetailid { get; set; }
        public int? ProjectId { get; set; }

        public int Timesheetid { get; set; }

        public int? Departmentid { get; set; }

        public int ?Contractid { get; set; }

        public string Activity { get; set; } = null!;

        public decimal? Dayhours { get; set; }

        public string? Remarks { get; set; }

        public bool? Isdrafted { get; set; }

        public bool? Isactive { get; set; }

        public bool Isdeleted { get; set; }

        public DateTime? Createddate { get; set; }

        public DateTime? Lastupdatedate { get; set; }

        public int? Createdby { get; set; }

        public int? Lastupdateby { get; set; }

        public DateTime? Timesheetdate { get; set; }
        public int? categoryofactivityid { get; set; }
        public string? categoryofactivityname { get; set; }
    }
    
}

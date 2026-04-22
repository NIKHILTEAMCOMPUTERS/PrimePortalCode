using Microsoft.AspNetCore.Http;
using RMS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Entity.DTO
{
    public class CostSheetDto
    {
        public int? Costsheetid { get; set; }
        public string Costsheetname { get; set; } = null!;
        public decimal? AverageXvalue { get; set; }
        public string? Orderdescription { get; set; }
        public int? Oafid { get; set; }
        public decimal? TotalAmount { get; set; }

        public List<CostsheetdetailDto> Costsheetdetails { get; set; }
        public List<GroupedCostsheetDetailDto> CostsheetHistory { get; set; }
    }

    public partial class CostsheetdetailDto
    {
        public int? Costsheetdetailid { get; set; }
        public int? Costsheetid { get; set; }
        public int  Skillid { get; set; }
        public string? skillexperience { get; set; }
        public int? Requiredresource { get; set; }
        public decimal? Skillcost { get; set; }      
        public string Skillname { get; set; } = null!;
        public decimal? Xvalue { get; set; }

        public decimal? Perioddays { get; set; }

        public decimal? Customerprice { get; set; }

        public decimal? Totalcost { get; set; }

        public decimal? Totalprice { get; set; }
       

    }
    public class GroupedCostsheetDetailDto
    {
        public int? Costsheetid { get; set; }
        public int? Version { get; set; }
        public List<CostsheetdetailDto> Costsheetdetails { get; set; }
    }
    public class CostSheetCheckDto
    {
        public int CostSheetid { get; set; }
        public string? CostSheetName { get; set; }
    }




}

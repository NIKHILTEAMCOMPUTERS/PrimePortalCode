using Microsoft.AspNetCore.Mvc.Rendering;
using RMS.Client.Models.Oaf;

namespace RMS.Client.ViewModels
{
    public class UpsertOafViewModel
    {
        public Oaf OafData { get; set; }
        public List<SelectListItem> Costsheets { get; set; }
        public List<SelectListItem> Customers { get; set; }
        public List<SelectListItem> ContractTypes { get; set; }
        public List<SelectListItem> ProjectTypes { get; set; }
        public List<SelectListItem> Practices { get; set; }
        public List<SelectListItem> Subpractices { get; set; }
        public List<SelectListItem> AccountManagers { get; set; }
    }
}

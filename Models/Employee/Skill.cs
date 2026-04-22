namespace RMS.Client.Models.Employee
{
    public class SkillCosting
    {
        public int skillcostingid { get; set; }
        public int skillid { get; set; }
        public string expname { get; set; }
        public string skillexperience { get; set; }
        public decimal amount { get; set; }
        public string skillname { get; set; }
        public string requiredresource { get; set; }
        public string skillcost { get; set; }
        
    }

    public class Skill
    {
        public int skillid { get; set; }
        public string skillname { get; set; }
        public bool isactive { get; set; }
        public bool isdeleted { get; set; }
        public DateTime createddate { get; set; }
        public DateTime lastupdatedate { get; set; }
        public int createdby { get; set; }
        public int lastupdateby { get; set; }
        public List<object> costsheetdetails { get; set; }
        public List<SkillCosting> skillcostings { get; set; }
    }
}

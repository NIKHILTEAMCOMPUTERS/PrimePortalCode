namespace RMS.Client.Models.Projection
{
    public class ModelSkillExp
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public List<SkillData> Data { get; set; }
        public object ErrorDetails { get; set; }
    }
    public class SkillData
    {
        public int SkillId { get; set; }
        public string Skill { get; set; }
        public List<string> SkillExp { get; set; }
    }
}

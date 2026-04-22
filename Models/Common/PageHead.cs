namespace RMS.Client.Models.Common
{
    public class PageHead
    {
        public string PageHeading { get; set; }
        public bool WritePermission { get; set; }
        public string LinkText { get; set; }
        public string LinkController { get; set; }
        public string LinkAction { get; set; }
        public bool ShowCreateButton { get; set; } = true;
        public List<string> Columns { get; set; }
        public bool extracolumn { get; set; }
    }
}
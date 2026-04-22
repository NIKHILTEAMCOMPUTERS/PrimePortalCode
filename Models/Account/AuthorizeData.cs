namespace RMS.Client.Models.Account
{
    public class AuthorizeData
    {
        public List<AuthorizeRoles> AuthorizeRoles { get; set; }
    }
    public class AuthorizeRoles
    {
        public string RoleName { get; set; }
        public List<AuthorizePages> AuthorizePages { get; set; }
    }

    public class AuthorizePages
    {
        public string ModuleName { get; set; }
        public string ModuleIcon { get; set; }
        public string PageName { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string PageIcon { get; set; }
        public bool? Isreadpermit { get; set; }
        public bool? Iswritepermit { get; set; }
        public bool? Isdeletepermit { get; set; }
        public bool? Isrowlevelpermit { get; set; }
        public bool? IsBillingpermit { get; set; }
        public int Pagesequence { get; set; }
    }
}

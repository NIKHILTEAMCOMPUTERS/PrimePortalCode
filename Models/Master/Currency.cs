namespace RMS.Client.Models.Master
{
    public class Currency
    {
        public int CurrencyId { get; set; }
        public string CurrencyName { get; set; }
        public string Abbreviation { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public int CreatedBy { get; set; }
        public int LastUpdateBy { get; set; }

    }
}

namespace RMS.Client.Models.Master
{
    public class Country
    {
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public int CreatedBy { get; set; }
        public int LastUpdateBy { get; set; }
        public string CountryCode { get; set; }
    }

    public class State
    {
        public int StateId { get; set; }
        public int CountryId { get; set; }
        public string StateName { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public int CreatedBy { get; set; }
        public int LastUpdateBy { get; set; }
        public string StateCode { get; set; }

    }
    public class City
    {
        public int CityId { get; set; }
        public int StateId { get; set; }
        public string CityName { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public int CreatedBy { get; set; }
        public int LastUpdateBy { get; set; }

    }
}

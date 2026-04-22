namespace RMS.Client.Models.Master
{
    public class Practice
    {
        public int? Practiceid { get; set; }

        public string? Practicename { get; set; } = null!;

        public int? Statusid { get; set; }

        public bool? Isactive { get; set; }

        public bool Isdeleted { get; set; }

        public DateTime Createddate { get; set; }

        public DateTime Lastupdatedate { get; set; }

        public int Createdby { get; set; }

        public int Lastupdateby { get; set; }

        public string Code { get; set; }


    }
}

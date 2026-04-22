namespace RMS.Client.Models.Master
{
    public class Subpractice
    {
        public int Subpracticeid { get; set; }

        public int? Practiceid { get; set; }

        public string Subpracticename { get; set; } = null!;

        public int? Statusid { get; set; }

        public bool? Isactive { get; set; }

        public bool Isdeleted { get; set; }

        public DateTime Createddate { get; set; }

        public DateTime Lastupdatedate { get; set; }

        public int Createdby { get; set; }

        public int Lastupdateby { get; set; }
        public Practice? practice {  get; set; }


    }
}

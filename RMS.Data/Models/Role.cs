using System;
using System.Collections.Generic;

namespace RMS.Data.Models;

public partial class Role
{
    public int Roleid { get; set; }

    public string Rolename { get; set; } = null!;

    public bool? Isactive { get; set; }

    public bool Isdeleted { get; set; }

    public DateTime Createddate { get; set; }

    public DateTime Lastupdatedate { get; set; }

    public int Createdby { get; set; }

    public int Lastupdateby { get; set; }

    public bool? Isadmin { get; set; }

    public bool? Isrowlevel { get; set; }

    public virtual ICollection<Employeerole> Employeeroles { get; set; } = new List<Employeerole>();

    public virtual ICollection<Rolepage> Rolepages { get; set; } = new List<Rolepage>();
}

using System;
using System.Collections.Generic;

namespace RMS.Data.Models;

public partial class Userloginlog
{
    public int Userloginlogid { get; set; }

    public int Userid { get; set; }

    public string? Ipaddress { get; set; }

    public DateTime Logindatetime { get; set; }

    public string? Jwttoken { get; set; }

    public string? Loginstatus { get; set; }

    public string? Browserclient { get; set; }
}

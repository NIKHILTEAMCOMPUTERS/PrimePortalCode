using System;
using System.Collections.Generic;

namespace RMS.Data.Models;

public partial class CrmUser
{
    public int Id { get; set; }

    public long? CrmUsersid { get; set; }

    public string? DisplayName { get; set; }

    public string? Email { get; set; }

    public bool? IsActive { get; set; }

    public string? WorkNumber { get; set; }

    public string? MobileNumber { get; set; }
}

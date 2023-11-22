using System;
using System.Collections.Generic;

namespace She_He_Store.Models;

public partial class Role
{
    public decimal Roleid { get; set; }

    public string? Rolename { get; set; }

    public virtual ICollection<Userlogin> Userlogins { get; set; } = new List<Userlogin>();
}

using System;
using System.Collections.Generic;

namespace She_He_Store.Models;

public partial class Userlogin
{
    public decimal Userloginid { get; set; }

    public decimal? Userid { get; set; }

    public decimal? Roleid { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public virtual Role? Role { get; set; }

    public virtual User? User { get; set; }
}

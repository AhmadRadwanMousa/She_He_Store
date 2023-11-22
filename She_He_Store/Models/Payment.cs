using System;
using System.Collections.Generic;

namespace She_He_Store.Models;

public partial class Payment
{
    public decimal Paymentid { get; set; }

    public decimal? Userid { get; set; }

    public DateTime Paymentdate { get; set; }

    public string? Paymentstatus { get; set; }

    public decimal? Cardid { get; set; }

    public virtual Card? Card { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual User? User { get; set; }
}

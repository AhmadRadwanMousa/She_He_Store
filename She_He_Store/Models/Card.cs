using System;
using System.Collections.Generic;

namespace She_He_Store.Models;

public partial class Card
{
    public decimal Cardid { get; set; }

    public string Cardholdername { get; set; } = null!;

    public decimal Expirtydate { get; set; }

    public decimal Cvv { get; set; }

    public decimal? Cardamount { get; set; }

    public decimal? Cardnumber { get; set; }

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}

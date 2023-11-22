using System;
using System.Collections.Generic;

namespace She_He_Store.Models;

public partial class Review
{
    public decimal? Userid { get; set; }

    public string? Reviewcomment { get; set; }

    public decimal? Productid { get; set; }

    public DateTime? Reviewat { get; set; }

    public string? Username { get; set; }

    public decimal? Rating { get; set; }

    public decimal Reviewid { get; set; }

    public virtual Product? Product { get; set; }

    public virtual User? User { get; set; }
}

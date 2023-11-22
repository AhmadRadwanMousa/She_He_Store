using System;
using System.Collections.Generic;

namespace She_He_Store.Models;

public partial class Testimonial
{
    public decimal Testimonialid { get; set; }

    public string? Testimonialcomment { get; set; }

    public string? Testimonialstate { get; set; }

    public decimal? Userid { get; set; }

    public string? Username { get; set; }

    public virtual User? User { get; set; }
}

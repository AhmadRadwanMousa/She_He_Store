using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace She_He_Store.Models;

public partial class User
{
    public decimal Userid { get; set; }

    public string Fname { get; set; } = null!;

    public string Lname { get; set; } = null!;

    public DateTime Dateofbirth { get; set; }

    public string Phonenumber { get; set; } = null!;

    public string Country { get; set; } = null!;

    public string City { get; set; } = null!;

    public decimal? Postalcode { get; set; }

    public string? Gender { get; set; }

    public string? Profilepicture { get; set; }

    public decimal? Totalbalance { get; set; }

    public DateTime? Createdat { get; set; }

    public DateTime? Updatedat { get; set; }

    public string? Userstatus { get; set; }
	[NotMapped]
	public IFormFile? ImageFile { get; set; }
	public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual ICollection<Testimonial> Testimonials { get; set; } = new List<Testimonial>();

    public virtual ICollection<Userlogin> Userlogins { get; set; } = new List<Userlogin>();

    public virtual ICollection<Wishlist> Wishlists { get; set; } = new List<Wishlist>();
}

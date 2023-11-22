using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace She_He_Store.Models;

public partial class Product
{
    public decimal Productid { get; set; }

    public string Productname { get; set; } = null!;

    public decimal Productprice { get; set; }

    public decimal? Categoryid { get; set; }

    public string? Productpicture { get; set; }

    public string? Description { get; set; }

    public decimal? Rating { get; set; }

    public DateTime? Addingdate { get; set; }

    public bool? Isavaliable { get; set; }

    public decimal? Stockquantity { get; set; }

    public decimal? Sale { get; set; }

    public string? Productdetails { get; set; }
	[NotMapped]
	public IFormFile? ImageFile { get; set; }
	public virtual Category? Category { get; set; }

    public virtual ICollection<Orderitem> Orderitems { get; set; } = new List<Orderitem>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual ICollection<Wishlist> Wishlists { get; set; } = new List<Wishlist>();
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace She_He_Store.Models;

public partial class Category
{
    public decimal Categoryid { get; set; }

    public string? Categoryname { get; set; }

    public string? Categorypicture { get; set; }
	[NotMapped]
	public IFormFile? ImageFile { get; set; }

	public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}

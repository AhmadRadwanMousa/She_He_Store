using System;
using System.Collections.Generic;

namespace She_He_Store.Models;

public partial class Orderitem
{
    public decimal Orderitemid { get; set; }

    public decimal? Orderid { get; set; }

    public decimal? Productid { get; set; }

    public decimal? Quantity { get; set; }

    public decimal? Totalprice { get; set; }

    public virtual Order? Order { get; set; }

    public virtual Product? Product { get; set; }
}

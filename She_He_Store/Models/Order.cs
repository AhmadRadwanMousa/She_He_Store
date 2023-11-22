using System;
using System.Collections.Generic;

namespace She_He_Store.Models;

public partial class Order
{
    public decimal Orderid { get; set; }

    public decimal? Userid { get; set; }

    public DateTime? Orderdate { get; set; }

    public decimal? Totalamount { get; set; }

    public string? Orderstatus { get; set; }

    public string? Shippingaddress { get; set; }

    public decimal? Paymentid { get; set; }

    public virtual ICollection<Orderitem> Orderitems { get; set; } = new List<Orderitem>();

    public virtual Payment? Payment { get; set; }

    public virtual User? User { get; set; }
}

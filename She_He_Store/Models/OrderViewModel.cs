namespace She_He_Store.Models
{
    public class OrderViewModel
    {
        public Order Order { get; set; }
        public IEnumerable<OrderItemWithProduct> OrderItems { get; set; }
    }
}
